using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Arithmetic
    {
        [Test]
        public void TestDecimal()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecimal));

            // Call original
            object expected = TestArithmetic.TestDecimal();
            object actual = method.Invoke(null, null);

            // Check for equal elements
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR ADDITION TESTS BY TYPE =====
     
        [Test]
        public void TestAdditionInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionInt));
            object expected = TestArithmetic.TestAdditionInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionUInt));
            object expected = TestArithmetic.TestAdditionUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionDouble));
            object expected = TestArithmetic.TestAdditionDouble();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionFloat));
            object expected = TestArithmetic.TestAdditionFloat();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionDecimal));
            object expected = TestArithmetic.TestAdditionDecimal();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionLong));
            object expected = TestArithmetic.TestAdditionLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionULong));
            object expected = TestArithmetic.TestAdditionULong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionShort));
            object expected = TestArithmetic.TestAdditionShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionUShort));
            object expected = TestArithmetic.TestAdditionUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionByte));
            object expected = TestArithmetic.TestAdditionByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionSByte));
            object expected = TestArithmetic.TestAdditionSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAdditionChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestAdditionChar));
            object expected = TestArithmetic.TestAdditionChar();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR SUBTRACTION TESTS BY TYPE =====

        [Test]
        public void TestSubtractionInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionInt));
            object expected = TestArithmetic.TestSubtractionInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionUInt));
            object expected = TestArithmetic.TestSubtractionUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionDouble));
            object expected = TestArithmetic.TestSubtractionDouble();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionFloat));
            object expected = TestArithmetic.TestSubtractionFloat();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionDecimal));
            object expected = TestArithmetic.TestSubtractionDecimal();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionLong));
            object expected = TestArithmetic.TestSubtractionLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionULong));
            object expected = TestArithmetic.TestSubtractionULong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionShort));
            object expected = TestArithmetic.TestSubtractionShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionUShort));
            object expected = TestArithmetic.TestSubtractionUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionByte));
            object expected = TestArithmetic.TestSubtractionByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionSByte));
            object expected = TestArithmetic.TestSubtractionSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSubtractionChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestSubtractionChar));
            object expected = TestArithmetic.TestSubtractionChar();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR MULTIPLICATION TESTS BY TYPE =====

        [Test]
        public void TestMultiplicationInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationInt));
            object expected = TestArithmetic.TestMultiplicationInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationUInt));
            object expected = TestArithmetic.TestMultiplicationUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationDouble));
            object expected = TestArithmetic.TestMultiplicationDouble();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationFloat));
            object expected = TestArithmetic.TestMultiplicationFloat();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationDecimal));
            object expected = TestArithmetic.TestMultiplicationDecimal();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationLong));
            object expected = TestArithmetic.TestMultiplicationLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationULong));
            object expected = TestArithmetic.TestMultiplicationULong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationShort));
            object expected = TestArithmetic.TestMultiplicationShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationUShort));
            object expected = TestArithmetic.TestMultiplicationUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationByte));
            object expected = TestArithmetic.TestMultiplicationByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationSByte));
            object expected = TestArithmetic.TestMultiplicationSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestMultiplicationChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestMultiplicationChar));
            object expected = TestArithmetic.TestMultiplicationChar();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR DIVISION TESTS BY TYPE =====

        [Test]
        public void TestDivisionInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionInt));
            object expected = TestArithmetic.TestDivisionInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionUInt));
            object expected = TestArithmetic.TestDivisionUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionDouble));
            object expected = TestArithmetic.TestDivisionDouble();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionFloat));
            object expected = TestArithmetic.TestDivisionFloat();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionDecimal));
            object expected = TestArithmetic.TestDivisionDecimal();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionLong));
            object expected = TestArithmetic.TestDivisionLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionULong));
            object expected = TestArithmetic.TestDivisionULong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionShort));
            object expected = TestArithmetic.TestDivisionShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionUShort));
            object expected = TestArithmetic.TestDivisionUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionByte));
            object expected = TestArithmetic.TestDivisionByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionSByte));
            object expected = TestArithmetic.TestDivisionSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDivisionChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDivisionChar));
            object expected = TestArithmetic.TestDivisionChar();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR MODULO TESTS BY TYPE =====

        [Test]
        public void TestModuloInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloInt));
            object expected = TestArithmetic.TestModuloInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloUInt));
            object expected = TestArithmetic.TestModuloUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloDouble));
            object expected = TestArithmetic.TestModuloDouble();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloFloat));
            object expected = TestArithmetic.TestModuloFloat();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloDecimal));
            object expected = TestArithmetic.TestModuloDecimal();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloLong));
            object expected = TestArithmetic.TestModuloLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloULong));
            object expected = TestArithmetic.TestModuloULong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloShort));
            object expected = TestArithmetic.TestModuloShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloUShort));
            object expected = TestArithmetic.TestModuloUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloByte));
            object expected = TestArithmetic.TestModuloByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestModuloSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestModuloSByte));
            object expected = TestArithmetic.TestModuloSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR UNARY MINUS TESTS BY TYPE =====

        [Test]
        public void TestUnaryMinusInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestUnaryMinusInt));
            object expected = TestArithmetic.TestUnaryMinusInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnaryMinusDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestUnaryMinusDouble));
            object expected = TestArithmetic.TestUnaryMinusDouble();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnaryMinusFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestUnaryMinusFloat));
            object expected = TestArithmetic.TestUnaryMinusFloat();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnaryMinusDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestUnaryMinusDecimal));
            object expected = TestArithmetic.TestUnaryMinusDecimal();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnaryMinusLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestUnaryMinusLong));
            object expected = TestArithmetic.TestUnaryMinusLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnaryMinusShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestUnaryMinusShort));
            object expected = TestArithmetic.TestUnaryMinusShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnaryMinusSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestUnaryMinusSByte));
            object expected = TestArithmetic.TestUnaryMinusSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR INCREMENT TESTS BY TYPE =====

        [Test]
        public void TestIncrementInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementInt));
            object expected = TestArithmetic.TestIncrementInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementUInt));
            object expected = TestArithmetic.TestIncrementUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementDouble));
            object expected = TestArithmetic.TestIncrementDouble();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementFloat));
            object expected = TestArithmetic.TestIncrementFloat();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementDecimal));
            object expected = TestArithmetic.TestIncrementDecimal();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementLong));
            object expected = TestArithmetic.TestIncrementLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementULong));
            object expected = TestArithmetic.TestIncrementULong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementShort));
            object expected = TestArithmetic.TestIncrementShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementUShort));
            object expected = TestArithmetic.TestIncrementUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementByte));
            object expected = TestArithmetic.TestIncrementByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementSByte));
            object expected = TestArithmetic.TestIncrementSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIncrementChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestIncrementChar));
            object expected = TestArithmetic.TestIncrementChar();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== GRANULAR DECREMENT TESTS BY TYPE =====

        [Test]
        public void TestDecrementInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementInt));
            object expected = TestArithmetic.TestDecrementInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementUInt));
            object expected = TestArithmetic.TestDecrementUInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementDouble));
            object expected = TestArithmetic.TestDecrementDouble();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementFloat));
            object expected = TestArithmetic.TestDecrementFloat();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementDecimal));
            object expected = TestArithmetic.TestDecrementDecimal();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementLong));
            object expected = TestArithmetic.TestDecrementLong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementULong));
            object expected = TestArithmetic.TestDecrementULong();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementShort));
            object expected = TestArithmetic.TestDecrementShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementUShort));
            object expected = TestArithmetic.TestDecrementUShort();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementByte));
            object expected = TestArithmetic.TestDecrementByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementSByte));
            object expected = TestArithmetic.TestDecrementSByte();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecrementChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestDecrementChar));
            object expected = TestArithmetic.TestDecrementChar();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== POST-INCREMENT AND POST-DECREMENT TESTS =====

        [Test]
        public void TestPostIncrementInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestPostIncrementInt));
            object expected = TestArithmetic.TestPostIncrementInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestPostDecrementInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestArithmetic), nameof(TestArithmetic.TestPostDecrementInt));
            object expected = TestArithmetic.TestPostDecrementInt();
            object actual = method.Invoke(null, null);
            Assert.AreEqual(expected, actual);
        }

        // ===== LEGACY COMPREHENSIVE TESTS FOR BACKWARD COMPATIBILITY =====
    }
}
