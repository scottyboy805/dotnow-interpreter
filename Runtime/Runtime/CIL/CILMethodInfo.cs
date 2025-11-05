using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dotnow.Runtime.CIL
{
    [Flags]
    internal enum CILMethodFlags : ushort
    {
        None = 0,
        Body = 1 << 0,
        This = 1 << 1,
        Ctor = 1 << 4,
        Virtual = 1 << 5,
        Abstract = 1 << 6,
        Interop = 1 << 7,
        Native = 1 << 8,
        Interpreted = 1 << 9,

        VoidCallDelegate = 1 << 10,
        DirectInstanceDelegate = 1 << 11,
        DirectCallDelegate = 1 << 12,
        DirectCallGenericDelegate = 1 << 13,

        InternalCall = 1 << 15,
    }

    internal sealed class CILMethodInfo
    {
        // Public
        /// <summary>
        /// The associated metadata method.
        /// </summary>
        public readonly MethodBase Method;
        /// <summary>
        /// The method flags which specify how the method should be used.
        /// </summary>
        public readonly CILMethodFlags Flags;
        /// <summary>
        /// The type information for the return type.
        /// </summary>
        public readonly CILTypeInfo ReturnType;
        /// <summary>
        /// The type information for the parameter types.
        /// </summary>
        public readonly CILTypeInfo[] ParameterTypes;        
        /// <summary>
        /// The optional delegate if an interop binding is associated with this method.
        /// </summary>
        public readonly Delegate InteropCall;
        /// <summary>
        /// The CIL bytecode instructions for this method if it is a CLR method (interpreted).
        /// </summary>
        public readonly byte[] Instructions;

        // Constructor
        internal CILMethodInfo(AssemblyLoadContext loadContext, MethodBase fromMethod)
        {

        }

        // Methods
        public override string ToString()
        {
            return $"{Method} = {Flags}";
        }
    }
}
