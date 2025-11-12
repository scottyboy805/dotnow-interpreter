using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class Conversion
    {
        // === IMPLICIT CONVERSION TESTS ===

        [Test]
        public void TestByteImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestByteImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestByteImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSByteImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestSByteImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestSByteImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestShortImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestShortImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestShortImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUShortImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestUShortImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestUShortImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIntImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIntImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestIntImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUIntImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestUIntImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestUIntImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLongImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestLongImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestLongImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestULongImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestULongImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestULongImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestFloatImplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestFloatImplicitConversions));

            // Call original
            object[] expected = TestConversion.TestFloatImplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === EXPLICIT CONVERSION TESTS ===

        [Test]
        public void TestIntExplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIntExplicitConversions));

            // Call original
            object[] expected = TestConversion.TestIntExplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestLongExplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestLongExplicitConversions));

            // Call original
            object[] expected = TestConversion.TestLongExplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestFloatExplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestFloatExplicitConversions));

            // Call original
            object[] expected = TestConversion.TestFloatExplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDoubleExplicitConversions()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestDoubleExplicitConversions));

            // Call original
            object[] expected = TestConversion.TestDoubleExplicitConversions();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === STRING CONVERSION TESTS ===

        [Test]
        public void TestIntToString()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIntToString));

            // Call original
            object[] expected = TestConversion.TestIntToString();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDoubleToString()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestDoubleToString));

            // Call original
            object[] expected = TestConversion.TestDoubleToString();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoolToString()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestBoolToString));

            // Call original
            object[] expected = TestConversion.TestBoolToString();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestStringToInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestStringToInt));

            // Call original
            object[] expected = TestConversion.TestStringToInt();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === CHECKED/UNCHECKED CONVERSION TESTS ===

        [Test]
        public void TestCheckedConversion()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestCheckedConversion));

            // Call original
            object[] expected = TestConversion.TestCheckedConversion();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUncheckedConversion()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestUncheckedConversion));

            // Call original
            object[] expected = TestConversion.TestUncheckedConversion();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        //// === NULLABLE CONVERSION TESTS ===

        //[Test]
        //public void TestNullableHasValue()
        //{
        //    // Try to load method
        //    MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestNullableHasValue));

        //    // Call original
        //    object[] expected = TestConversion.TestNullableHasValue();
        //    object[] actual = (object[])method.Invoke(null, null);

        //    // Check for equal elements
        //    CollectionAssert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void TestNullableValue()
        //{
        //    // Try to load method
        //    MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestNullableValue));

        //    // Call original
        //    object[] expected = TestConversion.TestNullableValue();
        //    object[] actual = (object[])method.Invoke(null, null);

        //    // Check for equal elements
        //    CollectionAssert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void TestNullableNoValue()
        //{
        //    // Try to load method
        //    MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestNullableNoValue));

        //    // Call original
        //    object[] expected = TestConversion.TestNullableNoValue();
        //    object[] actual = (object[])method.Invoke(null, null);

        //    // Check for equal elements
        //    CollectionAssert.AreEqual(expected, actual);
        //}

        // === CASTING OPERATION TESTS ===

        [Test]
        public void TestUnboxingCast()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestUnboxingCast));

            // Call original
            object[] expected = TestConversion.TestUnboxingCast();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestBoxingCast()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestBoxingCast));

            // Call original
            object[] expected = TestConversion.TestBoxingCast();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAsOperatorSuccess()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestAsOperatorSuccess));

            // Call original
            object[] expected = TestConversion.TestAsOperatorSuccess();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestAsOperatorFail()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestAsOperatorFail));

            // Call original
            object[] expected = TestConversion.TestAsOperatorFail();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIsOperatorTrue()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIsOperatorTrue));

            // Call original
            object[] expected = TestConversion.TestIsOperatorTrue();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIsOperatorFalse()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIsOperatorFalse));

            // Call original
            object[] expected = TestConversion.TestIsOperatorFalse();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === SIGNED/UNSIGNED CONVERSION TESTS ===

        [Test]
        public void TestSignedToUnsignedInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestSignedToUnsignedInt));

            // Call original
            object[] expected = TestConversion.TestSignedToUnsignedInt();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnsignedToSignedInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestUnsignedToSignedInt));

            // Call original
            object[] expected = TestConversion.TestUnsignedToSignedInt();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSignedToUnsignedByte()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestSignedToUnsignedByte));

            // Call original
            object[] expected = TestConversion.TestSignedToUnsignedByte();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUnsignedToSignedByte()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestUnsignedToSignedByte));

            // Call original
            object[] expected = TestConversion.TestUnsignedToSignedByte();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === FLOATING-POINT CONVERSION TESTS ===

        [Test]
        public void TestFloatToDoubleInfinity()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestFloatToDoubleInfinity));

            // Call original
            object[] expected = TestConversion.TestFloatToDoubleInfinity();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestFloatToDoubleNegInfinity()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestFloatToDoubleNegInfinity));

            // Call original
            object[] expected = TestConversion.TestFloatToDoubleNegInfinity();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestFloatToDoubleNaN()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestFloatToDoubleNaN));

            // Call original
            object[] expected = TestConversion.TestFloatToDoubleNaN();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDoubleToFloat()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestDoubleToFloat));

            // Call original
            object[] expected = TestConversion.TestDoubleToFloat();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestFloatPrecision()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestFloatPrecision));

            // Call original
            object[] expected = TestConversion.TestFloatPrecision();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === CHARACTER CONVERSION TESTS ===

        [Test]
        public void TestCharToInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestCharToInt));

            // Call original
            object[] expected = TestConversion.TestCharToInt();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIntToChar()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIntToChar));

            // Call original
            object[] expected = TestConversion.TestIntToChar();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestByteToChar()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestByteToChar));

            // Call original
            object[] expected = TestConversion.TestByteToChar();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUShortToChar()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestUShortToChar));

            // Call original
            object[] expected = TestConversion.TestUShortToChar();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCharToUShort()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestCharToUShort));

            // Call original
            object[] expected = TestConversion.TestCharToUShort();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCharIsLetter()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestCharIsLetter));

            // Call original
            object[] expected = TestConversion.TestCharIsLetter();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCharIsDigit()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestCharIsDigit));

            // Call original
            object[] expected = TestConversion.TestCharIsDigit();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestCharToUpper()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestCharToUpper));

            // Call original
            object[] expected = TestConversion.TestCharToUpper();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === ENUM CONVERSION TESTS ===

        [Test]
        public void TestEnumToInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestEnumToInt));

            // Call original
            object[] expected = TestConversion.TestEnumToInt();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIntToEnum()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIntToEnum));

            // Call original
            object[] expected = TestConversion.TestIntToEnum();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestEnumToString()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestEnumToString));

            // Call original
            object[] expected = TestConversion.TestEnumToString();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestEnumConstantToInt()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestEnumConstantToInt));

            // Call original
            object[] expected = TestConversion.TestEnumConstantToInt();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestZeroToEnum()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestZeroToEnum));

            // Call original
            object[] expected = TestConversion.TestZeroToEnum();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestEnumEquality()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestEnumEquality));

            // Call original
            object[] expected = TestConversion.TestEnumEquality();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === REFERENCE CONVERSION TESTS ===

        [Test]
        public void TestDowncastWithAs()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestDowncastWithAs));

            // Call original
            object[] expected = TestConversion.TestDowncastWithAs();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDowncastSuccess()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestDowncastSuccess));

            // Call original
            object[] expected = TestConversion.TestDowncastSuccess();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDowncastFail()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestDowncastFail));

            // Call original
            object[] expected = TestConversion.TestDowncastFail();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIsString()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIsString));

            // Call original
            object[] expected = TestConversion.TestIsString();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestIsObject()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestIsObject));

            // Call original
            object[] expected = TestConversion.TestIsObject();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestReferenceEquals()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestReferenceEquals));

            // Call original
            object[] expected = TestConversion.TestReferenceEquals();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestReferenceEqualsNull()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestReferenceEqualsNull));

            // Call original
            object[] expected = TestConversion.TestReferenceEqualsNull();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        // === ARRAY CONVERSION TESTS ===

        [Test]
        public void TestArrayAsObject()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestArrayAsObject));

            // Call original
            object[] expected = TestConversion.TestArrayAsObject();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestArrayTypeCheck()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestArrayTypeCheck));

            // Call original
            object[] expected = TestConversion.TestArrayTypeCheck();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestArrayLength()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestArrayLength));

            // Call original
            object[] expected = TestConversion.TestArrayLength();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestArrayElementAccess()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestArrayElementAccess));

            // Call original
            object[] expected = TestConversion.TestArrayElementAccess();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestArrayBoxing()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestArrayBoxing));

            // Call original
            object[] expected = TestConversion.TestArrayBoxing();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TestArrayIsArray()
        {
            // Try to load method
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestConversion), nameof(TestConversion.TestArrayIsArray));

            // Call original
            object[] expected = TestConversion.TestArrayIsArray();
            object[] actual = (object[])method.Invoke(null, null);

            // Check for equal elements
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}