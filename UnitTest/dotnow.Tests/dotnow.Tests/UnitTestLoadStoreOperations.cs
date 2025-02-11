using dotnow.Runtime;
using dotnow.Runtime.JIT;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Reflection;

namespace dotnow.Tests
{
    [TestClass]
    public class UnitTestLoadStoreOperations
    {
        [DataTestMethod]
        [DataRow(typeof(int), typeof(sbyte), DisplayName = "SByte")]
        [DataRow(typeof(int), typeof(byte), DisplayName = "Byte")]        
        [DataRow(typeof(int), typeof(short), DisplayName = "Short")]
        [DataRow(typeof(int), typeof(ushort), DisplayName = "UShort")]
        [DataRow(typeof(int), typeof(int), DisplayName = "Int")]
        [DataRow(typeof(uint), typeof(uint), DisplayName = "UInt")]
        [DataRow(typeof(long), typeof(long), DisplayName = "Long")]
        [DataRow(typeof(ulong), typeof(ulong), DisplayName = "ULong")]
        public void TestLocal0(Type expected, Type local)
        {
            TestLocalIndex(0, expected, local);
        }

        [DataTestMethod]
        [DataRow(typeof(int), typeof(sbyte), DisplayName = "SByte")]
        [DataRow(typeof(int), typeof(byte), DisplayName = "Byte")]
        [DataRow(typeof(int), typeof(short), DisplayName = "Short")]
        [DataRow(typeof(int), typeof(ushort), DisplayName = "UShort")]
        [DataRow(typeof(int), typeof(int), DisplayName = "Int")]
        [DataRow(typeof(uint), typeof(uint), DisplayName = "UInt")]
        [DataRow(typeof(long), typeof(long), DisplayName = "Long")]
        [DataRow(typeof(ulong), typeof(ulong), DisplayName = "ULong")]
        public void TestLocal1(Type expected, Type local)
        {
            TestLocalIndex(1, expected, local);
        }

        [DataTestMethod]
        [DataRow(typeof(int), typeof(sbyte), DisplayName = "SByte")]
        [DataRow(typeof(int), typeof(byte), DisplayName = "Byte")]
        [DataRow(typeof(int), typeof(short), DisplayName = "Short")]
        [DataRow(typeof(int), typeof(ushort), DisplayName = "UShort")]
        [DataRow(typeof(int), typeof(int), DisplayName = "Int")]
        [DataRow(typeof(uint), typeof(uint), DisplayName = "UInt")]
        [DataRow(typeof(long), typeof(long), DisplayName = "Long")]
        [DataRow(typeof(ulong), typeof(ulong), DisplayName = "ULong")]
        public void TestLocal2(Type expected, Type local)
        {
            TestLocalIndex(2, expected, local);
        }

        [DataTestMethod]
        [DataRow(typeof(int), typeof(sbyte), DisplayName = "SByte")]
        [DataRow(typeof(int), typeof(byte), DisplayName = "Byte")]
        [DataRow(typeof(int), typeof(short), DisplayName = "Short")]
        [DataRow(typeof(int), typeof(ushort), DisplayName = "UShort")]
        [DataRow(typeof(int), typeof(int), DisplayName = "Int")]
        [DataRow(typeof(uint), typeof(uint), DisplayName = "UInt")]
        [DataRow(typeof(long), typeof(long), DisplayName = "Long")]
        [DataRow(typeof(ulong), typeof(ulong), DisplayName = "ULong")]
        public void TestLocal3(Type expected, Type local)
        {
            TestLocalIndex(3, expected, local);
        }

        [DataTestMethod]
        [DataRow(typeof(int), typeof(sbyte), DisplayName = "SByte")]
        [DataRow(typeof(int), typeof(byte), DisplayName = "Byte")]
        [DataRow(typeof(int), typeof(short), DisplayName = "Short")]
        [DataRow(typeof(int), typeof(ushort), DisplayName = "UShort")]
        [DataRow(typeof(int), typeof(int), DisplayName = "Int")]
        [DataRow(typeof(uint), typeof(uint), DisplayName = "UInt")]
        [DataRow(typeof(long), typeof(long), DisplayName = "Long")]
        [DataRow(typeof(ulong), typeof(ulong), DisplayName = "ULong")]
        public void TestLocal4(Type expected, Type local)
        {
            TestLocalIndex(4, expected, local);
        }

        private void TestLocalIndex(int index, Type expectedType, Type localType)
        {
            if (expectedType.IsPrimitive == false || expectedType.IsValueType == false)
                throw new ArgumentException("Expected type must be a basic primitive type");

            // Get stack type
            StackData.ObjectType stackType = StackData.StackTypeFromTypeCode(Type.GetTypeCode(expectedType));
            object expectedValue = null;

            AppDomain domain = new AppDomain();

            // Generate test instructions
            ExecutableILGenerator generator = new ExecutableILGenerator(domain, 3);
            switch(stackType)
            {
                case StackData.ObjectType.Int8:
                case StackData.ObjectType.Int16:
                case StackData.ObjectType.Int32:
                    {
                        generator.Emit(Code.Ldc_I4, 128);
                        expectedValue = 128;
                        break;
                    }
                case StackData.ObjectType.UInt8:
                case StackData.ObjectType.UInt16:
                case StackData.ObjectType.UInt32:
                    {
                        generator.Emit(Code.Ldc_I4, 128);
                        expectedValue = (uint)128;
                        break;
                    }
                case StackData.ObjectType.Int64:
                    {
                        generator.Emit(Code.Ldc_I8, (long)128);
                        expectedValue = (long)128;
                        break;
                    }
                case StackData.ObjectType.UInt64:
                    {
                        generator.Emit(Code.Ldc_I8, (long)128);
                        expectedValue = (ulong)128;
                        break;
                    }
            }            
            switch (index)
            {
                case 0:
                    {
                        generator.Emit(Code.Stloc_0);
                        generator.Emit(Code.Ldloc_0);                        
                        break;
                    }
                case 1:
                    {
                        generator.Emit(Code.Stloc_1);
                        generator.Emit(Code.Ldloc_1);                        
                        break;
                    }
                case 2:
                    {
                        generator.Emit(Code.Stloc_2);
                        generator.Emit(Code.Ldloc_2);                        
                        break;
                    }
                case 3:
                    {
                        generator.Emit(Code.Stloc_3);
                        generator.Emit(Code.Ldloc_3);                        
                        break;
                    }
                default:
                    {
                        generator.Emit(Code.Stloc, index);
                        generator.Emit(Code.Ldloc, index);                        
                        break;
                    }
            }

            // Build the method
            MethodInfo method = generator.BuildExecutableMethod(Enumerable.Repeat(localType, index + 1).ToArray(), index + 1, typeof(void));

            // Call the method
            method.Invoke(null, null);

            // Get the stack so we can check values
            StackData[] stack = domain.GetExecutionEngine().stack;

            // Check local first - should be offset 0
            Assert.AreEqual(stackType, stack[index].type);
            Assert.AreEqual(expectedValue, stack[index].BoxAsTypeSlow(expectedType));


            // Check eval stack
            Assert.AreEqual(stackType, stack[index + 1].type);
            Assert.AreEqual(expectedValue, stack[index + 1].BoxAsTypeSlow(expectedType));
        }
    }
}
