using System.Reflection;
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
            ExecutionFrame frame;
            engine.AllocExecutionFrame(out frame, domain, engine, method, body.MaxStack, paramCount, locals);

            // Push instance
            if (instanceCount > 0)
            {
                StackData.AllocTypedSlow(frame._heap, ref frame._stack[frame.stackIndex++], obj.GetType(), obj);
            }

            // ### More work required to copy value types
            // Push parameters
            for (int i = 0; i < paramCount; i++)
                StackData.AllocTyped(frame._heap, ref frame._stack[frame.stackIndex++], signature.parameterTypeInfos[i], parameters[i]);


            // Set heap size
            frame.heapSize = frame._heap.Size;

            // Execute method body
            body.ExecuteMethodBody(engine, frame);

            

            
            // Load return type
            if (isCtor == false && signature.returnsValue == true)
            {
                // Get return value from stack
                StackData returnVal = frame._stack[--frame.stackIndex];

                // Release the frame
                engine.FreeExecutionFrame(frame);

                // Get return object
                object result = returnVal.UnboxAsType(frame._heap, signature.returnType);

                // Free some memory
                //frame._heap.FreeMemory(frame._stack, frame.stackIndex);
                return result;
            }

            // Free some memory
            //frame._heap.FreeMemory(frame._stack, frame.stackIndex);

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
            ExecutionFrame frame;
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
