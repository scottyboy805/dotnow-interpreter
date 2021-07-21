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
    public class UnitTestCustomAttributes
    {
        [TestMethod]
        public void TestCustomAttribute1()
        {
            Type type = TestUtils.LoadTestType("TestCustomAttributes");

            PropertyInfo property = type.GetProperty("TestProperty");

            ObsoleteAttribute attrib = (ObsoleteAttribute)property.GetCustomAttributes(false)[0];

            Assert.AreEqual("Test Message", attrib.Message);
        }
    }
}
