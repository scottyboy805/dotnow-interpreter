using dotnow.Reflection;
using dotnow.Runtime.CIL;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace dotnow.Runtime.JIT
{
    internal static class ILAnalyzer
    {
        // Methods
        public static unsafe void ResolveInteropMetadataTokens(AppDomain appDomain, MethodBase interopMethod)
        {
            // Check for interop
            if (interopMethod is CLRMethodInfo or CLRConstructorInfo)
                throw new InvalidOperationException("Should only be used for interop methods");

            // Get parameters
            foreach(ParameterInfo parameter in interopMethod.GetParameters())
            {
                // Resolve token
                // Calling get handle on the meta type will force resolution if it was not already resolved
                parameter.ParameterType.GetHandle(appDomain);
            }

            // Get return value
            if(interopMethod is MethodInfo methodInfo)
                methodInfo.ReturnType.GetHandle(appDomain);
        }

        public static unsafe void ResolveMetadataTokens(AssemblyLoadContext assemblyLoadContext, in CILMethodHandle methodHandle)
        {
            // Check for body - cannot analyze a method with no body
            if ((methodHandle.Flags & CILMethodFlags.Body) == 0)
                return;

            // Report optimizing method
            Debug.LineFormat("JIT analyze method: {0}", methodHandle.MetaMethod);
            Stopwatch timer = Stopwatch.StartNew();


            // Resolve signature types
            ResolveSignatureMetadataTokens(assemblyLoadContext, methodHandle.Signature);

            // Resolve local types
            if(methodHandle.Body.LocalCount > 0)
                ResolveLocalMetadataTokens(assemblyLoadContext, methodHandle.Body.Locals);

            // Get instruction bounds
            byte* pc = methodHandle.Body.Instructions.Ptr;
            byte* pcMax = methodHandle.Body.Instructions.MaxPtr;

            // Find all instructions that use a metadata token
            while(FetchNextMetadataTokenInstruction(ref pc, pcMax, out ILOpCode opCode, out ILOperandType operandType, out EntityHandle metadataToken) == true)
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
                                        break;
                                    }
                                case HandleKind.FieldDefinition:
                                    {
                                        // Ensure field handle is loaded
                                        assemblyLoadContext.ResolveField(metadataToken);
                                        break;
                                    }
                                case HandleKind.MethodDefinition:
                                    {
                                        // Ensure method handle is loaded
                                        assemblyLoadContext.ResolveMethod(metadataToken); 
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
                            break;
                        }
                    case ILOperandType.InlineField:
                        {
                            // Ensure field handle is loaded
                            assemblyLoadContext.ResolveField(metadataToken);
                            break;
                        }
                    case ILOperandType.InlineMethod:
                        {
                            // Ensure method handle is loaded
                            assemblyLoadContext.ResolveMethod(metadataToken);
                            break;
                        }
                }
            }

            Debug.LineFormat("Analyze IL: '{0}.{1} took: {2}ms", methodHandle.MetaMethod.DeclaringType.Name, methodHandle.MetaMethod.Name, timer.Elapsed.TotalMilliseconds);
        }

        public static void ResolveSignatureMetadataTokens(AssemblyLoadContext assemblyLoadContext, in CILMethodSignatureHandle signature)
        {
            // Check for any parameters
            if ((signature.Flags & CILMethodSignatureFlags.HasParameters) != 0)
            {
                // Resolve parameter types
                for (int i = 0; i < signature.Parameters.Length; i++)
                {
                    // Get the parameter
                    CILMethodVariableHandle parameterHandle = signature.Parameters[i];

                    // Resolve the parameter
                    parameterHandle.VariableTypeToken.ResolveTypeHandle(assemblyLoadContext);
                }
            }

            // Check for return
            if ((signature.Flags & CILMethodSignatureFlags.HasReturn) != 0)
            {
                // Resolve return type
                signature.Return.VariableTypeToken.ResolveTypeHandle(assemblyLoadContext);
            }
        }

        public static void ResolveLocalMetadataTokens(AssemblyLoadContext assemblyLoadContext, CILMethodVariableHandle[] locals)
        {
            // Process all
            for(int i = 0; i < locals.Length; i++)
            {
                // Get the local
                CILMethodVariableHandle localHandle = locals[i];

                // TResolve the local
                localHandle.VariableTypeToken.ResolveTypeHandle(assemblyLoadContext);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bool FetchNextMetadataTokenInstruction(ref byte* pc, byte* pcMax, out ILOpCode opCode, out ILOperandType operandType, out EntityHandle metadataToken)
        {
            opCode = 0;
            operandType = 0;
            metadataToken = default;

            // Process all instructions
            while (pc < pcMax)
            {
                // Fetch decode opcode
                opCode = (ILOpCode)(*pc++);

                // Check for 2-byte encoded instructions
                if ((byte)opCode == 0xFE)
                    opCode = (ILOpCode)(((byte)opCode << 8) | *pc++);

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
                    case ILOperandType.InlineString:
                        {
                            int normalToken = *(int*)pc;

                            // Get the token
                            metadataToken = *(EntityHandle*)pc;

                            // Increment by operand size before returning
                            pc += opCode.GetOperandSize();
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
