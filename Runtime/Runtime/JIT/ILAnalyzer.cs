using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace dotnow.Runtime.JIT
{
    internal static class ILAnalyzer
    {
        // Methods
        public static void ResolveInteropMetadataTokens(AppDomain appDomain, MethodBase interopMethod)
        {
            // Check for interop
            if (interopMethod is CLRMethodInfo or CLRConstructorInfo)
                throw new InvalidOperationException("Should only be used for interop methods");

            // Get parameters
            foreach(ParameterInfo parameter in interopMethod.GetParameters())
            {
                // Resolve token
                // Calling get handle on the meta type will force resolution if it was not already resolved
                parameter.ParameterType.GetTypeInfo(appDomain);
            }

            // Get return value
            if(interopMethod is MethodInfo methodInfo)
                methodInfo.ReturnType.GetTypeInfo(appDomain);
        }

        public static void ResolveMetadataTokens(AssemblyLoadContext assemblyLoadContext, in CILMethodInfo methodInfo)
        {
            // Check for body - cannot analyze a method with no body
            if ((methodInfo.Flags & CILMethodFlags.Body) == 0)
                return;

            // Report optimizing method
            Debug.LineFormat("JIT analyze method: {0}", methodInfo.Method);
            Stopwatch timer = Stopwatch.StartNew();


            //// Resolve signature types
            //ResolveSignatureMetadataTokens(assemblyLoadContext, methodInfo.Signature);

            //// Resolve local types
            //if(methodInfo.Body.LocalCount > 0)
            //    ResolveLocalMetadataTokens(assemblyLoadContext, methodInfo.Body.Locals);

            byte[] instructions = methodInfo.Method.GetMethodBody().GetILAsByteArray();

            // Get instruction bounds
            int pc = 0, pcMax = instructions.Length;

            // Find all instructions that use a metadata token
            while(FetchNextMetadataTokenInstruction(instructions, ref pc, pcMax, out ILOpCode opCode, out ILOperandType operandType, out EntityHandle metadataToken) == true)
            {
                // Select operand
                switch(operandType)
                {
                    case ILOperandType.InlineToken:
                        {
                            // Check token kind
                            switch(metadataToken.Kind)
                            {
                                case HandleKind.TypeDefinition:
                                case HandleKind.TypeReference:
                                case HandleKind.TypeSpecification:
                                    {
                                        // Ensure type handle is loaded
                                        assemblyLoadContext.ResolveType(metadataToken);

                                        Debug.Line("Resolve type: " + assemblyLoadContext.GetTypeHandle(metadataToken));
                                        break;
                                    }
                                case HandleKind.FieldDefinition:
                                    {
                                        // Ensure field handle is loaded
                                        assemblyLoadContext.ResolveField(metadataToken);

                                        Debug.Line("Resolve field: " + assemblyLoadContext.GetFieldHandle(metadataToken));
                                        break;
                                    }
                                case HandleKind.MethodDefinition:
                                    {
                                        // Ensure method handle is loaded
                                        assemblyLoadContext.ResolveMethod(metadataToken);

                                        Debug.Line("Resolve method: " + assemblyLoadContext.GetMethodHandle(metadataToken));
                                        break;
                                    }
                                default:
                                    throw new NotSupportedException(metadataToken.Kind.ToString());
                            }
                            break;
                        }
                    case ILOperandType.InlineType:
                        {
                            // Ensure type handle is loaded
                            assemblyLoadContext.ResolveType(metadataToken);

                            Debug.Line("Resolve type: " + assemblyLoadContext.GetTypeHandle(metadataToken));
                            break;
                        }
                    case ILOperandType.InlineField:
                        {
                            // Ensure field handle is loaded
                            assemblyLoadContext.ResolveField(metadataToken);

                            Debug.Line("Resolve field: " + assemblyLoadContext.GetFieldHandle(metadataToken));
                            break;
                        }
                    case ILOperandType.InlineMethod:
                        {
                            // Ensure method handle is loaded
                            assemblyLoadContext.ResolveMethod(metadataToken);

                            Debug.Line("Resolve method: " + assemblyLoadContext.GetMethodHandle(metadataToken));
                            break;
                        }
                }
            }

            Debug.LineFormat("Analyze IL: '{0}.{1} took: {2}ms", methodInfo.Method.DeclaringType.Name, methodInfo.Method.Name, timer.Elapsed.TotalMilliseconds);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool FetchNextMetadataTokenInstruction(byte[] instructions, ref int pc, int pcMax, out ILOpCode opCode, out ILOperandType operandType, out EntityHandle metadataToken)
        {
            opCode = 0;
            operandType = 0;
            metadataToken = default;

            // Process all instructions
            while (pc < pcMax)
            {
                // Fetch decode opcode
                opCode = (ILOpCode)CILInterpreter.FetchDecode<byte>(instructions, ref pc);

                // Check for 2-byte encoded instructions
                if ((byte)opCode == 0xFE)
                    opCode = (ILOpCode)(((byte)opCode << 8) | CILInterpreter.FetchDecode<byte>(instructions, ref pc));

                // Get operand type
                operandType = opCode.GetOperandType();

                // Check for prefix
                if (opCode.IsPrefix() == true)
                {
                    // Add the operand size
                    pc += opCode.GetOperandSize();

                    // Skip over the prefix instruction
                    continue;
                }

                // Select operand
                switch (operandType)
                {
                    case ILOperandType.InlineToken:
                    case ILOperandType.InlineType:
                    case ILOperandType.InlineField:
                    case ILOperandType.InlineMethod:
                    //case ILOperandType.InlineString:
                        {
                            // Fetch the token and increment the pc by the size of the token
                            int token = CILInterpreter.FetchDecode<int>(instructions, ref pc);

                            // Get the token
                            metadataToken = MetadataTokens.EntityHandle(token);
                            
                            // Exit early now that we have found the next token
                            return true;
                        }
                }

                // Increment by operand size
                pc += opCode.GetOperandSize();
            }

            // No next metadata token instruction found
            return false;
        }
    }
}
