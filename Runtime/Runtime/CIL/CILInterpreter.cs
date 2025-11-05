using System;
using System.Reflection;
using Mono.Cecil.Cil;
using dotnow.Reflection;
using System.Collections;
using Codice.CM.Common;

namespace dotnow.Runtime.CIL
{
    internal static class CILInterpreter
    {
        // Methods
        internal static void ExecuteInterpreted(AppDomain domain, ExecutionEngine engine, ref ExecutionFrame frame, ref CILOperation[] instructions, ref CLRExceptionHandler[] exceptionHandlers, ExecutionEngine.DebugFlags debugFlags)
        {
            // Locals
            // Locals are predefined so that they can be shared between instructions to heavily reduce the locals required in compiled code (previous versions could require over 150 locals, most of which are of identical type)
            // Stack locals
            StackData[] stack = frame.stack;        // ldloc.0
            int stackPtr = frame.stackIndex;        // ldloc.1

            // Shared locals
            StackData left, right, temp;            // ldloc.2.3/ldloc.s (4)
            bool flag;
            CILFieldAccess fieldAccess;
            CILMethodInvocation methodInvoke;
            CILSignature signature;

            char[] charArrImpl;
            bool[] boolArrImpl;
            sbyte[] int8ArrImpl;
            byte[] uint8ArrImpl;
            short[] int16ArrImpl;
            ushort[] uint16ArrImpl;
            int[] int32ArrImpl;
            uint[] uint32ArrImpl;
            long[] int64ArrImpl;
            float[] singleArrImpl;
            double[] doubleArrImpl;
            object[] objArrImpl;
            IList listArrImpl;


            int instructionLength = instructions.Length;            

            // Use local variables in instruction loop           
            
            int instructionPtr = frame.instructionPtr;
            int stackArgOffset = frame.Method.IsStatic == false ? 1 : 0;

            // Main instruction loop
            while (instructionPtr < instructionLength && frame.abort == false)
            {
                // Get the instruction
                CILOperation instruction = instructions[instructionPtr];


                /// ### WARNING - Only enable this for small snippets of non-looping (Or very shallow looping) code otherwise the performance and memory allocations will be horrific and likley cause an editor crash
#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_PROFILE && UNITY_PROFILE_INSTRUCTIONS
                //UnityEngine.Profiling.Profiler.BeginSample(instruction.instructionName);
#endif
#endif

                // Switch (opCode)
                switch (instruction.opCode)
                {
                    default:
                        throw new NotImplementedException("MSIL instruction is not implemented: " + instruction.opCode.ToString() + "\nAt method body: " + frame.Method);

#region Arithmetic
                    case Code.Add:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    stack[stackPtr - 2].Int32 = unchecked(left.Int32 + right.Int32);
                                    stack[stackPtr - 2].Type = right.Type;
                                    break;

                                case StackType.UInt32:
                                    stack[stackPtr - 2].Int32 = (int)unchecked((uint)left.Int32 + (uint)right.Int32);
                                    break;

                                case StackType.Int64:
                                    stack[stackPtr - 2].Int64 = left.Int64 + right.Int64;// unchecked(left.value.Int64 + right.value.Int64);
                                    break;

                                case StackType.UInt64:
                                    stack[stackPtr - 2].Int64 = (long)unchecked((ulong)left.Int64 + (ulong)right.Int64);
                                    stack[stackPtr - 2].Type = right.Type;
                                    break;

                                case StackType.Single:
                                    stack[stackPtr - 2].Single = unchecked(left.Single + right.Single);
                                    break;

                                case StackType.Double:
                                    stack[stackPtr - 2].Double = unchecked(left.Double + right.Double);
                                    break;
                            }
                            stackPtr--;
                            break;
                        }

                    case Code.Add_Ovf:
                    case Code.Add_Ovf_Un:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    stack[stackPtr - 2].Int32 = checked(left.Int32 + right.Int32);
                                    break;

                                case StackType.UInt32:
                                    stack[stackPtr - 2].Int32 = (int)checked((uint)left.Int32 + (uint)right.Int32);
                                    break;

                                case StackType.Int64:
                                    stack[stackPtr - 2].Int64 = checked(left.Int64 + right.Int64);
                                    break;

                                case StackType.UInt64:
                                    stack[stackPtr - 2].Int64 = (long)checked((ulong)left.Int64 + (ulong)right.Int64);
                                    break;

                                case StackType.Single:
                                    stack[stackPtr - 2].Single = checked(left.Single + right.Single);
                                    break;

                                case StackType.Double:
                                    stack[stackPtr - 2].Double = checked(left.Double + right.Double);
                                    break;
                            }

                            stackPtr--;
                            break;
                        }

                    case Code.Sub:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    stack[stackPtr - 2].Int32 = unchecked(left.Int32 - right.Int32);
                                    break;

                                case StackType.UInt32:
                                    stack[stackPtr - 2].Int32 = (int)unchecked((uint)left.Int32 - (uint)right.Int32);
                                    break;

                                case StackType.Int64:
                                    stack[stackPtr - 2].Int64 = unchecked(left.Int64 - right.Int64);
                                    break;

                                case StackType.UInt64:
                                    stack[stackPtr - 2].Int64 = (long)unchecked((ulong)left.Int64 - (ulong)right.Int64);
                                    break;

                                case StackType.Single:
                                    stack[stackPtr - 2].Single = unchecked(left.Single - right.Single);
                                    break;

