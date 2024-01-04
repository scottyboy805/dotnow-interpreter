using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using dotnow.Reflection;
using dotnow.Runtime.CIL;

namespace dotnow.Runtime
{
    internal sealed class ExecutionMethod
    {
        // Private
        private AppDomain domain = null;
        private CILSignature signature = null;
        private MethodBase method = null; 
        private CLRMethodBodyBase body = null;
        private bool isStatic = false;
        private bool isCtor = false;

        // Constructor
        public ExecutionMethod(AppDomain domain, CILSignature signature, MethodBase method, CLRMethodBodyBase body, bool isStatic, bool isCtor)
        {
            this.domain = domain;
            this.signature = signature;
            this.method = method;
            this.body = body;
            this.isStatic = isStatic;
            this.isCtor = isCtor;
        }

        // Methods
        public object ReflectionInvoke(object obj, object[] parameters)
        {
            // Get executing engine for current thread
            ExecutionEngine engine = domain.GetExecutionEngine();

            // Get locals
            StackLocal[] locals = (body.InitLocals == true) ? body.Locals : null;

            // Get input counts
            int instanceCount = (isStatic == false) ? 1 : 0;
            int paramCount = (parameters != null) ? parameters.Length : 0;

            // Get method frame
            ExecutionFrameOld frame;
            engine.AllocExecutionFrame(out frame, domain, engine, method, body.MaxStack, paramCount, locals);


            // Push instance
            if(instanceCount > 0)
            {
                frame.stack[frame.stackIndex].refValue = obj;
                frame.stack[frame.stackIndex++].type = StackData.ObjectType.Ref;
            }

            // ### More work required to copy value types
            // Push parameters
            for (int i = 0; i < paramCount; i++)
                StackData.AllocTyped(ref frame.stack[frame.stackIndex++], signature.parameterTypeInfos[i], parameters[i]);


            // Execute method body
            body.ExecuteMethodBody(engine, frame);



            // Load return type
            if(isCtor == false && signature.returnsValue == true)
            {
                //// Get return value from stack
                //StackData returnVal = frame.stack[--frame.stackIndex];

                //// Release the frame
                //engine.FreeExecutionFrame(frame);

                //// Get return object
                //return returnVal.UnboxAsType(signature.returnType);
                unsafe
                {
                    int returnSize = __memory.SizeOfTypedSlow(signature.returnType.type);

                    //fixed (byte* returnPtr = &frame.stackMemory[frame.stackIndex - returnSize + 1])
                    byte* returnPtr = (byte*)frame.stackMemory;
                    {
                        // Get the value

                        // Release the frame
                        engine.FreeExecutionFrame(frame);

                        return *(int*)returnPtr;
                    }
                }
            }


            // Release the frame
            engine.FreeExecutionFrame(frame);
            return null;
        }        

        internal void DelegateInvoke(object obj)
        {
            // Get executing engine for current thread
            ExecutionEngine engine = domain.GetExecutionEngine();

            // Get locals
            StackLocal[] locals = (body.InitLocals == true) ? body.Locals : null;

            // Get input counts
            int instanceCount = (isStatic == false) ? 1 : 0;

            // Get method frame
            ExecutionFrameOld frame;
            engine.AllocExecutionFrame(out frame, domain, engine, method, body.MaxStack, 0, locals);

            // Push instance
            if (instanceCount > 0)
            {
                frame.stack[frame.stackIndex].refValue = obj;
                frame.stack[frame.stackIndex++].type = StackData.ObjectType.Ref;
            }

            // Execute method body
            body.ExecuteMethodBody(engine, frame);

            // Release the frame
            engine.FreeExecutionFrame(frame);
        }
    }
}
