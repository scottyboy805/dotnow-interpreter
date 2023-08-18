using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading;
using dotnow.Debugging;
using dotnow.Reflection;
using dotnow.Runtime.CIL;

namespace dotnow.Runtime
{
    public class ExecutionEngine
    {
        [Flags]
        public enum DebugFlags
        {
            None = 0,
            DebuggerAttached = 1 << 1,
            DebugStepOnce = 1 << 2,
            DebugPause = 1 << 4,
        }

        public enum ExceptionHandlingResult
        {
            Rethrow,
            Continue,
            Return
        }

        internal class ExecutionState
        {
            // Public
            public AppDomain domain;
            public ExecutionFrame frame;
            public CILOperation[] instructions;
            public CLRExceptionHandler[] exceptionHandlers;
        }

        // Internal
        internal Dictionary<int, object[]> argumentCache = new Dictionary<int, object[]>();

        internal ExecutionFrame currentFrame = null;
        internal StackData[] stack = null;
        internal byte[] stackMemory = null;

        // Private        
        private static readonly FieldInfo exceptionStackTraceProperty = typeof(Exception).GetField("_stackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

        private Stack<ExecutionFrame> availableFrames = new Stack<ExecutionFrame>();

        private bool isDebuggerPaused = false;
        private ExecutionState debuggerPauseState = null;

        private Thread thread = null;
        private IDebugger debugger = null;
        private DebugFlags debugFlags = 0;

        // Public
        public const int defaultStackSize = 4096;
        public const int rethrowOnReturn = int.MaxValue;

        // Properties
        public bool IsDebuggerAttached
        {
            get { return (debugFlags & DebugFlags.DebuggerAttached) != 0; }
        }

        public bool IsDebuggerPaused
        {
            get { return isDebuggerPaused; }
        }

        // Constructor
        public ExecutionEngine(Thread thread, int maxStack = 0)
        {
            int stackSize = (maxStack > 0) ? maxStack : defaultStackSize;

            this.thread = thread;
            this.stack = new StackData[stackSize];
            this.stackMemory = new byte[stackSize];
        }

        // Methods
        internal void SetDebugger(IDebugger debugger)
        {
            this.debugger = debugger;
            this.debugFlags = 0;

            if (this.debugger != null)
                this.debugFlags |= DebugFlags.DebuggerAttached;
        }

        public void PauseExecution()
        {
            if (isDebuggerPaused == false && debugger != null)
            {
                debugFlags |= DebugFlags.DebugPause;
            }
        }

        public void ContinueExecution()
        {
            if (isDebuggerPaused == true && debugger != null)
            {
                // Remove pause flag
                debugFlags &= ~DebugFlags.DebugPause;

                isDebuggerPaused = false;

                if (debuggerPauseState != null)
                {
                    Execute(debuggerPauseState.domain, debuggerPauseState.frame, debuggerPauseState.instructions, debuggerPauseState.exceptionHandlers, debuggerPauseState.frame.instructionPtr, true);
                }
            }
        }

        public void StepExecution()
        {
            if (isDebuggerPaused == true && debugger != null)
            {
                // Remove pause flag
                debugFlags &= ~DebugFlags.DebugPause;

                //shouldPauseDebugger = false;
                isDebuggerPaused = false;

                if (debuggerPauseState != null)
                {
                    // Set step once flag
                    debugFlags |= DebugFlags.DebugStepOnce;
                    Execute(debuggerPauseState.domain, debuggerPauseState.frame, debuggerPauseState.instructions, debuggerPauseState.exceptionHandlers, debuggerPauseState.frame.instructionPtr, true);
                }
            }
        }

        internal void SaveExecutionState(AppDomain domain, ExecutionFrame frame, CILOperation[] instructions, CLRExceptionHandler[] exceptionHandlers)
        {
            isDebuggerPaused = true;
            debuggerPauseState = new ExecutionState
            {
                domain = domain,
                frame = frame,
                instructions = instructions,
                exceptionHandlers = exceptionHandlers,
            };

            // Create debugger frame
            if (debugger != null && (frame.Method is CLRMethod || frame.Method is CLRConstructor))
            {
                // Create debug frame
                DebugFrame debugFrame = new DebugFrame(frame.Method, frame, instructions, frame.instructionPtr);

                // Add args
                for (int i = 0; i < frame.Method.GetParameters().Length + ((frame.Method.IsStatic == true) ? 0 : 1); i++)
                    debugFrame.AddArgumentVariable(i);

                // Add locals
                for (int i = 0; i < frame.Method.GetMethodBody().LocalVariables.Count; i++)
                    debugFrame.AddLocalVariable(i);

                // Send frame to debugger
                debugger.OnDebugFrame(debugFrame);
            }

            // Clear step once flag
            debugFlags &= ~DebugFlags.DebugStepOnce;
        }

        internal void Execute(AppDomain domain, ExecutionFrame frame, CILOperation[] methodInstructions, CLRExceptionHandler[] exceptionHandlers, int initialInstructionIndex = 0, bool debuggerResume = false)
        {
            // Check for paused execution
            if (isDebuggerPaused == true && debuggerResume == false)
                return;

            // Keep executing until complete, an exception is raised, or we return gracefully
            while (true)
            {
                try
                {
                    CILInterpreterUnsafe.ExecuteInterpreted(domain, this, ref frame, ref methodInstructions, ref exceptionHandlers, debugFlags);
                    return;
                }
                catch (Exception e)
                {
#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
                    UnityEngine.Debug.LogError("Execution Error: " + e.Message);
                    UnityEngine.Debug.LogError("At method body: " + ((frame.Method != null) ? frame.Method.ToString() : "<Unknown>"));
                    UnityEngine.Debug.LogError("At instruction: " + ((frame.instructionPtr < methodInstructions.Length) ? methodInstructions[frame.instructionPtr].ToString() : "<Unknown>"));
#endif
#endif
                    // Reset call frame
                    if (currentFrame != null)
                        currentFrame = (currentFrame.Parent != null) ? currentFrame.Parent : null;

                    // Handle any exceptions
                    switch (HandleException(domain, frame, methodInstructions, exceptionHandlers, e))
                    {
                        case ExceptionHandlingResult.Rethrow: throw;
                        case ExceptionHandlingResult.Continue: continue;
                        case ExceptionHandlingResult.Return: return;
                    }
                }
            }
        }

        private ExceptionHandlingResult HandleException(AppDomain domain, ExecutionFrame frame, CILOperation[] instructions, CLRExceptionHandler[] exceptionHandlers, Exception e)
        {
            // Goto exception handler body
            CLRExceptionHandler handler;
            frame.instructionPtr += GotoHandler(frame, e, exceptionHandlers, out handler);

            // Check for valid handler
            if (handler == null || handler.ExceptionType == null)
            {
                return ExceptionHandlingResult.Rethrow;
            }

            while (true)
            {
                try
                {
                    CILInterpreterUnsafe.ExecuteInterpreted(domain, this, ref frame, ref instructions, ref exceptionHandlers, debugFlags);
                    
                    // Check for rethrow
                    if (frame.instructionPtr == rethrowOnReturn)
                        return ExceptionHandlingResult.Rethrow;

                    // Default behaviour
                    return ExceptionHandlingResult.Return;
                }
                catch (Exception nestedException)
                {
                    switch (HandleException(domain, frame, instructions, exceptionHandlers, nestedException))
                    {
                        case ExceptionHandlingResult.Rethrow: throw;
                        case ExceptionHandlingResult.Continue: continue;
                        case ExceptionHandlingResult.Return: return ExceptionHandlingResult.Return;

                        default: throw CreateException(new Exception("Unreachable code was executed"));
                    }
                }
            }
        }

        internal int GotoHandler(ExecutionFrame frame, object exception, CLRExceptionHandler[] exceptionHandlers, out CLRExceptionHandler handler)
        {
            // try to get best matching exception handler
            handler = GetBestHandler(frame.instructionPtr, exceptionHandlers, exception.GetType());
            return 1;
        }

        internal CLRExceptionHandler GetBestHandler(int instructionIndex, CLRExceptionHandler[] exceptionHandlers, Type exceptionType)
        {
            CLRExceptionHandler best = null;

            // Check all handlers
            foreach(CLRExceptionHandler handler in exceptionHandlers)
            {
                if(handler.IsMatch(exceptionType, instructionIndex) == true)
                {
                    if(handler.IsBetterMatchThan(best) == true)
                    {
                        best = handler;
                    }
                }
            }

            return best;
        }

        internal CLRExceptionHandler GetFinallyHandler(int instructionIndex, CLRExceptionHandler[] exceptionHandlers)
        {
            // Check all handlers
            foreach (CLRExceptionHandler handler in exceptionHandlers)
            {
                if(handler.IsFinally == true && handler.IsMatch(instructionIndex) == true)
                {
                    // Matched handler
                    return handler;
                }
            }
            return null;
        }

        internal void AllocExecutionFrame(out ExecutionFrame frame, AppDomain domain, ExecutionEngine engine, MethodBase method, int maxStack, int paramCount, StackLocal[] locals)
        {
            if (availableFrames.Count > 0)
            {
                // Use pooled frame
                frame = availableFrames.Pop();
                frame.SetupFrame(domain, engine, currentFrame, method, maxStack, paramCount, locals);
            }
            else
            {
                frame = new ExecutionFrame(domain, engine, currentFrame, method, maxStack, paramCount, locals);
            }

            // Set current
            currentFrame = frame;
        }

        internal void FreeExecutionFrame(ExecutionFrame frame)
        {
            lock (availableFrames)
            {
                availableFrames.Push(frame);

                // Pop current frame
                if (currentFrame == frame && frame != null)
                    currentFrame = frame.Parent;
            }
        }        

        public Exception CreateException(Exception e)
        {
            // Update stack trace to use interpreted stack
            if (e != null)
                PatchException(e);

            return e;
        }

        private void PatchException(Exception e)
        {
            // Check for null
            if (e == null)
                return;

            // Create stack trace
            string stackTrace = GetStackTrace(true);

            // Update exception
            exceptionStackTraceProperty.SetValue(e, stackTrace);
        }

        public string GetStackTrace(bool includeFileInfo)
        {
            StringBuilder builder = new StringBuilder();

            // Build from current frame
            BuildStackTrace(currentFrame, builder, includeFileInfo);

            // Get full string
            return builder.ToString();
        }

        private void BuildStackTrace(ExecutionFrame frame, StringBuilder builder, bool includeFileInfo)
        {
            // Get method from frame
            MethodBase currentMethod = frame.Method;

            // Add interpreted tag
            builder.Append("[Interpreted] ");

            // Get full method string
            string methodString = currentMethod.ToString();

            // Remove return type
            methodString = methodString.Remove(0, methodString.IndexOf(' ') + 1);

            // Append method info
            builder.Append(methodString);
            
            if(includeFileInfo == true)
            {
                CLRMethod method = currentMethod as CLRMethod;
                CILOperation op;

                if (method != null && frame.GetCurrentOperation(out op) == true)
                {
                    // Get debug location
                    DebugSymbolLocation location = method.GetDebugSymbolLocation(op);

                    // File name
                    builder.Append(" (at ");
                    builder.Append(location.fileName);

                    // Check for line number
                    if (location.lineNumber > 0)
                    {
                        builder.Append(":");
                        builder.Append(location.lineNumber);
                    }
                    builder.Append(")");
                }
                else
                {
                    builder.Append(" (at unknown)");
                }
            }

            // Check for parent
            if (frame.Parent != null)
            {
                builder.Append("\n");
                BuildStackTrace(frame.Parent, builder, includeFileInfo);
            }
        }

        //private FieldInfo stackTraceField = typeof(Exception).GetField("_stackTraceString", BindingFlags.NonPublic | BindingFlags.Instance);
        //private FieldInfo remoteStackTraceField = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.NonPublic | BindingFlags.Instance);
        //internal object InitExceptionCallstack(object o)
        //{
        //    if (o is Exception)
        //    {
        //        Exception e = o as Exception;

        //        string callstack = "Testing";

        //        stackTraceField.SetValue(e, callstack);
        //        remoteStackTraceField.SetValue(e, callstack);

        //        UnityEngine.Debug.Log("Stack trace: " + GetStackTrace(true));// e.StackTrace);
        //    }

        //    return o;
        //}
    }
}
