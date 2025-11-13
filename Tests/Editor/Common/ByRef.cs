#if DOTNOW_ENABLE_TESTS
using NUnit.Framework;
using System.Reflection;
using TestAssembly;

namespace dotnow.Common
{
    [TestFixture]
    [Category("Unit Test")]
    public class ByRef
    {
        // ===== OUT PARAMETER TESTS BY TYPE =====

        [Test]
        public void TestOutBool()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutBool));
            object[] expected = TestByRef.TestOutBool();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutChar));
            object[] expected = TestByRef.TestOutChar();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutByte));
            object[] expected = TestByRef.TestOutByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutSByte));
            object[] expected = TestByRef.TestOutSByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutShort));
            object[] expected = TestByRef.TestOutShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutUShort));
            object[] expected = TestByRef.TestOutUShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutInt));
            object[] expected = TestByRef.TestOutInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutUInt));
            object[] expected = TestByRef.TestOutUInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutLong));
            object[] expected = TestByRef.TestOutLong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutULong));
            object[] expected = TestByRef.TestOutULong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFloat));
            object[] expected = TestByRef.TestOutFloat();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutDouble));
            object[] expected = TestByRef.TestOutDouble();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutDecimal));
            object[] expected = TestByRef.TestOutDecimal();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutString()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutString));
            object[] expected = TestByRef.TestOutString();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        // ===== REF PARAMETER TESTS BY TYPE =====

        [Test]
        public void TestRefBool()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefBool));
            object[] expected = TestByRef.TestRefBool();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefChar));
            object[] expected = TestByRef.TestRefChar();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefByte));
            object[] expected = TestByRef.TestRefByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefSByte));
            object[] expected = TestByRef.TestRefSByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefShort));
            object[] expected = TestByRef.TestRefShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefUShort));
            object[] expected = TestByRef.TestRefUShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefInt));
            object[] expected = TestByRef.TestRefInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefUInt));
            object[] expected = TestByRef.TestRefUInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefLong));
            object[] expected = TestByRef.TestRefLong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefULong));
            object[] expected = TestByRef.TestRefULong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFloat));
            object[] expected = TestByRef.TestRefFloat();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefDouble));
            object[] expected = TestByRef.TestRefDouble();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefDecimal));
            object[] expected = TestByRef.TestRefDecimal();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefString()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefString));
            object[] expected = TestByRef.TestRefString();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        // ===== IN PARAMETER TESTS BY TYPE =====

        [Test]
        public void TestInBool()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInBool));
            object[] expected = TestByRef.TestInBool();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInChar));
            object[] expected = TestByRef.TestInChar();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInByte));
            object[] expected = TestByRef.TestInByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInSByte));
            object[] expected = TestByRef.TestInSByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInShort));
            object[] expected = TestByRef.TestInShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInUShort));
            object[] expected = TestByRef.TestInUShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInInt));
            object[] expected = TestByRef.TestInInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInUInt));
            object[] expected = TestByRef.TestInUInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInLong));
            object[] expected = TestByRef.TestInLong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInULong));
            object[] expected = TestByRef.TestInULong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInFloat));
            object[] expected = TestByRef.TestInFloat();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInDouble));
            object[] expected = TestByRef.TestInDouble();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInDecimal));
            object[] expected = TestByRef.TestInDecimal();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestInString()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestInString));
            object[] expected = TestByRef.TestInString();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        // ===== OUT FIELD BYREF TESTS BY TYPE =====

        [Test]
        public void TestOutFieldBool()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldBool));
            object[] expected = TestByRef.TestOutFieldBool();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldChar));
            object[] expected = TestByRef.TestOutFieldChar();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldByte));
            object[] expected = TestByRef.TestOutFieldByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldSByte));
            object[] expected = TestByRef.TestOutFieldSByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldShort));
            object[] expected = TestByRef.TestOutFieldShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldUShort));
            object[] expected = TestByRef.TestOutFieldUShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldInt));
            object[] expected = TestByRef.TestOutFieldInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldUInt));
            object[] expected = TestByRef.TestOutFieldUInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldLong));
            object[] expected = TestByRef.TestOutFieldLong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldULong));
            object[] expected = TestByRef.TestOutFieldULong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldFloat));
            object[] expected = TestByRef.TestOutFieldFloat();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldDouble));
            object[] expected = TestByRef.TestOutFieldDouble();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldDecimal));
            object[] expected = TestByRef.TestOutFieldDecimal();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutFieldString()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutFieldString));
            object[] expected = TestByRef.TestOutFieldString();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        // ===== REF FIELD BYREF TESTS BY TYPE =====

        [Test]
        public void TestRefFieldBool()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldBool));
            object[] expected = TestByRef.TestRefFieldBool();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldChar()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldChar));
            object[] expected = TestByRef.TestRefFieldChar();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldByte));
            object[] expected = TestByRef.TestRefFieldByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldSByte()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldSByte));
            object[] expected = TestByRef.TestRefFieldSByte();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldShort));
            object[] expected = TestByRef.TestRefFieldShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldUShort()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldUShort));
            object[] expected = TestByRef.TestRefFieldUShort();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldInt));
            object[] expected = TestByRef.TestRefFieldInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldUInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldUInt));
            object[] expected = TestByRef.TestRefFieldUInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldLong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldLong));
            object[] expected = TestByRef.TestRefFieldLong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldULong()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldULong));
            object[] expected = TestByRef.TestRefFieldULong();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldFloat));
            object[] expected = TestByRef.TestRefFieldFloat();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldDouble()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldDouble));
            object[] expected = TestByRef.TestRefFieldDouble();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldDecimal()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldDecimal));
            object[] expected = TestByRef.TestRefFieldDecimal();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefFieldString()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefFieldString));
            object[] expected = TestByRef.TestRefFieldString();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        // ===== ARRAY ELEMENT BYREF TESTS =====

        [Test]
        public void TestOutArrayElementInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutArrayElementInt));
            object[] expected = TestByRef.TestOutArrayElementInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefArrayElementInt()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefArrayElementInt));
            object[] expected = TestByRef.TestRefArrayElementInt();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutArrayElementFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutArrayElementFloat));
            object[] expected = TestByRef.TestOutArrayElementFloat();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefArrayElementFloat()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefArrayElementFloat));
            object[] expected = TestByRef.TestRefArrayElementFloat();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOutArrayElementString()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestOutArrayElementString));
            object[] expected = TestByRef.TestOutArrayElementString();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRefArrayElementString()
        {
            MethodInfo method = TestUtils.LoadTestMethod(nameof(TestByRef), nameof(TestByRef.TestRefArrayElementString));
            object[] expected = TestByRef.TestRefArrayElementString();
            object[] actual = (object[])method.Invoke(null, null);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
#endif