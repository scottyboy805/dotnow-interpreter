using System;

namespace TrivialCLR
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
