using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace dotnow.Runtime.CIL
{
    [Flags]
    internal enum ExceptionHandlerKind
    {
        Clause = 0,
        Filter = 1,
        Finally = 2,
        Fault = 4
    }

    internal readonly struct CILExceptionHandlerInfo
    {
        // Public
        public readonly Type ExceptionType;
        public readonly ExceptionHandlerKind HandlerKind;
        public readonly int TryOffset;
        public readonly int TryLength;
        public readonly int HandlerOffset;
        public readonly int HandlerLength;

        // Constructor
        public CILExceptionHandlerInfo(ExceptionHandlingClause clause)
        {
            this.ExceptionType = clause.CatchType;
            this.HandlerKind = (ExceptionHandlerKind)clause.Flags;
            this.TryOffset = clause.TryOffset;
            this.TryLength = clause.TryLength;
            this.HandlerOffset = clause.HandlerOffset;
            this.HandlerLength = clause.HandlerLength;
        }

        // Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCaught(int pc)
        {
            return pc >= TryOffset && pc < TryOffset + TryLength;
        }
    }
}
