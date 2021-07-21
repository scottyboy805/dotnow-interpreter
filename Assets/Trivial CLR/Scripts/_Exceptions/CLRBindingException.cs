using System;

namespace TrivialCLR
{
    public class CLRBindingException : Exception
    {
        // Constructor
        public CLRBindingException(string message)
            : base(message)
        {
        }

        public CLRBindingException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
