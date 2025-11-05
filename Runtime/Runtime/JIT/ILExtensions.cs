using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace dotnow.Runtime.JIT
{
    internal enum ILOperandType : ushort
    {
        InlineNone = 0,
        ShortInlineI,           // 1        
        ShortInlineBrTarget,    // 1
        ShortInlineVar,         // 1
        InlineVar,              // 2
        InlineI,                // 4
        InlineBrTarget,         // 4
        InlineField,            // 4
        InlineMethod,           // 4
        InlineSignature,        // 4
        InlineToken,            // 4
        InlineType,             // 4
        InlineString,           // 4
        ShortInlineR,           // 4
        InlineI8,               // 8
        InlineR,                // 8
    }

    internal static class ILOpCodeExtensions
    {
        // Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPrefix(this ILOpCode op)
        {
            switch (op)
            {
                case ILOpCode.Tail:
                case ILOpCode.Unaligned:
                case ILOpCode.Volatile:
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetOperandSize(this ILOpCode op)
        {
            // Get operand type first
            ILOperandType type = op.GetOperandType();

            // Select size by operand
            return type switch
            {
                ILOperandType.ShortInlineI => 1,
                ILOperandType.ShortInlineBrTarget => 1,
                ILOperandType.ShortInlineVar => 1,

                ILOperandType.InlineVar => 2,
                ILOperandType.InlineI => 4,
                ILOperandType.InlineBrTarget => 4,
                ILOperandType.InlineField => 4,
                ILOperandType.InlineMethod => 4,
                ILOperandType.InlineSignature => 4,
                ILOperandType.InlineToken => 4,
                ILOperandType.InlineType => 4,
                ILOperandType.InlineString => 4,
                ILOperandType.ShortInlineR => 4,
                ILOperandType.InlineI8 => 8,
                ILOperandType.InlineR => 8,
                _ => 0,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILOperandType GetOperandType(this ILOpCode op)
        {
            switch (op)
            {
                // ShortInlineI
                case ILOpCode.Ldc_i4_s: return ILOperandType.ShortInlineI;

                // Short br
                case ILOpCode.Br_s:
                case ILOpCode.Brtrue_s:
                case ILOpCode.Brfalse_s: return ILOperandType.ShortInlineBrTarget;

                // Br
                case ILOpCode.Br:
                case ILOpCode.Brtrue:
                case ILOpCode.Brfalse:
                case ILOpCode.Beq:
                case ILOpCode.Bne_un:
                case ILOpCode.Bge:
                case ILOpCode.Bge_un:
                case ILOpCode.Bgt:
                case ILOpCode.Bgt_un:
                case ILOpCode.Ble:
                case ILOpCode.Ble_un:
                case ILOpCode.Blt:
                case ILOpCode.Blt_un: return ILOperandType.InlineBrTarget;

                // 1 byte
                case ILOpCode.Ldloca_s:
                case ILOpCode.Ldloc_s:
                case ILOpCode.Stloc_s:
                case ILOpCode.Ldarga_s:
                case ILOpCode.Ldarg_s:
                case ILOpCode.Starg_s: return ILOperandType.ShortInlineVar;

                // Inline var
                case ILOpCode.Ldarg:
                case ILOpCode.Ldarga:
                case ILOpCode.Starg:
                case ILOpCode.Ldloc:
                case ILOpCode.Ldloca:
                case ILOpCode.Stloc: return ILOperandType.InlineVar;

                // Inline I
                case ILOpCode.Ldc_i4: return ILOperandType.InlineI;

                // Inline field
                case ILOpCode.Ldfld:
                case ILOpCode.Ldflda:
                case ILOpCode.Stfld:
                case ILOpCode.Ldsfld:
                case ILOpCode.Ldsflda:
                case ILOpCode.Stsfld: return ILOperandType.InlineField;

                case ILOpCode.Newobj:
                case ILOpCode.Call:
                case ILOpCode.Callvirt:
                case ILOpCode.Ldftn:
                case ILOpCode.Ldvirtftn:
                case ILOpCode.Jmp: return ILOperandType.InlineMethod;

                // Inline signature
                case ILOpCode.Calli:
                case ILOpCode.Localloc: return ILOperandType.InlineSignature;

                // Inline token
                case ILOpCode.Ldtoken: return ILOperandType.InlineToken;

                // Inline type
                case ILOpCode.Newarr:
                case ILOpCode.Box:
                case ILOpCode.Unbox:
                case ILOpCode.Unbox_any:
                case ILOpCode.Isinst:
                case ILOpCode.Castclass:
                case ILOpCode.Ldelema:
                case ILOpCode.Ldobj:
                case ILOpCode.Stobj:
                case ILOpCode.Initobj:
                case ILOpCode.Sizeof:
                case ILOpCode.Refanyval:
                case ILOpCode.Mkrefany: return ILOperandType.InlineType;

                // Inline string
                case ILOpCode.Ldstr: return ILOperandType.InlineString;

                // Short Inline R
                case ILOpCode.Ldc_r4: return ILOperandType.ShortInlineR;

                // Inline I8
                case ILOpCode.Ldc_i8: return ILOperandType.InlineI8;

                // Inline R
                case ILOpCode.Ldc_r8: return ILOperandType.InlineR;


                // Prefix
                case ILOpCode.Unaligned: return ILOperandType.ShortInlineVar;
            }
            return ILOperandType.InlineNone;
        }
    }
}
