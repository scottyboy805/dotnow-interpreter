using System;

namespace dotnow
{
    public sealed class CLRRuntimeException : Exception
    {
        // Constructor
        public CLRRuntimeException(string message)
            : base(message)
        { 
        }
    }
}
