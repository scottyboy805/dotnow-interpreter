//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using Trivial.Mono.Cecil.Cil;
//using dotnow.Runtime.JIT;

//namespace dotnow.Tests
//{
//    [TestClass]
//    public class UnitTestILGenerator
//    {
//        [TestMethod]
//        public void TestEmit()
//        {
//            AppDomain domain = new AppDomain();
//            ExecutableILGenerator il = new ExecutableILGenerator(domain);

//            il.Emit(Code.Ldarg_0);
//            il.Emit(Code.Ldarg_1);
//            il.Emit(Code.Add);
//            il.Emit(Code.Ldarg_2);
//            il.Emit(Code.Div);
//            il.Emit(Code.Ret);

//            MethodBase method = il.BuildExecutableMethod(new Type[0], 2, typeof(int), typeof(int), typeof(int), typeof(int));

//            Assert.AreEqual(15, method.Invoke(null, new object[] { 10, 20, 2 }));
//        }
//    }
//}
