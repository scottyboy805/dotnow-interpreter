using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrivialCLR.Tests
{
    [TestClass]
    public class UnitTestReflection
    {
        [TestMethod]
        public void TestPrivateProperty()
        {
            Type type = TestUtils.LoadTestType("TestReflection");

            PropertyInfo prop = type.GetProperty("PropertyTest", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Assert.IsNotNull(prop);
        }

        [TestMethod]
        public void TestPrivateProperties()
        {
            Type type = TestUtils.LoadTestType("TestReflection");

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            Assert.AreEqual(2, properties.Length);
        }

        [TestMethod]
        public void TestPrivateOnlyProperties()
        {
            Type type = TestUtils.LoadTestType("TestReflection");

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.AreEqual(1, properties.Length);
        }

        [TestMethod]
        public void TestMemberProperties()
        {
            Type type = TestUtils.LoadTestType("TestReflection");

            MemberInfo[] members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MemberInfo[] properties = members.Where(m => m is PropertyInfo).ToArray();

            Assert.AreEqual(2, properties.Length);
        }
    }
}