                                case StackType.Double:
                                    stack[stackPtr - 2].Double = unchecked(left.Double - right.Double);
                                    break;
                            }

                            stackPtr--;
                            break;
                        }

                    case Code.Sub_Ovf:
                    case Code.Sub_Ovf_Un:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    stack[stackPtr - 2].Int32 = checked(left.Int32 - right.Int32);
                                    break;

                                case StackType.UInt32:
                                    stack[stackPtr - 2].Int32 = (int)checked((uint)left.Int32 - (uint)right.Int32);
                                    break;

                                case StackType.Int64:
                                    stack[stackPtr - 2].Int64 = checked(left.Int64 - right.Int64);
                                    break;

                                case StackType.UInt64:
                                    stack[stackPtr - 2].Int64 = (long)checked((ulong)left.Int64 - (ulong)right.Int64);
                                    break;

                                case StackType.Single:
                                    stack[stackPtr - 2].Single = checked(left.Single - right.Single);
                                    break;

                                case StackType.Double:
                                    stack[stackPtr - 2].Double = checked(left.Double - right.Double);
                                    break;
                            }

                            stackPtr--;
                            break;
                        }

                    case Code.Mul:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    stack[stackPtr - 2].Int32 = unchecked(left.Int32 * right.Int32);
                                    break;

                                case StackType.UInt32:
                                    stack[stackPtr - 2].Int32 = (int)unchecked((uint)left.Int32 * (uint)right.Int32);
                                    break;

                                case StackType.Int64:
                                    stack[stackPtr - 2].Int64 = unchecked(left.Int64 * right.Int64);
                                    break;

                                case StackType.UInt64:
                                    stack[stackPtr - 2].Int64 = (long)unchecked((ulong)left.Int64 * (ulong)right.Int64);
                                    break;

                                case StackType.Single:
                                    stack[stackPtr - 2].Single = unchecked(left.Single * right.Single);
                                    break;

                                case StackType.Double:
                                    stack[stackPtr - 2].Double = unchecked(left.Double * right.Double);
                                    break;
                            }

                            stackPtr--;
                            break;
                        }

                    case Code.Mul_Ovf:
                    case Code.Mul_Ovf_Un:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    stack[stackPtr - 2].Int32 = checked(left.Int32 * right.Int32);
                                    break;

                                case StackType.UInt32:
                                    stack[stackPtr - 2].Int32 = (int)checked((uint)left.Int32 * (uint)right.Int32);
                                    break;

                                case StackType.Int64:
                                    stack[stackPtr - 2].Int64 = checked(left.Int64 * right.Int64);
                                    break;

                                case StackType.UInt64:
                                    stack[stackPtr - 2].Int64 = (long)checked((ulong)left.Int64 * (ulong)right.Int64);
                                    break;

                                case StackType.Single:
                                    stack[stackPtr - 2].Single = checked(left.Single * right.Single);
                                    break;

                                case StackType.Double:
                                    stack[stackPtr - 2].Double = checked(left.Double * right.Double);
                                    break;
                            }

                            stackPtr--;
                            break;
                        }

                    case Code.Div:
                    case Code.Div_Un:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    stack[stackPtr - 2].Int32 = unchecked(left.Int32 / right.Int32);
                                    break;

                                case StackType.UInt32:
                                    stack[stackPtr - 2].Int32 = (int)unchecked((uint)left.Int32 / (uint)right.Int32);
                                    break;

                                case StackType.Int64:
                                    stack[stackPtr - 2].Int64 = unchecked(left.Int64 / right.Int64);
                                    break;

                                case StackType.UInt64:
                                    stack[stackPtr - 2].Int64 = (long)unchecked((ulong)left.Int64 / (ulong)right.Int64);
                                    break;

                                case StackType.Single:
                                    stack[stackPtr - 2].Single = unchecked(left.Single / right.Single);
                                    break;

                                case StackType.Double:
                                    stack[stackPtr - 2].Double = unchecked(left.Double / right.Double);
                                    break;
                            }

                            stackPtr--;
                            break;
                        }

                    case Code.Neg:
                        {
                            StackData val = stack[stackPtr - 1];

                            switch (val.Type)
                            {
                                case StackType.Int32:
                                    stack[stackPtr - 1].Int32 = -stack[stackPtr - 1].Int32;
                                    break;
                                case StackType.Int64:
                                    stack[stackPtr - 1].Int64 = -stack[stackPtr - 1].Int64;
                                    break;
                                case StackType.Single:
                                    stack[stackPtr - 1].Single = -stack[stackPtr - 1].Single;
                                    break;
                                case StackType.Double:
                                    stack[stackPtr - 1].Double = -stack[stackPtr - 1].Double;
                                    break;

                                default:
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Rem:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    {
                                        stack[stackPtr++].Int32 = (left.Int32 % right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                    {
                                        stack[stackPtr++].Int64 = (left.Int64 % right.Int64);
                                        break;
                                    }
                                case StackType.Single:
                                    {
                                        stack[stackPtr++].Single = (left.Single % right.Single);
                                        break;
                                    }
                                case StackType.Double:
                                    {
                                        stack[stackPtr++].Double = (left.Double % right.Double);
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Rem_Un:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.UInt32:
                                case StackType.Int32:
                                    {
                                        stack[stackPtr++].Int32 = (int)((uint)left.Int32 % (uint)right.Int32);
                                        break;
                                    }

                                case StackType.UInt64:
                                case StackType.Int64:
                                    {
                                        stack[stackPtr++].Int64 = (long)((ulong)left.Int64 % (ulong)right.Int64);
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Ckfinite:
                        {
                            temp = stack[stackPtr - 1];

                            // Check for finite
                            if (float.IsNaN(temp.Single) == true || float.IsInfinity(temp.Single) == true)
                                throw new ArithmeticException("Not a finite number");
                            break;
                        }

                    case Code.Shl:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    {
                                        stack[stackPtr].Int32 = left.Int32 << right.Int32;
                                        break;
                                    }

                                case StackType.UInt32:
                                    {
                                        stack[stackPtr].Int32 = (int)((uint)left.Int32 << right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                    {
                                        stack[stackPtr].Int64 = left.Int64 << right.Int32;
                                        break;
                                    }

                                case StackType.UInt64:
                                    {
                                        stack[stackPtr].Int64 = (long)((ulong)left.Int64 << right.Int32);
                                        break;
                                    }

                                default: throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Shr:
                    case Code.Shr_Un:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                    {
                                        stack[stackPtr].Int32 = left.Int32 >> right.Int32;
                                        break;
                                    }

                                case StackType.UInt32:
                                    {
                                        stack[stackPtr].Int32 = (int)((uint)left.Int32 >> right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                    {
                                        stack[stackPtr].Int64 = left.Int64 >> right.Int32;
                                        break;
                                    }

                                case StackType.UInt64:
                                    {
                                        stack[stackPtr].Int64 = (long)((ulong)left.Int64 >> right.Int32);
                                        break;
                                    }

                                default: throw new NotSupportedException();
                            }
                            break;
                        }
#endregion

#region Compare
                    case Code.Ceq:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        stack[stackPtr].Int32 = (left.Int32 == right.Int32) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        stack[stackPtr].Int32 = (left.Int64 == right.Int64) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Single:
                                    {
                                        stack[stackPtr].Int32 = (left.Single == right.Single) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        stack[stackPtr].Int32 = (left.Double == right.Double) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Ref:
                                case StackType.RefBoxed:
                                    {
                                        if (left.Ref == null)
                                        {
                                            stack[stackPtr].Int32 = (right.Ref == null) ? 1 : 0;
                                            stack[stackPtr++].Type = StackType.Int32;
                                            break;
                                        }

                                        if (left.Type == StackType.RefBoxed)
                                        {
                                            stack[stackPtr].Int32 = (left.Ref.Equals(right.Ref) == true) ? 1 : 0;
                                            stack[stackPtr++].Type = StackType.Int32;
                                        }
                                        else
                                        {
                                            stack[stackPtr].Int32 = (left.Ref == right.Ref) ? 1 : 0;
                                            stack[stackPtr++].Type = StackType.Int32;
                                        }
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Clt:
                    case Code.Clt_Un:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        stack[stackPtr].Int32 = (left.Int32 < right.Int32) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        stack[stackPtr].Int32 = (left.Int64 < right.Int64) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Single:
                                    {
                                        stack[stackPtr].Int32 = (left.Single < right.Single) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        stack[stackPtr].Int32 = (left.Double < right.Double) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        stack[stackPtr].Int32 = (left.Address < right.Address) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                default: throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Cgt:
                    case Code.Cgt_Un:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        stack[stackPtr].Int32 = (left.Int32 > right.Int32) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;    
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        stack[stackPtr].Int32 = (left.Int64 > right.Int64) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Single:
                                    {
                                        stack[stackPtr].Int32 = (left.Single > right.Single) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        stack[stackPtr].Int32 = (left.Double > right.Double) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        stack[stackPtr].Int32 = (left.Address > right.Address) ? 1 : 0;
                                        stack[stackPtr++].Type = StackType.Int32;
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }
                            break;
                        }


#endregion

#region Convert
                    case Code.Box:
                        {
                            stack[stackPtr - 1].Ref = stack[stackPtr - 1].BoxAsType(instruction.typeOperand);
                            stack[stackPtr - 1].Type = StackType.RefBoxed;
                            break;
                        }

                    case Code.Unbox_Any:
                        {
                            stack[stackPtr - 1].UnboxAsType(ref stack[stackPtr - 1], instruction.typeOperand);
                            break;
                        }

                    case Code.Castclass:
                        {
                            CLRTypeInfo castType = instruction.typeOperand;

                            // Pop object
                            temp = stack[--stackPtr];

                            // Get ref
                            object inst = temp.Box();

                            // Chekc for null
                            if(inst != null)
                            {
                                // Get interpreted type
                                Type instType = inst.GetInterpretedType();

                                // Check for equal or assignable (May need more work to support interfaces??)
                                if(castType.type == instType || instType.IsSubclassOf(castType.type) == true)
                                {
                                    // Push object
                                    stack[stackPtr++] = temp;
                                }
                                else if(castType.type.IsSubclassOf(typeof(MulticastDelegate)) == true)
                                {
                                    // Do nothing - cast is implicit
                                }
                                else
                                {
                                    // Invalid cast
                                    throw new InvalidCastException();
                                }
                            }
                            else
                            {
                                // Null reference can be pushed pack onto top of stack - should not fail
                                stack[stackPtr++] = temp;
                            }
                            break;
                        }

                    case Code.Conv_I:
                        {
                            RuntimeConvert.ToInt32(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_I1:
                        {
                            RuntimeConvert.ToInt8(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_I2:
                        {
                            RuntimeConvert.ToInt16(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_I4:
                        {
                            RuntimeConvert.ToInt32(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_I8:
                        {
                            RuntimeConvert.ToInt64(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_U:
                        {
                            RuntimeConvert.ToUInt32(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_U1:
                        {
                            RuntimeConvert.ToUInt8(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_U2:
                        {
                            RuntimeConvert.ToUInt16(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_U4:
                        {
                            RuntimeConvert.ToUInt32(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_U8:
                        {
                            RuntimeConvert.ToUInt64(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_R_Un:
                        {
                            RuntimeConvert.ToSingle(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_R4:
                        {
                            RuntimeConvert.ToSingle(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_R8:
                        {
                            RuntimeConvert.ToDouble(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I:
                        {
                            RuntimeConvert.ToInt32Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I_Un:
                        {
                            RuntimeConvert.ToInt32Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I1:
                        {
                            RuntimeConvert.ToInt8Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I1_Un:
                        {
                            RuntimeConvert.ToInt8Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I2:
                        {
                            RuntimeConvert.ToInt16Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I2_Un:
                        {
                            RuntimeConvert.ToInt16Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I4:
                        {
                            RuntimeConvert.ToInt32Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I4_Un:
                        {
                            RuntimeConvert.ToInt32Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I8:
                        {
                            RuntimeConvert.ToInt64Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_I8_Un:
                        {
                            RuntimeConvert.ToInt64Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U:
                        {
                            RuntimeConvert.ToUInt32Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U_Un:
                        {
                            RuntimeConvert.ToUInt32Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U1:
                        {
                            RuntimeConvert.ToUInt8Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U1_Un:
                        {
                            RuntimeConvert.ToUInt8Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U2:
                        {
                            RuntimeConvert.ToUInt16Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U2_Un:
                        {
                            RuntimeConvert.ToUInt16Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U4:
                        {
                            RuntimeConvert.ToUInt32Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U4_Un:
                        {
                            RuntimeConvert.ToUInt32Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U8:
                        {
                            RuntimeConvert.ToUInt64Checked(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Conv_Ovf_U8_Un:
                        {
                            RuntimeConvert.ToUInt64Checked(ref stack[stackPtr - 1]);
                            break;
                        }
#endregion

#region Branch
                    case Code.Br:
                    case Code.Br_S:
                        {
                            instructionPtr += instruction.operand.Int32;
                            continue;
                        }

                    case Code.Brtrue:
                    case Code.Brtrue_S:
                        {
                            switch (stack[stackPtr - 1].Type)
                            {
                                case StackType.Ref:
                                case StackType.RefBoxed:
                                case StackType.ByRef:
                                    {
                                        if (stack[--stackPtr].Address != 0)
                                        {
                                            instructionPtr += instruction.operand.Int32;
                                            continue;
                                        }
                                        break;
                                    }

                                default:
                                    {
                                        if (stack[--stackPtr].Int32 != 0)
                                        {
                                            instructionPtr += instruction.operand.Int32;
                                            continue;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }

                    case Code.Brfalse:
                    case Code.Brfalse_S:
                        {
                            switch(stack[stackPtr - 1].Type)
                            {
                                case StackType.Ref:
                                case StackType.RefBoxed:
                                case StackType.ByRef:
                                    {
                                        if(stack[--stackPtr].Address == 0)
                                        {
                                            instructionPtr += instruction.operand.Int32;
                                            continue;
                                        }
                                        break;
                                    }

                                default:
                                    {
                                        if (stack[--stackPtr].Int32 == 0)
                                        {
                                            instructionPtr += instruction.operand.Int32;
                                            continue;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }

                    case Code.Beq:
                    case Code.Beq_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];                            

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 == right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 == right.Int64);
                                        break;
                                    }

                                case StackType.Single:
                                    {
                                        flag = (left.Single == right.Single);
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        flag = (left.Double == right.Double);
                                        break;
                                    }

                                case StackType.Ref:
                                case StackType.RefBoxed:
                                    {
                                        flag = (left.Ref == right.Ref);
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }

                    case Code.Bne_Un:
                    case Code.Bne_Un_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 != right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 != right.Int64);
                                        break;
                                    }

                                case StackType.Ref:
                                case StackType.RefBoxed:
                                    {            
                                        flag = (left.Ref.Equals(right.Ref) == false);
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }

                    case Code.Blt:
                    case Code.Blt_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 < right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 < right.Int64);
                                        break;
                                    }

                                case StackType.Single:
                                    {
                                        flag = (left.Single < right.Single);
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        flag = (left.Double < right.Double);
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        flag = (left.Address < right.Int32);
                                        break;
                                    }

                                default: throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }
                    case Code.Blt_Un:
                    case Code.Blt_Un_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = ((uint)left.Int32 < (uint)right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = ((uint)left.Int64 < (uint)right.Int64);
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        flag = ((uint)left.Address < (uint)right.Int32);
                                        break;
                                    }

                                default: throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }

                    case Code.Ble:
                    case Code.Ble_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 <= right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 <= right.Int64);
                                        break;
                                    }

                                case StackType.Single:
                                    {
                                        flag = (left.Single <= right.Single);
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        flag = (left.Double <= right.Double);
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        flag = (left.Address <= right.Int32);
                                        break;
                                    }

                                default: throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }
                    case Code.Ble_Un:
                    case Code.Ble_Un_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 <= right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 <= right.Int64);
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        flag = (left.Address <= right.Int32);
                                        break;
                                    }

                                default: throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }

                    case Code.Bgt:
                    case Code.Bgt_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 > right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 > right.Int64);
                                        break;
                                    }

                                case StackType.Single:
                                    {
                                        flag = (left.Single > right.Single);
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        flag = (left.Double > right.Double);
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        flag = (left.Address > right.Address);
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }
                    case Code.Bgt_Un:
                    case Code.Bgt_Un_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 > right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 > right.Int64);
                                        break;
                                    }
                                case StackType.Single:
                                    {
                                        flag = (left.Single > right.Single);
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        flag = (left.Double > right.Double);
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        flag = (left.Address > right.Int32);
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }

                    case Code.Bge:
                    case Code.Bge_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 >= right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 >= right.Int64);
                                        break;
                                    }

                                case StackType.Single:
                                    {
                                        flag = (left.Single >= right.Single);
                                        break;
                                    }

                                case StackType.Double:
                                    {
                                        flag = (left.Double >= right.Double);
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        flag = (left.Address >= right.Int32);
                                        break;
                                    }

                                default: throw new NotSupportedException();
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }
                    case Code.Bge_Un:
                    case Code.Bge_Un_S:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        flag = (left.Int32 >= right.Int32);
                                        break;
                                    }

                                case StackType.Int64:
                                case StackType.UInt64:
                                    {
                                        flag = (left.Int64 >= right.Int64);
                                        break;
                                    }
                                case StackType.Single:
                                    {
                                        flag = (left.Single >= right.Single);
                                        break;
                                    }
                                case StackType.Double:
                                    {
                                        flag = ((left.Double >= right.Double));
                                        break;
                                    }

                                case StackType.Ref:
                                    {
                                        flag = (left.Address >= right.Address);
                                        break;
                                    }

                                default: throw new NotSupportedException(left.Type.ToString());
                            }

                            if (flag == true)
                            {
                                instructionPtr += instruction.operand.Int32;
                                continue;
                            }
                            break;
                        }

                    case Code.Switch:
                        {
                            int index = stack[--stackPtr].Int32;
                            int[] offsets = (int[])instruction.objectOperand;

                            if (index >= 0 && index < offsets.Length)
                            {
                                instructionPtr += offsets[index];
                                continue;
                            }
                            break;
                        }
#endregion

#region Constant
                    case Code.Ldnull:
                        {
                            stack[stackPtr].Ref = null;
                            stack[stackPtr++].Type = StackType.Ref;
                            break;
                        }

                    case Code.Ldstr:
                        {
                            StackData.AllocRef(ref stack[stackPtr++], (string)instruction.objectOperand);
                            break;
                        }

                    case Code.Ldc_I4:
                    case Code.Ldc_I4_S:
                        {
                            stack[stackPtr].Int32 = instruction.operand.Int32;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_0:
                        {
                            stack[stackPtr].Int32 = 0;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_1:
                        {
                            stack[stackPtr].Int32 = 1;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_2:
                        {
                            stack[stackPtr].Int32 = 2;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_3:
                        {
                            stack[stackPtr].Int32 = 3;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_4:
                        {
                            stack[stackPtr].Int32 = 4;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_5:
                        {
                            stack[stackPtr].Int32 = 5;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_6:
                        {
                            stack[stackPtr].Int32 = 6;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_7:
                        {
                            stack[stackPtr].Int32 = 7;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_8:
                        {
                            stack[stackPtr].Int32 = 8;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I4_M1:
                        {
                            stack[stackPtr].Int32 = -1;
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldc_I8:
                        {
                            stack[stackPtr].Int64 = instruction.operand.Int64;
                            stack[stackPtr++].Type = StackType.Int64;
                            break;
                        }

                    case Code.Ldc_R4:
                        {
                            stack[stackPtr].Single = instruction.operand.Single;
                            stack[stackPtr++].Type = StackType.Single;
                            break;
                        }

                    case Code.Ldc_R8:
                        {
                            stack[stackPtr].Double = instruction.operand.Double;
                            stack[stackPtr++].Type = StackType.Double;
                            break;
                        }
#endregion

#region Argument
                    case Code.Ldarg_0:
                        {
                            stack[stackPtr++] = stack[frame.stackArgIndex];
                            break;
                        }

                    case Code.Ldarg_1:
                        {
                            stack[stackPtr++] = stack[frame.stackArgIndex + 1];
                            break;
                        }

                    case Code.Ldarg_2:
                        {
                            stack[stackPtr++] = stack[frame.stackArgIndex + 2];
                            break;
                        }

                    case Code.Ldarg_3:
                        {
                            stack[stackPtr++] = stack[frame.stackArgIndex + 3];
                            break;
                        }

                    case Code.Ldarg:
                    case Code.Ldarg_S:
                        {
                            stack[stackPtr++] = stack[frame.stackArgIndex + instruction.operand.Int32 + stackArgOffset];
                            break;
                        }

                    case Code.Ldarga:
                    case Code.Ldarga_S:
                        {
                            __internal.__gc_alloc_addr_stack(ref stack[stackPtr++], stack, frame.stackArgIndex + instruction.operand.Int32 + stackArgOffset);

                            //stack[stackPtr].refValue = new ByRefVariable(stack, frame.stackArgIndex + instruction.operand.Int32 + stackArgOffset);
                            //stack[stackPtr++].type = StackData.ObjectType.ByRef;
                            break;
                        }

                    case Code.Starg:
                    case Code.Starg_S:
                        {
                            stack[frame.stackArgIndex + instruction.operand.Int32 + stackArgOffset] = stack[--stackPtr];
                            break;
                        }
#endregion

#region Local
                    case Code.Ldloc_0:
                        {
                            stack[stackPtr++] = stack[frame.stackMin + 0];
                            break;
                        }

                    case Code.Ldloc_1:
                        {
                            stack[stackPtr++] = stack[frame.stackMin + 1];
                            break;
                        }

                    case Code.Ldloc_2:
                        {
                            stack[stackPtr++] = stack[frame.stackMin + 2];
                            break;
                        }

                    case Code.Ldloc_3:
                        {
                            stack[stackPtr++] = stack[frame.stackMin + 3];
                            break;
                        }

                    case Code.Ldloc:
                    case Code.Ldloc_S:
                        {
                            stack[stackPtr++] = stack[frame.stackMin + instruction.operand.Int32];
                            break;
                        }

                    case Code.Ldloca:
                    case Code.Ldloca_S:
                        {
                            __internal.__gc_alloc_addr_stack(ref stack[stackPtr++], stack, frame.stackMin + instruction.operand.Int32);

                            //stack[stackPtr].refValue = new ByRefVariable(stack, instruction.operand.Int32);
                            //stack[stackPtr++].type = StackData.ObjectType.ByRef;
                            break;
                        }

                    case Code.Stloc_0:
                        {
                            // Copy value type
                            StackData.ValueTypeCopy(ref stack[stackPtr - 1]);

                            // Copy the value but don't overwrite dest type - This is important for unsigned primitives because the CLR loads them as non-unsigned in some cases
                            StackData.AssignKeepType(ref stack[frame.stackMin + 0], stack[--stackPtr]);
                            break;
                        }

                    case Code.Stloc_1:
                        {
                            // Copy value type
                            StackData.ValueTypeCopy(ref stack[stackPtr - 1]);

                            // Copy the value but don't overwrite dest type - This is important for unsigned primitives because the CLR loads them as non-unsigned in some cases
                            StackData.AssignKeepType(ref stack[frame.stackMin + 1], stack[--stackPtr]);
                            break;
                        }

                    case Code.Stloc_2:
                        {
                            // Copy value type
                            StackData.ValueTypeCopy(ref stack[stackPtr - 1]);

                            // Copy the value but don't overwrite dest type - This is important for unsigned primitives because the CLR loads them as non-unsigned in some cases
                            StackData.AssignKeepType(ref stack[frame.stackMin + 2], stack[--stackPtr]);
                            break;
                        }

                    case Code.Stloc_3:
                        {
                            // Copy value type
                            StackData.ValueTypeCopy(ref stack[stackPtr - 1]);

                            // Copy the value but don't overwrite dest type - This is important for unsigned primitives because the CLR loads them as non-unsigned in some cases
                            StackData.AssignKeepType(ref stack[frame.stackMin + 3], stack[--stackPtr]);
                            break;
                        }

                    case Code.Stloc:
                    case Code.Stloc_S:
                        {
                            // Copy value type
                            StackData.ValueTypeCopy(ref stack[stackPtr - 1]);

                            // Copy the value but don't overwrite dest type - This is important for unsigned primitives because the CLR loads them as non-unsigned in some cases
                            StackData.AssignKeepType(ref stack[frame.stackMin + instruction.operand.Int32], stack[--stackPtr]);
                            break;
                        }
#endregion

#region Indirect
                    case Code.Ldind_I:
                        {
                            stack[stackPtr - 1].Int32 = ((IByRef)stack[--stackPtr].Ref).GetReferenceValueI4();
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldind_I1:
                        {
                            stack[stackPtr - 1].Int32 = ((IByRef)stack[--stackPtr].Ref).GetReferenceValueI1();
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldind_I2:
                        {
                            stack[stackPtr - 1].Int32 = ((IByRef)stack[--stackPtr].Ref).GetReferenceValueI2();
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldind_I4:
                        {
                            stack[stackPtr - 1].Int32 = ((IByRef)stack[--stackPtr].Ref).GetReferenceValueI4();
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldind_I8:
                        {
                            stack[stackPtr - 1].Int64 = ((IByRef)stack[--stackPtr].Ref).GetReferenceValueI8();
                            stack[stackPtr++].Type = StackType.Int64;
                            break;
                        }

                    case Code.Ldind_R4:
                        {
                            stack[stackPtr - 1].Single = ((IByRef)stack[--stackPtr].Ref).GetReferenceValueR4();
                            stack[stackPtr++].Type = StackType.Single;
                            break;
                        }

                    case Code.Ldind_R8:
                        {
                            stack[stackPtr - 1].Double = ((IByRef)stack[--stackPtr].Ref).GetReferenceValueR8();
                            stack[stackPtr++].Type = StackType.Double;
                            break;
                        }

                    case Code.Ldind_U1:
                        {
                            stack[stackPtr - 1].Int32 = (sbyte)((IByRef)stack[--stackPtr].Ref).GetReferenceValueU1();
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldind_U2:
                        {
                            stack[stackPtr - 1].Int32 = (short)((IByRef)stack[--stackPtr].Ref).GetReferenceValueU2();
                            stack[stackPtr++].Type = StackType.Int32;
                            break;
                        }

                    case Code.Ldind_U4:
                        {
                            stack[stackPtr - 1].Int32 = (int)((IByRef)stack[--stackPtr].Ref).GetReferenceValueU4();
                            stack[stackPtr++].Type = StackType.UInt32;
                            break;
                        }

                    case Code.Ldind_Ref:
                        {
                            stack[stackPtr - 1].Ref = ((IByRef)stack[--stackPtr].Ref).GetReferenceValue();
                            stack[stackPtr++].Type = StackType.Ref;
                            break;
                        }

                    case Code.Stind_I:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            ((IByRef)left.Ref).SetReferenceValueI4((int)(IntPtr)right.Int32);
                            break;
                        }

                    case Code.Stind_I1:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            ((IByRef)left.Ref).SetReferenceValueI1((sbyte)right.Int32);
                            break;
                        }

                    case Code.Stind_I2:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            ((IByRef)left.Ref).SetReferenceValueI2((short)right.Int32);
                            break;
                        }

                    case Code.Stind_I4:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            ((IByRef)left.Ref).SetReferenceValueI4(right.Int32);
                            break;
                        }

                    case Code.Stind_I8:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            ((IByRef)left.Ref).SetReferenceValueI8(right.Int64);
                            break;
                        }

                    case Code.Stind_R4:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            ((IByRef)left.Ref).SetReferenceValueR4(right.Single);
                            break;
                        }

                    case Code.Stind_R8:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            ((IByRef)left.Ref).SetReferenceValueR8(right.Double);
                            break;
                        }

                    case Code.Stind_Ref:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            ((IByRef)left.Ref).SetReferenceValue(right);
                            break;
                        }
#endregion

#region Array
                    case Code.Newarr:
                        {
                            // Length
                            temp = stack[--stackPtr];

                            if ((int)temp.Type <= 32)
                            {
                                // Allocate array short size
                                __internal.__gc_alloc_arrays(ref stack[stackPtr++], instruction.typeOperand.type, temp.Int32);
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                // Allocate array long size
                                __internal.__gc_alloc_arrayl(ref stack[stackPtr++], instruction.typeOperand.type, temp.Int64);
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_I:
                        {
                            temp = stack[--stackPtr];       // index
                            left = stack[--stackPtr];       // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            int32ArrImpl = (int[])left.Ref;    // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Int32 = int32ArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.Int32;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Int32 = int32ArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.Int32;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_I1:
                        {
                            temp = stack[--stackPtr];                   // index
                            left = stack[--stackPtr];                   // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            int8ArrImpl = (sbyte[])left.Ref;       // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Int32 = int8ArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.Int32;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Int32 = int8ArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.Int32;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_I2:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            // Handle char case
                            if (left.Ref is char[])
                            {
                                charArrImpl = (char[])left.Ref; // arr impl

                                if ((int)temp.Type <= 32)
                                {
                                    stack[stackPtr].Int32 = (short)charArrImpl[temp.Int32];
                                    stack[stackPtr++].Type = StackType.Int32;
                                }
                                else if ((int)temp.Type <= 64)
                                {
                                    stack[stackPtr].Int32 = (short)charArrImpl[temp.Int64];
                                    stack[stackPtr++].Type = StackType.Int32;
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            // Fallback to standard i16
                            else
                            {
                                int16ArrImpl = (short[])left.Ref;    // arr impl

                                if ((int)temp.Type <= 32)
                                {
                                    stack[stackPtr].Int32 = int16ArrImpl[temp.Int32];
                                    stack[stackPtr++].Type = StackType.Int32;
                                }
                                else if ((int)temp.Type <= 64)
                                {
                                    stack[stackPtr].Int32 = int16ArrImpl[temp.Int64];
                                    stack[stackPtr++].Type = StackType.Int32;
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Ldelem_I4:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            int32ArrImpl = (int[])left.Ref;        // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Int32 = int32ArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.Int32;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Int32 = int32ArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.Int32;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_I8:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            int64ArrImpl = (long[])left.Ref;   // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Int64 = int64ArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.Int64;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Int64 = int64ArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.Int64;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_R4:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            singleArrImpl = (float[])left.Ref;     // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Single = singleArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.Single;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Single = singleArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.Single;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_R8:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            doubleArrImpl = (double[])left.Ref;    // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Double = doubleArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.Double;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Double = doubleArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.Double;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_U1:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            // Check for bool array as I1
                            if (left.Ref is bool[])
                            {
                                boolArrImpl = (bool[])left.Ref;    // arr impl

                                if ((int)temp.Type <= 32)
                                {
                                    stack[stackPtr].Int32 = (sbyte)(boolArrImpl[temp.Int32] == true ? 1 : 0);
                                    stack[stackPtr++].Type = StackType.Int32;
                                }
                                else if ((int)temp.Type <= 64)
                                {
                                    stack[stackPtr].Int32 = (sbyte)(boolArrImpl[temp.Int64] == true ? 1 : 0);
                                    stack[stackPtr++].Type = StackType.Int32;
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            // Use I1 abyte or byte array
                            else
                            {
                                uint8ArrImpl = (byte[])left.Ref;   // arr impl

                                if ((int)temp.Type <= 32)
                                {
                                    stack[stackPtr].Int32 = (sbyte)uint8ArrImpl[temp.Int32];
                                    stack[stackPtr++].Type = StackType.Int32;
                                }
                                else if ((int)temp.Type <= 64)
                                {
                                    stack[stackPtr].Int32 = (sbyte)uint8ArrImpl[temp.Int64];
                                    stack[stackPtr++].Type = StackType.Int32;
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Ldelem_U2:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            uint16ArrImpl = (ushort[])left.Ref;    // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Int32 = (short)uint16ArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.Int32;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Int32 = (short)uint16ArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.Int32;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_U4:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            // Get exact array type
                            uint32ArrImpl = (uint[])left.Ref;  // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Int32 = (int)uint32ArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.UInt32;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Int32 = (int)uint32ArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.UInt32;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_Any:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            if ((int)temp.Type <= 32)
                            {
                                // Use IList for 32 bit indexing to support value types (Not possible to cast value type array to object[])
                                listArrImpl = (IList)left.Ref;
                                StackData.AllocTyped(ref stack[stackPtr++], instruction.typeOperand, listArrImpl[temp.Int32]);
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                // Fallback to slow reflection access in order to support 64 bit indexing
                                Array arr = (Array)left.Ref;
                                StackData.AllocTyped(ref stack[stackPtr++], instruction.typeOperand, arr.GetValue(temp.Int64));
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelem_Ref:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            objArrImpl = (object[])left.Ref;   // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                stack[stackPtr].Ref = objArrImpl[temp.Int32];
                                stack[stackPtr++].Type = StackType.Ref;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                stack[stackPtr].Ref = objArrImpl[temp.Int64];
                                stack[stackPtr++].Type = StackType.Ref;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldelema:
                        {
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            if ((int)temp.Type <= 32)
                            {
                                // Get array element address
                                __internal.__gc_alloc_addr_elem(ref stack[stackPtr++], (Array)left.Ref, temp.Int32);

                                //stack[stackPtr].refValue = new ByRefElement((Array)left.refValue, temp.value.Int32);
                                //stack[stackPtr++].type = StackData.ObjectType.ByRef;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                // Get array element address
                                __internal.__gc_alloc_addr_elem(ref stack[stackPtr++], (Array)left.Ref, temp.Int64);

                                //stack[stackPtr].refValue = new ByRefElement((Array)left.refValue, temp.value.Int64);
                                //stack[stackPtr++].type = StackData.ObjectType.ByRef;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Stelem_I:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            int32ArrImpl = (int[])left.Ref;    // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                int32ArrImpl[temp.Int32] = right.Int32;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                int32ArrImpl[temp.Int64] = right.Int32;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Stelem_I1:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            // Check for bool array as I1
                            if (left.Ref is bool[])
                            {
                                boolArrImpl = (bool[])left.Ref;    // arr impl

                                if ((int)temp.Type <= 32)
                                {
                                    boolArrImpl[temp.Int32] = ((byte)right.Int32) == 1 ? true : false;
                                }
                                else if ((int)temp.Type <= 64)
                                {
                                    boolArrImpl[temp.Int64] = ((byte)right.Int32) == 1 ? true : false;
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            // Use I1 sbyte or byte array
                            else
                            {
                                int8ArrImpl = (sbyte[])left.Ref;   // arr impl

                                if ((int)temp.Type <= 32)
                                {
                                    int8ArrImpl[temp.Int32] = (sbyte)right.Int32;
                                }
                                else if ((int)temp.Type <= 64)
                                {
                                    int8ArrImpl[temp.Int64] = (sbyte)right.Int32;
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Stelem_I2:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            // Handle char case
                            if(left.Ref is char[])
                            {
                                charArrImpl = (char[])left.Ref; // arr impl

                                if ((int)temp.Type <= 32)
                                {
                                    charArrImpl[temp.Int32] = (char)right.Int32;
                                }
                                else if ((int)temp.Type <= 64)
                                {
                                    charArrImpl[temp.Int64] = (char)right.Int32;
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            // Fallback to standard i16
                            else
                            {
                                int16ArrImpl = (short[])left.Ref;  // arr impl

                                if ((int)temp.Type <= 32)
                                {
                                    int16ArrImpl[temp.Int32] = (short)right.Int32;
                                }
                                else if ((int)temp.Type <= 64)
                                {
                                    int16ArrImpl[temp.Int64] = (short)right.Int32;
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Stelem_I4:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            int32ArrImpl = (int[])left.Ref;    // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                int32ArrImpl[temp.Int32] = right.Int32;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                int32ArrImpl[temp.Int64] = right.Int32;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Stelem_I8:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            int64ArrImpl = (long[])left.Ref;   // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                int64ArrImpl[temp.Int32] = right.Int64;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                int64ArrImpl[temp.Int64] = right.Int64;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Stelem_R4:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            singleArrImpl = (float[])left.Ref; // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                singleArrImpl[temp.Int32] = right.Single;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                singleArrImpl[temp.Int64] = right.Single;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Stelem_R8:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            doubleArrImpl = (double[])left.Ref;    // arr impl

                            if ((int)temp.Type <= 32)
                            {
                                doubleArrImpl[temp.Int32] = right.Double;
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                doubleArrImpl[temp.Int64] = right.Double;
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Stelem_Any:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            if ((int)temp.Type <= 32)
                            {
                                ((Array)left.Ref).SetValue(right.UnboxAsType(instruction.typeOperand), temp.Int32);
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                ((Array)left.Ref).SetValue(right.UnboxAsType(instruction.typeOperand), temp.Int64);
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Stelem_Ref:
                        {
                            right = stack[--stackPtr];              // element
                            temp = stack[--stackPtr];               // index
                            left = stack[--stackPtr];               // arr

                            if (left.Ref == null)
                                throw new NullReferenceException();

                            if ((int)temp.Type <= 32)
                            {
                                ((Array)left.Ref).SetValue(right.Box(), temp.Int32);
                            }
                            else if ((int)temp.Type <= 64)
                            {
                                ((Array)left.Ref).SetValue(right.Box(), temp.Int64);
                            }
                            else
                                throw new NotSupportedException();
                            break;
                        }

                    case Code.Ldlen:
                        {
                            temp = stack[--stackPtr];           // arr

                            if (temp.Ref == null)
                                throw new NullReferenceException();

                            stack[stackPtr].Int32 = ((Array)temp.Ref).Length;
                            stack[stackPtr++].Type = StackType.UInt32;
                            break;
                        }
#endregion

#region Field
                    case Code.Ldsfld:
                        {
                            fieldAccess = (CILFieldAccess)instruction.objectOperand;

                            // Check for direct access delegate
                            if(fieldAccess.directReadAccessDelegate != null)
                            {
                                // No instance + add 1 to stack
                                fieldAccess.directReadAccessDelegate(stack, stackPtr);
                                stackPtr++;
                                break;
                            }

                            if (fieldAccess.isClrField == true)
                            {
                                (fieldAccess.targetField as CLRField).GetValueStack(default(StackData), ref stack[stackPtr++]);
                            }
                            else
                            {
                                StackData.AllocTyped(ref stack[stackPtr++], fieldAccess.fieldTypeInfo, fieldAccess.targetField.GetValue(null));
                            }
                            break;
                        }

                    case Code.Ldsflda:
                        {
                            fieldAccess = (CILFieldAccess)instruction.objectOperand;

                            // Get address of field value
                            __internal.__gc_alloc_addr_fld(ref stack[stackPtr++], fieldAccess, default);
                            break;
                        }

                    case Code.Ldfld:
                        {
                            fieldAccess = (CILFieldAccess)instruction.objectOperand;

                            // Check for direct access delegate
                            if(fieldAccess.directReadAccessDelegate != null)
                            {
                                fieldAccess.directReadAccessDelegate(stack, stackPtr - 1);
                                break;
                            }

                            temp = stack[--stackPtr];       // inst

                            // Check null
                            if (StackData.NullCheck(temp) == true)
                                throw new NullReferenceException();

                            if (fieldAccess.isClrField == true)
                            {
                                if (temp.Type == StackType.ByRef)
                                {
                                    (fieldAccess.targetField as CLRField).GetValueStack(((IByRef)temp.Ref).GetReferenceValue(), ref stack[stackPtr++]);
                                    break;
                                }

                                (fieldAccess.targetField as CLRField).GetValueStack(temp, ref stack[stackPtr++]);
                            }
                            else
                            {
                                if (temp.Type == StackType.ByRef)
                                {
                                    object instByRef = ((IByRef)temp.Ref).GetReferenceValue().Box();

                                    if (fieldAccess.isClrField == false)
                                        instByRef = instByRef.Unwrap();

                                    StackData.AllocTyped(ref stack[stackPtr++], fieldAccess.fieldTypeInfo, fieldAccess.targetField.GetValue(instByRef));
                                    break;
                                }

                                object inst = temp.Box();

                                if (fieldAccess.isClrField == false)
                                    inst = inst.Unwrap();

                                StackData.AllocTyped(ref stack[stackPtr++], fieldAccess.fieldTypeInfo, fieldAccess.targetField.GetValue(inst));
                            }
                            break;
                        }

                    case Code.Ldflda:
                        {
                            fieldAccess = (CILFieldAccess)instruction.objectOperand;

                            temp = stack[--stackPtr];       // inst

                            // Check null
                            if (StackData.NullCheck(temp) == true)
                                throw new NullReferenceException();

                            // Get address of field value
                            __internal.__gc_alloc_addr_fld(ref stack[stackPtr++], fieldAccess, temp);

                            //stack[stackPtr].refValue = new ByRefField(fieldAccess, temp);
                            //stack[stackPtr++].type = StackData.ObjectType.ByRef;
                            break;
                        }

                    case Code.Stsfld:
                        {
                            fieldAccess = (CILFieldAccess)instruction.objectOperand;

                            // Check for direct access delegate
                            if(fieldAccess.directWriteAccessDelegate != null)
                            {
                                fieldAccess.directWriteAccessDelegate(stack, stackPtr - 1);
                                break;
                            }

                            right = stack[--stackPtr];      // val

                            StackData.ValueTypeCopy(ref right);

                            fieldAccess.targetField.SetValue(null, right.UnboxAsType(fieldAccess.fieldTypeInfo));
                            break;
                        }

                    case Code.Stfld:
                        {
                            fieldAccess = (CILFieldAccess)instruction.objectOperand;

                            // Check for direct access deleate
                            if(fieldAccess.directWriteAccessDelegate != null)
                            {
                                fieldAccess.directWriteAccessDelegate(stack, stackPtr - 2);
                                break;
                            }

                            right = stack[--stackPtr];      // val
                            temp = stack[--stackPtr];       // inst

                            // Check null
                            if (StackData.NullCheck(temp) == true)
                                throw new NullReferenceException();

                            StackData.ValueTypeCopy(ref right);

                            if (temp.Ref is IByRef)
                            {
                                object inst = (((IByRef)temp.Box()).GetReferenceValue().Ref);

                                // Check for non-clr field
                                if (fieldAccess.isClrField == false)
                                    inst = inst.Unwrap();

                                fieldAccess.targetField.SetValue(inst, right.UnboxAsType(fieldAccess.fieldTypeInfo));
                            }
                            else
                            {
                                object inst = temp.Box();

                                // Check for non-clr field
                                if (fieldAccess.isClrField == false)
                                    inst = inst.Unwrap();

                                fieldAccess.targetField.SetValue(inst, right.UnboxAsType(fieldAccess.fieldTypeInfo));
                            }
                            break;
                        }
#endregion

#region Object
                    case Code.Newobj:
                        {
                            methodInvoke = (CILMethodInvocation)instruction.objectOperand;

                            // Get target ctor
                            ConstructorInfo ctor = (ConstructorInfo)methodInvoke.targetMethod;

                            ParameterInfo[] parameters = ctor.GetParameters();
                            int argumentCount = parameters.Length;
                            int first = stackPtr - argumentCount;

                            // Create argument array
                            object[] args = new object[argumentCount];

                            for (int i = 0; i < args.Length; i++)
                            {
                                args[i] = stack[first + i].BoxAsTypeSlow(parameters[i].ParameterType);
                            }

                            // Get declaring type
                            CLRTypeInfo instanceType = instruction.typeOperand;

                            // Reset stack index
                            stackPtr = first;

                            // Allocate instance
                            __internal.__gc_alloc_inst(ref stack[stackPtr++], ref domain, instanceType.type, ctor, args);
                            break;
                        }

                    case Code.Initobj:
                        {
                            // Must pop from stack even though we do nothing
                            stackPtr--;

                            // Value types are initialized before method execution
                            break;
                        }

                    case Code.Ldtoken:
                        {
                            CILFieldAccess access = instruction.objectOperand as CILFieldAccess;
                            CILMethodInvocation invocation = instruction.objectOperand as CILMethodInvocation;
                            if(access != null)
                            {
                                stack[stackPtr].Ref = access.targetField;
                                stack[stackPtr++].Type = StackType.Ref;
                            }
                            else if(invocation != null)
                            {
                                stack[stackPtr].Ref = invocation.targetMethod;
                                stack[stackPtr++].Type = StackType.Ref;
                            }
                            else
                            {
                                stack[stackPtr].Ref = (MemberInfo)instruction.objectOperand;
                                stack[stackPtr++].Type = StackType.Ref;
                            }
                            break;
                        }

                    case Code.Ldobj:
                        {
                            while (stack[stackPtr].Ref is IByRef)
                            {
                                // Load value type from by ref
                                stack[stackPtr - 1] = ((IByRef)stack[stackPtr - 1].Ref).GetReferenceValue();
                            }

                            // Perform value type copy on stack value type
                            StackData.ValueTypeCopy(ref stack[stackPtr - 1]);
                            break;
                        }

                    case Code.Stobj:
                        {
                            right = stack[--stackPtr];
                            left = stack[--stackPtr];

                            // Overwrite source
                            ((IByRef)left.Ref).SetReferenceValue(right);
                            break;
                        }

                    case Code.Isinst:
                        {
                            temp = stack[--stackPtr];       // inst

                            // Check for null inst
                            if (temp.Ref != null)
                            {
                                Type instanceType = null;

                                if (temp.Ref.IsCLRInstance() == true)
                                {
                                    instanceType = ((CLRInstance)temp.Ref).Type;
                                }
                                else
                                {
                                    instanceType = temp.Ref.GetType();
                                }

                                // Check for assignable
                                if (TypeExtensions.AreAssignable(instanceType, instruction.typeOperand.type) == true)
                                {
                                    stack[stackPtr++] = temp;
                                    break;
                                }
                            }

                            stack[stackPtr++].Ref = null;
                            break;
                        }

                    case Code.Throw:
                        {
                            // Fetch exception
                            Exception e = (Exception)stack[--stackPtr].Ref;

                            // Update frame markers
                            frame.instructionPtr = instructionPtr;
                            frame.stackIndex = stackPtr;

                            throw e;
                        }

                    case Code.Sizeof:
                        {
                            TypeCode code = instruction.typeOperand.typeCode;

                            switch(code)
                            {
                                case TypeCode.Boolean:
                                case TypeCode.Byte:
                                case TypeCode.Char:
                                case TypeCode.SByte:
                                    {
                                        stack[stackPtr].Int32 = sizeof(byte);
                                        stack[stackPtr++].Type = StackType.UInt32;
                                        break;
                                    }
                                case TypeCode.Int16:
                                case TypeCode.UInt16:
                                    {
                                        stack[stackPtr].Int32 = sizeof(short);
                                        stack[stackPtr++].Type = StackType.UInt32;
                                        break;
                                    }

                                case TypeCode.Int32:
                                case TypeCode.UInt32:
                                case TypeCode.Single:
                                case TypeCode.String:
                                case TypeCode.Object:
                                    {
                                        stack[stackPtr].Int32 = sizeof(int);
                                        stack[stackPtr++].Type = StackType.UInt32;
                                        break;
                                    }

                                case TypeCode.Int64:
                                case TypeCode.UInt64:
                                case TypeCode.Double:
                                    {
                                        stack[stackPtr].Int32 = sizeof(long);
                                        stack[stackPtr++].Type = StackType.UInt32;
                                        break;
                                    }
                                default: throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.Call:
                    case Code.Callvirt:
                        {
                            // Try to get the method cached data
                            methodInvoke = (CILMethodInvocation)instruction.objectOperand;
                            signature = methodInvoke.signature;

                            // Get parameters
                            bool isStatic = methodInvoke.isStatic;
                            int argSize = signature.argumentCount;
                            int argOffset = stackPtr - (argSize + ((isStatic == true) ? 0 : 1));

                            // Check for virtual
                            bool virtualCall = instruction.opCode == Code.Callvirt;

                            // Load argument and instance
                            object instance = null;
                            object[] arguments;

                            // Check for static
                            if (isStatic == false)
                            {
                                // Get instance
                                instance = stack[argOffset].Box();
                            }

                            // Get the target method - used for virtual calls
                            MethodBase targetMethod = methodInvoke.targetMethod;

                            // #### Virtual Call ####
                            // Resolve virtual method based on instance
                            if (virtualCall == true && methodInvoke.isCtor == false)
                            {
                                // Require reference for instance methods
                                if (instance == null && isStatic == false)
                                    throw new NullReferenceException();

                                // Get method attributes
                                MethodAttributes attributes = methodInvoke.attributes;

                                // Check if the method can be overridden - we can save some time by only looking up virtual or abstract methods
                                if ((attributes & MethodAttributes.Virtual) != 0 || (attributes & MethodAttributes.Abstract) != 0)
                                {
                                    // Get the runtime type of the object - this is a little slow
                                    Type runtimeType = instance.IsCLRInstance() == true
                                        ? ((CLRInstance)instance).Type
                                        : instance.GetType();

                                    // Get vtable
                                    VTable vTable = methodInvoke.vTable;

                                    // Generic types need to be resolved on demand - vTable will be null in this case
                                    if (vTable == null)
                                        vTable = domain.GetMethodVTableForType(runtimeType);

                                    // Get the virtual method
                                    targetMethod = vTable.GetVirtualMethodInvocation(targetMethod, runtimeType, signature, attributes);
                                }
                            }


                            // #### Method interpreted invoke
                            if (targetMethod is CLRMethod)
                            {
                                CLRMethod clrMethod = targetMethod as CLRMethod;
                                CLRMethodBodyBase body = clrMethod.Body;

                                // Create call frame
                                ExecutionFrame callFrame;//= new ExecutionFrame(engine, targetMethod, body.MaxStack, argSize, body.Locals);
                                engine.AllocExecutionFrame(out callFrame, domain, engine, targetMethod, body.MaxStack, argSize, body.Locals);

                                int baseOffset = argSize + ((isStatic == true) ? 0 : 1);

                                // Copy stack
                                Array.Copy(stack, stackPtr - baseOffset, callFrame.stack, callFrame.stackArgIndex, baseOffset);

                                callFrame.stackIndex += baseOffset;

                                // Invoke interpreted
                                body.ExecuteMethodBody(engine, callFrame);

                                // Reset stack pointer for exiting method
                                stackPtr = argOffset + ((signature.returnsValue == true) ? 1 : 0);

                                // Copy return value
                                if(signature.returnsValue == true)
                                {
                                    stack[stackPtr - 1] = callFrame.stack[callFrame.stackBaseIndex];
                                }
                                break;
                            }


                            // #### Method direct invoke ####
                            // Use the delegate for much quick method invocation
                            if (methodInvoke.directCallDelegate != null)
                            {
                                // Load direct call arguments
                                //LoadDirectCallInvocationArguments(ref engine, ref frame, methodInvoke, argList, isStatic, argSize, argOffset, out directArguments);

                                methodInvoke.directCallDelegate(stack, argOffset);

                                // Set stack pointer
                                stackPtr = argOffset + ((signature.returnsValue == true) ? 1 : 0);
                                //LoadDirectCallInvocationReturnValue(ref frame, methodInvoke, returnType, ref returnVal, argOffset);
                                break;
                            }


                            // #### Method arguments ####
                            // Load method arguments
                            if (methodInvoke != null)
                            {
                                arguments = methodInvoke.cachedArgumentList;
                            }
                            else
                            {
                                // Try to get cached parameter array
                                if (engine.argumentCache.TryGetValue(argSize, out arguments) == false)
                                {
                                    arguments = new object[argSize];
                                    engine.argumentCache.Add(argSize, arguments);
                                }
                            }

                            int offset = isStatic == true ? 0 : 1;

                            // Create arguments
                            for (int i = 0, j = offset; i < arguments.Length; i++, j++)
                            {
                                // Check for out argument (Use 'null' as argument in this case to avoid incompatible type checking)
                                if (signature.parameters[i].IsOut == false)
                                {
                                    // Try to unbox as type
                                    arguments[i] = stack[argOffset + j].UnboxAsType(signature.parameterTypeInfos[i]);

                                    // Check for interop method
                                    if (methodInvoke.isCLRMethod == false)
                                        arguments[i] = arguments[i].UnwrapAs(signature.parameterTypes[i]);
                                }

                                // Check for by ref
                                while (arguments[i] is IByRef)
                                    arguments[i] = ((IByRef)arguments[i]).GetReferenceValue().Ref;
                            }



                            // #### Method Invoke ####
                            object invocationResult = null;

                            // Invoke the method
                            if (isStatic == true)
                            {
                                // Invoke static method
                                invocationResult = targetMethod.Invoke(null, arguments);
                            }
                            else
                            {
                                // Convert instance
                                if ((targetMethod is CLRMethod) == false)
                                {
                                    // Need to unwrap the instance for interop calls
                                    if (instance.IsCLRInstance() == true)
                                        instance = instance.Unwrap();
                                }

                                // Dereference by ref instance
                                IByRef @ref = instance as IByRef;
                                if (@ref != null)
                                    instance = @ref.GetReferenceValue().UnboxAsType(Type.GetTypeCode(targetMethod.DeclaringType));//.Box();

                                // Now we can invoke the method safely
                                invocationResult = targetMethod.Invoke(instance, arguments);
                            }


                            // Load ref/out argumnents
                            for (int i = 0, j = offset; i < arguments.Length; i++, j++)
                            {
                                // Skip args that are not passed by reference
                                if (signature.parameterTypes[i].IsByRef == false)
                                    continue;

                                if (stack[argOffset + j].Ref is IByRef)
                                    CILSignature.LoadByRefArgument(signature, (IByRef)stack[argOffset + j].Ref, arguments, signature.parameterTypeInfos, i);
                            }


                            // #### Load return value
                            // Load return value onto stack
                            if (signature.returnsValue == true)
                            {
                                StackData.AllocTyped(ref stack[argOffset], signature.returnType, invocationResult);
                                stackPtr = argOffset + 1;
                            }
                            else
                            {
                                stackPtr = argOffset;
                            }
                            break;
                        }

                    case Code.Ret:
                        {
                            frame.abort = true;

                            // Reset call frame
                            if (engine.currentFrame != null)
                                engine.currentFrame = (engine.currentFrame.Parent != null) ? engine.currentFrame.Parent : null;
                            break;
                        }

                    case Code.Ldftn:
                        {
                            stack[stackPtr].Ref = ((CILMethodInvocation)instruction.objectOperand).targetMethod;
                            stack[stackPtr++].Type = StackType.Ref;
                            break;
                        }

                    case Code.Leave_S:
                    case Code.Leave:
                        {
                            CLRExceptionHandler leaveHandler = engine.GetFinallyHandler(instructionPtr, exceptionHandlers);

                            // Get the leave instruction index
                            int leaveInstructionTarget = instructionPtr + instruction.operand.Int32 - 1;

                            // Run finally
                            if(leaveHandler != null)
                            {
                                instructionPtr = leaveHandler.handlerStartIndex;
                                stackPtr = frame.stackBaseIndex;

                                // Update frame
                                frame.instructionPtr = instructionPtr;
                                frame.stackIndex = stackPtr;

                                // Execute finally
                                ExecuteInterpreted(domain, engine, ref frame, ref instructions, ref exceptionHandlers, debugFlags);
                            }

                            // Return to target instruction
                            instructionPtr = leaveInstructionTarget;
                            stackPtr = frame.stackIndex;

                            // Continue execution from the current instruction
                            break;
                        }

                    case Code.Endfinally:
                        {
                            // Do nothing
                            break;
                        }

                    case Code.Constrained:
                        {
                            break;
                        }
#endregion

#region Stack
                    case Code.Nop:
                        {
                            // Do nothing - no operation
                            break;
                        }

                    case Code.Pop:
                        {
                            stackPtr--;
                            break;
                        }

                    case Code.Dup:
                        {
                            temp = stack[stackPtr - 1];
                            stack[stackPtr++] = temp;
                            break;
                        }
#endregion

#region Logical
                    case Code.Not:
                        {
                            switch(stack[stackPtr - 1].Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        stack[stackPtr - 1].Int32 = ~stack[stackPtr - 1].Int32;
                                        stack[stackPtr - 1].Type = StackType.Int32;
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }
                            break;
                        }

                    case Code.And:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        stack[stackPtr - 2].Int32 = left.Int32 & right.Int32;
                                        stack[stackPtr - 2].Type = StackType.Int32;
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }
                            stackPtr--;
                            break;
                        }

                    case Code.Or:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        stack[stackPtr - 2].Int32 = left.Int32 | right.Int32;
                                        stack[stackPtr - 2].Type = StackType.Int32;
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }
                            stackPtr--;
                            break;
                        }

                    case Code.Xor:
                        {
                            left = stack[stackPtr - 2];
                            right = stack[stackPtr - 1];

                            switch (left.Type)
                            {
                                case StackType.Int32:
                                case StackType.UInt32:
                                    {
                                        stack[stackPtr - 2].Int32 = left.Int32 ^ right.Int32;
                                        stack[stackPtr - 2].Type = StackType.Int32;
                                        break;
                                    }

                                default:
                                    throw new NotSupportedException();
                            }
                            stackPtr--;
                            break;
                        }
#endregion
                } // End Switch (opCode)


                /// ### WARNING - Only enable this for small snippets of non-looping (Or very shallow looping) code otherwise the performance and memory allocations will be horrific and likley cause an editor crash
#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH) && UNITY_PROFILE && UNITY_PROFILE_INSTRUCTIONS
                //UnityEngine.Profiling.Profiler.EndSample();
#endif
#endif

                // Next instruction
                instructionPtr++;

                // Check for debugger attached
                if ((debugFlags & ExecutionEngine.DebugFlags.DebuggerAttached) == 0)
                    continue;

                // Check for paused
                if ((debugFlags & ExecutionEngine.DebugFlags.DebugPause) != 0 || (debugFlags & ExecutionEngine.DebugFlags.DebugStepOnce) != 0)
                {
                    // Save execution state
                    engine.SaveExecutionState(domain, frame, instructions, exceptionHandlers);

                    frame.instructionPtr = instructionPtr;
                    frame.stackIndex = stackPtr;
                    return;
                }
            } // End while

            frame.instructionPtr = instructionPtr;
            frame.stackIndex = stackPtr;
        }
    }
}
