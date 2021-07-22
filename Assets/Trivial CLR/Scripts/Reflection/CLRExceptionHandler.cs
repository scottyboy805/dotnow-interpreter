using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace TrivialCLR.Reflection
{
    public class CLRExceptionHandler
    {
        // Private
        private AppDomain domain = null;
        private CLRMethodBodyBase declaringMethodBody = null;
        private ExceptionHandler handler = null;
        private Lazy<Type> exceptionType = null;

        // Public
        public readonly int tryStartIndex = 0;
        public readonly int tryEndIndex = 0;
        public readonly int handlerStartIndex = 0;
        public readonly int handlerEndIndex = 0;

        // Properties
        public bool IsFinally
        {
            get { return handler.HandlerType == ExceptionHandlerType.Finally; }
        }

        public Type ExceptionType
        {
            get { return exceptionType.Value; }
        }

        // Constructor
        internal CLRExceptionHandler(AppDomain domain, CLRMethodBodyBase declaringMethodBody, MethodBody body, ExceptionHandler handler)
        {
            this.domain = domain;
            this.declaringMethodBody = declaringMethodBody;
            this.handler = handler;

            // Handler bounds
            this.tryStartIndex = body.Instructions.IndexOf(handler.TryStart);// handler.TryStart;
            this.tryEndIndex = body.Instructions.IndexOf(handler.TryEnd);
            this.handlerStartIndex = body.Instructions.IndexOf(handler.HandlerStart);
            this.handlerEndIndex = body.Instructions.IndexOf(handler.HandlerEnd);

            // Initialize exception type
            this.exceptionType = new Lazy<Type>(() => (handler.CatchType != null) ? domain.ResolveType(handler.CatchType) : null);
        }

        // Methods
        public bool IsMatch(int index)
        {
            if (index < tryStartIndex || index >= tryEndIndex)
                return false;

            return true;
        }

        public bool IsMatch(Type exceptionType, int index)
        {
            // Check out of bounds
            if (index < tryStartIndex || index >= tryEndIndex)
                return false;

            // Note that exception type can be null (catch all)
            if (ExceptionType == null || ExceptionType.IsAssignableFrom(exceptionType) == true)
                return true;

            return false;
        }

        public bool IsBetterMatchThan(CLRExceptionHandler other)
        {
            // Check for error case
            if (other == null)
                return true;

            if (tryStartIndex == other.tryStartIndex && tryEndIndex == other.tryEndIndex)
                return handlerStartIndex < other.handlerStartIndex;

            if(tryStartIndex > other.tryStartIndex)
            {
                Debug.Assert(tryEndIndex <= other.tryEndIndex);
                return true;
            }

            if(tryEndIndex < other.tryEndIndex)
            {
                Debug.Assert(tryStartIndex == other.tryStartIndex);
                return true;
            }

            return false;
        }

        internal bool IsInside(int index)
        {
            return index >= tryStartIndex && index < tryEndIndex;
        }
    }
}
