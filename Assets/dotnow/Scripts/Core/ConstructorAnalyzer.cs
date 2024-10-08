using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dotnow
{
    public static class MonoCecilBaseConstructorAnalyzer
    {
        // Dictionaries to hold the pop and push counts for opcodes
        private static readonly Dictionary<Code, int> OpCodePopCounts = new Dictionary<Code, int>();
        private static readonly Dictionary<Code, int> OpCodePushCounts = new Dictionary<Code, int>();

        // Static constructor to initialize the opcode mappings
        static MonoCecilBaseConstructorAnalyzer()
        {
            InitializeOpCodeCounts();
        }

        public static List<MethodDefinition> GenerateArgumentMethods(MethodDefinition childConstructor)
        {
            var baseConstructorCall = FindBaseConstructorCall(childConstructor);
            if (baseConstructorCall == null)
                return new List<MethodDefinition>();

            var argumentInstructions = ExtractArgumentInstructions(childConstructor, baseConstructorCall);
            return CreateArgumentMethods(childConstructor, argumentInstructions, baseConstructorCall);
        }

        private static MethodReference FindBaseConstructorCall(MethodDefinition childConstructor)
        {
            var baseType = childConstructor.DeclaringType.BaseType;
            var instructions = childConstructor.Body.Instructions;

            foreach (var instruction in instructions)
            {
                if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt)
                {
                    if (instruction.Operand is MethodReference calledMethod &&
                        calledMethod.DeclaringType.FullName == baseType.FullName &&
                        calledMethod.Name == ".ctor")
                    {
                        return calledMethod;
                    }
                }
            }

            return null;
        }

        private class StackEntry
        {
            public List<Instruction> Instructions { get; } = new List<Instruction>();
        }

        private static List<List<Instruction>> ExtractArgumentInstructions(MethodDefinition childConstructor, MethodReference baseConstructor)
        {
            var instructions = childConstructor.Body.Instructions;
            var baseCallIndex = instructions.IndexOf(instructions.First(i => i.Operand == baseConstructor));
            var evalStack = new Stack<StackEntry>();
            var argumentInstructions = new List<List<Instruction>>();

            for (int i = 0; i <= baseCallIndex; i++)
            {
                var instruction = instructions[i];
                var opCode = instruction.OpCode;

                // Handle stack pops
                int popCount = GetPopCount(instruction);
                var poppedEntries = new List<StackEntry>();
                for (int j = 0; j < popCount; j++)
                {
                    if (evalStack.Count == 0)
                        throw new Exception($"Stack underflow at instruction index {i}, opcode {opCode}");

                    poppedEntries.Insert(0, evalStack.Pop());
                }

                // Special handling for call instructions
                if (opCode == OpCodes.Call || opCode == OpCodes.Callvirt)
                {
                    var methodRef = (MethodReference)instruction.Operand;
                    int argCount = methodRef.Parameters.Count + (methodRef.HasThis ? 1 : 0);

                    // For base constructor call, extract the arguments
                    if (methodRef == baseConstructor)
                    {
                        // The arguments are the popped entries
                        for (int k = 0; k < argCount; k++)
                        {
                            var argEntry = poppedEntries[k];
                            argumentInstructions.Add(argEntry.Instructions);
                        }
                        // We can exit the loop since we've found the base constructor call
                        break;
                    }
                }

                // Handle stack pushes
                int pushCount = GetPushCount(instruction);
                for (int p = 0; p < pushCount; p++)
                {
                    var newEntry = new StackEntry();

                    // For instructions that push a result based on popped entries
                    if (popCount > 0)
                    {
                        foreach (var entry in poppedEntries)
                        {
                            newEntry.Instructions.AddRange(entry.Instructions);
                        }
                    }

                    newEntry.Instructions.Add(instruction);
                    evalStack.Push(newEntry);
                }
            }

            // Reverse the argument instructions to match the correct order
            argumentInstructions.Reverse();

            return argumentInstructions;
        }

        private static int GetPopCount(Instruction instruction)
        {
            if (OpCodePopCounts.TryGetValue(instruction.OpCode.Code, out int count))
            {
                // Handle special cases where pop count depends on the operand
                if (instruction.OpCode.Code == Code.Call || instruction.OpCode.Code == Code.Callvirt)
                {
                    var methodRef = (MethodReference)instruction.Operand;
                    int argCount = methodRef.Parameters.Count + (methodRef.HasThis ? 1 : 0);
                    return argCount;
                }
                else if (instruction.OpCode.Code == Code.Newobj)
                {
                    var methodRef = (MethodReference)instruction.Operand;
                    int argCount = methodRef.Parameters.Count;
                    return argCount;
                }

                return count;
            }
            else
            {
                throw new NotSupportedException($"Unsupported opcode for pop count: {instruction.OpCode.Code}");
            }
        }

        private static int GetPushCount(Instruction instruction)
        {
            if (OpCodePushCounts.TryGetValue(instruction.OpCode.Code, out int count))
            {
                // Handle special cases where push count depends on the operand
                if (instruction.OpCode.Code == Code.Call || instruction.OpCode.Code == Code.Callvirt)
                {
                    var methodRef = (MethodReference)instruction.Operand;
                    return methodRef.ReturnType.FullName == "System.Void" ? 0 : 1;
                }
                else if (instruction.OpCode.Code == Code.Newobj)
                {
                    return 1; // Constructor returns a new object
                }

                return count;
            }
            else
            {
                throw new NotSupportedException($"Unsupported opcode for push count: {instruction.OpCode.Code}");
            }
        }

        private static void InitializeOpCodeCounts()
        {
            // Initialize pop and push counts for all opcodes
            foreach (FieldInfo field in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.GetValue(null) is OpCode opCode)
                {
                    // Determine pop count
                    int popCount = GetStackBehaviourPopCount(opCode.StackBehaviourPop);

                    // Determine push count
                    int pushCount = GetStackBehaviourPushCount(opCode.StackBehaviourPush);

                    // Add to dictionaries
                    OpCodePopCounts[opCode.Code] = popCount;
                    OpCodePushCounts[opCode.Code] = pushCount;
                }
            }
        }

        private static int GetStackBehaviourPopCount(StackBehaviour behaviour)
        {
            switch (behaviour)
            {
                case StackBehaviour.Pop0:
                    return 0;
                case StackBehaviour.Pop1:
                case StackBehaviour.Popi:
                case StackBehaviour.Popref:
                    return 1;
                case StackBehaviour.Pop1_pop1:
                case StackBehaviour.Popi_pop1:
                case StackBehaviour.Popi_popi:
                case StackBehaviour.Popi_popr4:
                case StackBehaviour.Popi_popr8:
                case StackBehaviour.Popref_pop1:
                case StackBehaviour.Popref_popi:
                    return 2;
                case StackBehaviour.Popi_popi_popi:
                case StackBehaviour.Popref_popi_popi:
                case StackBehaviour.Popref_popi_popi8:
                case StackBehaviour.Popref_popi_popr4:
                case StackBehaviour.Popref_popi_popr8:
                case StackBehaviour.Popref_popi_popref:
                    return 3;
                case StackBehaviour.PopAll:
                    return int.MaxValue; // Special case, handle appropriately
                case StackBehaviour.Varpop:
                    return -1; // Variable pop count, handle specially
                default:
                    return 0;
            }
        }

        private static int GetStackBehaviourPushCount(StackBehaviour behaviour)
        {
            switch (behaviour)
            {
                case StackBehaviour.Push0:
                    return 0;
                case StackBehaviour.Push1:
                case StackBehaviour.Pushi:
                case StackBehaviour.Pushi8:
                case StackBehaviour.Pushr4:
                case StackBehaviour.Pushr8:
                case StackBehaviour.Pushref:
                    return 1;
                case StackBehaviour.Push1_push1:
                    return 2;
                case StackBehaviour.Varpush:
                    return -1; // Variable push count, handle specially
                default:
                    return 0;
            }
        }

        private static List<MethodDefinition> CreateArgumentMethods(MethodDefinition childConstructor, List<List<Instruction>> argumentInstructions, MethodReference baseConstructor)
        {
            var methods = new List<MethodDefinition>();
            for (int t = 0; t < baseConstructor.Parameters.Count(); t++)
            {
                var argType = baseConstructor.Parameters[t].ParameterType;

                var method = new MethodDefinition(
                    $"GetBaseConstructorArg_{t}",
                    Mono.Cecil.MethodAttributes.Public,
                    argType // Set return type to argument type
                );

                // Copy parameter definitions from the child constructor
                foreach (var parameter in childConstructor.Parameters)
                {
                    method.Parameters.Add(new ParameterDefinition(parameter.Name, Mono.Cecil.ParameterAttributes.None, parameter.ParameterType));
                }

                // Copy variables
                var variableMap = new Dictionary<VariableDefinition, VariableDefinition>();
                if (childConstructor.Body.HasVariables)
                {
                    foreach (var variable in childConstructor.Body.Variables)
                    {
                        var newVariable = new VariableDefinition(variable.VariableType);
                        method.Body.Variables.Add(newVariable);
                        variableMap[variable] = newVariable;
                    }
                    method.Body.InitLocals = true;
                }

                var il = method.Body.GetILProcessor();

                // Map old instructions to new instructions
                var instructionMap = new Dictionary<Instruction, Instruction>();

                // Clone instructions that produce the argument
                foreach (var instruction in argumentInstructions[t])
                {
                    var clonedInstruction = CloneInstruction(instruction, method, variableMap, instructionMap);
                    il.Append(clonedInstruction);
                    instructionMap[instruction] = clonedInstruction;
                }

                // Fix branch targets
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.Operand is Instruction targetInstruction)
                    {
                        instruction.Operand = instructionMap[targetInstruction];
                    }
                    else if (instruction.Operand is Instruction[] targetInstructions)
                    {
                        var newTargets = new Instruction[targetInstructions.Length];
                        for (int i = 0; i < targetInstructions.Length; i++)
                        {
                            newTargets[i] = instructionMap[targetInstructions[i]];
                        }
                        instruction.Operand = newTargets;
                    }
                }

                // Add a return instruction
                il.Emit(OpCodes.Ret);

                methods.Add(method);
            }

            return methods;
        }

        private static Instruction CloneInstruction(Instruction original, MethodDefinition newMethod, Dictionary<VariableDefinition, VariableDefinition> variableMap, Dictionary<Instruction, Instruction> instructionMap)
        {
            Instruction instruction;
            switch (original.Operand)
            {
                case ParameterDefinition param:
                    instruction = Instruction.Create(original.OpCode, newMethod.Parameters[param.Index]);
                    break;
                case VariableDefinition variableDef:
                    instruction = Instruction.Create(original.OpCode, variableMap[variableDef]);
                    break;
                case FieldReference fieldRef:
                    instruction = Instruction.Create(original.OpCode, fieldRef);
                    break;
                case MethodReference methodRef:
                    instruction = Instruction.Create(original.OpCode, methodRef);
                    break;
                case TypeReference typeRef:
                    instruction = Instruction.Create(original.OpCode, typeRef);
                    break;
                case CallSite callSite:
                    instruction = Instruction.Create(original.OpCode, callSite);
                    break;
                case string strOperand:
                    instruction = Instruction.Create(original.OpCode, strOperand);
                    break;
                case sbyte sbyteOperand:
                    instruction = Instruction.Create(original.OpCode, sbyteOperand);
                    break;
                case byte byteOperand:
                    instruction = Instruction.Create(original.OpCode, byteOperand);
                    break;
                case int intOperand:
                    instruction = Instruction.Create(original.OpCode, intOperand);
                    break;
                case long longOperand:
                    instruction = Instruction.Create(original.OpCode, longOperand);
                    break;
                case float floatOperand:
                    instruction = Instruction.Create(original.OpCode, floatOperand);
                    break;
                case double doubleOperand:
                    instruction = Instruction.Create(original.OpCode, doubleOperand);
                    break;
                case Instruction targetInstruction:
                    instruction = Instruction.Create(original.OpCode, targetInstruction); // Will fix later
                    break;
                case Instruction[] instructionArray:
                    instruction = Instruction.Create(original.OpCode, instructionArray); // Will fix later
                    break;
                default:
                    if (original.Operand == null)
                        instruction = Instruction.Create(original.OpCode);
                    else
                        throw new NotSupportedException($"Unsupported operand type: {original.Operand.GetType()}");
                    break;
            }

            return instruction;
        }
    }
}
