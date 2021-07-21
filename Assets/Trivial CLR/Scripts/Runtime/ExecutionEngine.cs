using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrivialCLR.Debugging;
using TrivialCLR.Reflection;
using TrivialCLR.Runtime.CIL;

namespace TrivialCLR.Runtime
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
            //public Instruction[] methodInstructions;
            public CILOperation[] instructions;
            public CLRExceptionHandler[] exceptionHandlers;
            public int instructionPointer;
        }

        // Internal
        internal Dictionary<int, object[]> argumentCache = new Dictionary<int, object[]>();

        internal StackData[] stack = null;

        // Private        
        private bool shouldPauseDebugger = false;
        private bool isDebuggerPaused = false;
        private bool debugStepOnce = false;
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
            int stackSize = (maxStack != 0) ? maxStack : defaultStackSize;

            this.thread = thread;
            this.stack = new StackData[stackSize];
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

                //shouldPauseDebugger = true;
            }
        }

        public void ContinueExecution()
        {
            if (isDebuggerPaused == true && debugger != null)
            {
                // Remove pause flag
                debugFlags &= ~DebugFlags.DebugPause;

                //shouldPauseDebugger = false;
                isDebuggerPaused = false;

                if (debuggerPauseState != null)
                {
                    Execute(debuggerPauseState.domain, debuggerPauseState.frame, debuggerPauseState.instructions, debuggerPauseState.exceptionHandlers, debuggerPauseState.instructionPointer, true);
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

                    debugStepOnce = true;
                    Execute(debuggerPauseState.domain, debuggerPauseState.frame, debuggerPauseState.instructions, debuggerPauseState.exceptionHandlers, debuggerPauseState.instructionPointer, true);
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
                //for (int i = 0; i < frame.Method.GetParameters().Length + ((frame.Method.IsStatic == true) ? 0 : 1); i++)
                //    debugFrame.AddArgumentVariable(i);

                //// Add locals
                //for (int i = 0; i < frame.Method.GetMethodBody().LocalVariables.Count; i++)
                //    debugFrame.AddLocalVariable(i);

                // Send frame to debugger
                debugger.OnDebugFrame(debugFrame);
            }

            // Clear step once flag
            debugFlags &= ~DebugFlags.DebugStepOnce;

            //debugStepOnce = false;
        }

        internal void Execute(AppDomain domain, ExecutionFrame frame, CILOperation[] methodInstructions, CLRExceptionHandler[] exceptionHandlers, int initialInstructionIndex = 0, bool debuggerResume = false)
        {
            // Check for paused execution
            if (isDebuggerPaused == true && debuggerResume == false)
                return;

            //int index = initialInstructionIndex;

            // Keep executing until complete, an exceptio is raised, or we return gracefully
            while (true)
            {
                try
                {
                    CILInterpreter.ExecuteInterpreted(domain, this, ref frame, ref methodInstructions, ref exceptionHandlers, debugFlags);

                    //if(frame.leave == true)
                    //{
                    //    int leaveInstructionTarget = frame.instructionPtr + frame.leaveOffset - 1;

                    //    // Run finally handler
                    //    if (frame.leaveHandler == null)
                    //        throw new ExecutionEngineException("A finally filter block was expected at the current execution stage");

                    //    // Go to handler start index
                    //    frame.instructionPtr = frame.leaveHandler.handlerStartIndex;

                    //    // Empty stack
                    //    frame.stackIndex = frame.stackBaseIndex;
                    //    frame.leave = false;
                    //    frame.abort = false;

                    //    // Execute finally
                    //    CILInterpreter.ExecuteInterpreted(domain, this, ref frame, ref methodInstructions, ref exceptionHandlers, debugFlags);
                                               

                    //    // Return execution to the leave target
                    //    frame.instructionPtr = leaveInstructionTarget;

                    //    // Continue execution after leaving protected region
                    //    CILInterpreter.ExecuteInterpreted(domain, this, ref frame, ref methodInstructions, ref exceptionHandlers, debugFlags);
                    //}
                    return;
                }
                catch (Exception e)
                {
#if UNITY
                    UnityEngine.Debug.LogError("At method body: " + ((frame.Method != null) ? frame.Method.ToString() : "<Unknown>"));
                    UnityEngine.Debug.LogError("At instruction: " + ((frame.instructionPtr < methodInstructions.Length) ? methodInstructions[frame.instructionPtr].ToString() : "<Unknown>"));
#endif

                    // Handle any exceptions
                    switch (HandleException(domain, frame, methodInstructions, exceptionHandlers, e))
                    {
                        case ExceptionHandlingResult.Rethrow: throw;
                        case ExceptionHandlingResult.Continue: continue;
                        case ExceptionHandlingResult.Return: return;
                    }
                }


            //while (true)
            //{
            //    try
            //    {
            //        index = frame.instructionPtr;

            //        while (index < methodInstructions.Length && frame.abort == false && shouldPauseDebugger == false)
            //        {
            //            // Run the instruction
            //            index += methodInstructions[index].Run(frame);
            //            frame.instructionPtr = index;

            //            if (debugStepOnce == true)
            //                break;
            //        }

            //        // Check for paused
            //        if (shouldPauseDebugger == true || debugStepOnce == true)
            //        {
            //            isDebuggerPaused = true;
            //            debuggerPauseState = new ExecutionState
            //            {
            //                frame = frame,
            //                methodInstructions = methodInstructions,
            //                exceptionHandlers = exceptionHandlers,
            //                instructionPointer = index,
            //            };

            //            // Create debugger frame
            //            if (debugger != null && (frame.Method is CLRMethod || frame.Method is CLRConstructor))
            //            {
            //                // Create debug frame
            //                DebugFrame debugFrame = new DebugFrame(frame.Method, frame, methodInstructions, index);

            //                // Add args
            //                //for (int i = 0; i < frame.Method.GetParameters().Length + ((frame.Method.IsStatic == true) ? 0 : 1); i++)
            //                //    debugFrame.AddArgumentVariable(i);

            //                //// Add locals
            //                //for (int i = 0; i < frame.Method.GetMethodBody().LocalVariables.Count; i++)
            //                //    debugFrame.AddLocalVariable(i);

            //                // Send frame to debugger
            //                debugger.OnDebugFrame(debugFrame);
            //            }
            //            debugStepOnce = false;
            //        }

//                    return;
//                }
//                catch (Exception e)
//                {
//                    //UnityEngine.Debug.LogException(e);

//#if UNITY
//                    UnityEngine.Debug.LogError("At method body: " + ((frame.Method != null) ? frame.Method.ToString() : "<Unknown>"));
//                    UnityEngine.Debug.LogError("At instruction: " + ((index < methodInstructions.Length) ? methodInstructions[index].ToString() : "<Unknown>"));
//#endif

//                    // Handle any exceptions
//                    switch (HandleException(frame, methodInstructions, exceptionHandlers, e))
//                    {
//                        case ExceptionHandlingResult.Rethrow: throw;
//                        case ExceptionHandlingResult.Continue: continue;
//                        case ExceptionHandlingResult.Return: return;
//                    }
//                }
            }
        }

        //        public void Execute(ExecutionFrame frame, Instruction[] methodInstructions, CLRExceptionHandler[] exceptionHandlers, int initialInstructionIndex = 0, bool debuggerResume = false)
        //        {
        //            // Check for paused execution
        //            if (isDebuggerPaused == true && debuggerResume == false)
        //                return;

        //            int index = initialInstructionIndex;

        //            while(true)
        //            {
        //                try
        //                {
        //                    index = frame.instructionPtr;

        //                    while(index < methodInstructions.Length && frame.abort == false && shouldPauseDebugger == false)
        //                    {
        //                        // Run the instruction
        //                        index += methodInstructions[index].Run(frame);
        //                        frame.instructionPtr = index;

        //                        if (debugStepOnce == true)
        //                            break;
        //                    }

        //                    // Check for paused
        //                    if(shouldPauseDebugger == true || debugStepOnce == true)
        //                    {
        //                        isDebuggerPaused = true;
        //                        debuggerPauseState = new ExecutionState
        //                        {
        //                            frame = frame,
        //                            methodInstructions = methodInstructions,
        //                            exceptionHandlers = exceptionHandlers,
        //                            instructionPointer = index,
        //                        };

        //                        // Create debugger frame
        //                        if(debugger != null && (frame.Method is CLRMethod || frame.Method is CLRConstructor))
        //                        {
        //                            // Create debug frame
        //                            DebugFrame debugFrame = new DebugFrame(frame.Method, frame, methodInstructions, index);

        //                            // Add args
        //                            //for (int i = 0; i < frame.Method.GetParameters().Length + ((frame.Method.IsStatic == true) ? 0 : 1); i++)
        //                            //    debugFrame.AddArgumentVariable(i);

        //                            //// Add locals
        //                            //for (int i = 0; i < frame.Method.GetMethodBody().LocalVariables.Count; i++)
        //                            //    debugFrame.AddLocalVariable(i);

        //                            // Send frame to debugger
        //                            debugger.OnDebugFrame(debugFrame);
        //                        }
        //                        debugStepOnce = false;
        //                    }

        //                    return;
        //                }
        //                catch(Exception e)
        //                {
        //                    //UnityEngine.Debug.LogException(e);

        //#if UNITY
        //                    UnityEngine.Debug.LogError("At method body: " + ((frame.Method != null) ? frame.Method.ToString() : "<Unknown>"));
        //                    UnityEngine.Debug.LogError("At instruction: " + ((index < methodInstructions.Length) ? methodInstructions[index].ToString() : "<Unknown>"));
        //#endif

        //                    // Handle any exceptions
        //                    switch (HandleException(frame, methodInstructions, exceptionHandlers, e))
        //                    {
        //                        case ExceptionHandlingResult.Rethrow: throw;
        //                        case ExceptionHandlingResult.Continue: continue;
        //                        case ExceptionHandlingResult.Return: return;
        //                    }
        //                }
        //            }
        //        }


        private ExceptionHandlingResult HandleException(AppDomain domain, ExecutionFrame frame, CILOperation[] instructions, CLRExceptionHandler[] exceptionHandlers, Exception e)
        {
            // Goto exception handler body
            frame.instructionPtr += GotoHandler(frame, e, exceptionHandlers, out CLRExceptionHandler handler);

            // Check for valid handler
            if (handler == null || handler.ExceptionType == null)
            {
                // Execute the frame
                //Execute(frame, instructions, exceptionHandlers);

                //if (instructions[frame.instructionPtr] is Throw) //frame.instructionPtr == rethrowOnReturn)
                return ExceptionHandlingResult.Rethrow;

                // Return from method
                //return ExceptionHandlingResult.Return;
            }

            while (true)
            {
                try
                {
                    CILInterpreter.ExecuteInterpreted(domain, this, ref frame, ref instructions, ref exceptionHandlers, debugFlags);
                    //int index = frame.instructionPtr;

                    //while (index < instructions.Length)
                    //{
                    //    // Get the instruction instance
                    //    Instruction currentInstruction = instructions[index];

                    //    // Run the instruction
                    //    index += currentInstruction.Run(frame);
                    //    frame.instructionPtr = index;

                    //    //if(currentInstruction is LeaveExceptionHandler)
                    //    //    return ExceptionHandlingResult.Continue;
                    //}

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

                        default: throw new Exception("Unreachable code was executed");
                    }
                }
            }
        }

        //private ExceptionHandlingResult HandleException(ExecutionFrame frame, Instruction[] instructions, CLRExceptionHandler[] exceptionHandlers, Exception e)
        //{
        //    // Goto exception handler body
        //    frame.instructionPtr += GotoHandler(frame, e, exceptionHandlers, out CLRExceptionHandler handler);

        //    // Check for valid handler
        //    if(handler == null || handler.ExceptionType == null)
        //    {
        //        // Execute the frame
        //        //Execute(frame, instructions, exceptionHandlers);

        //        //if (instructions[frame.instructionPtr] is Throw) //frame.instructionPtr == rethrowOnReturn)
        //            return ExceptionHandlingResult.Rethrow;

        //        // Return from method
        //        //return ExceptionHandlingResult.Return;
        //    }

        //    while(true)
        //    {
        //        try
        //        {
        //            int index = frame.instructionPtr;

        //            while(index < instructions.Length)
        //            {
        //                // Get the instruction instance
        //                Instruction currentInstruction = instructions[index];

        //                // Run the instruction
        //                index += currentInstruction.Run(frame);
        //                frame.instructionPtr = index;

        //                //if(currentInstruction is LeaveExceptionHandler)
        //                //    return ExceptionHandlingResult.Continue;
        //            }

        //            // Check for rethrow
        //            if (frame.instructionPtr == rethrowOnReturn)
        //                return ExceptionHandlingResult.Rethrow;

        //            // Default behaviour
        //            return ExceptionHandlingResult.Return;
        //        }
        //        catch(Exception nestedException)
        //        {
        //            switch(HandleException(frame, instructions, exceptionHandlers, nestedException))
        //            {
        //                case ExceptionHandlingResult.Rethrow: throw;
        //                case ExceptionHandlingResult.Continue: continue;
        //                case ExceptionHandlingResult.Return: return ExceptionHandlingResult.Return;

        //                default: throw new Exception("Unreachable code was executed");
        //            }
        //        }
        //    }
        //}

        internal int GotoHandler(ExecutionFrame frame, object exception, CLRExceptionHandler[] exceptionHandlers, out CLRExceptionHandler handler)
        {
            // try to get best matching exception handler
            handler = GetBestHandler(frame.instructionPtr, exceptionHandlers, exception.GetType());

            // Check for handler
            //if(handler == null)
            //{
            //    return 
            //}

            return 1;// frame.go
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
    }
}
