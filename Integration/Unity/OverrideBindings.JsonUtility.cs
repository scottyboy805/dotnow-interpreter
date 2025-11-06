//using dotnow;
//using System;
//using System.Reflection;

//namespace UnityEngine
//{
//    public static partial class OverrideBindings
//    {
//		[Preserve]
//		[CLRMethodBinding(typeof(JsonUtility), "ToJson", typeof(object))]
//		public static object ToJsonOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//		{
//			// Check for CLR instance
//			if (args[0] != null && args[0].IsCLRInstance() == true)
//				throw new NotSupportedException("CLR types are not supported by JsonUtility");

//			// Call utility
//			return JsonUtility.ToJson(args[0]);
//		}

//		[Preserve]
//		[CLRMethodBinding(typeof(JsonUtility), "ToJson", typeof(object), typeof(bool))]
//		public static object ToJsonPrettyOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//		{
//			// Check for CLR instance
//			if (args[0] != null && args[0].IsCLRInstance() == true)
//				throw new NotSupportedException("CLR types are not supported by JsonUtility");

//			// Call utility
//			return JsonUtility.ToJson(args[0], (bool)args[1]);
//		}

//		[Preserve]
//		[CLRGenericMethodBinding(typeof(JsonUtility), "FromJson", 1, typeof(string))]
//		public static object FromJsonOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
//		{
//			// Check for CLR type
//			if (genericTypes[0].IsCLRType() == true)
//				throw new NotSupportedException("CLR types are not supported by JsonUtility");

//			// Call utility
//			return JsonUtility.FromJson(args[0] as string, genericTypes[0]);
//		}

//		[Preserve]
//		[CLRMethodBinding(typeof(JsonUtility), "FromJson", typeof(string), typeof(Type))]
//		public static object FromJsonTypeOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//		{
//			// Check for CLR type
//			if (((Type)args[1]).IsCLRType() == true)
//				throw new NotSupportedException("CLR types are not supported by JsonUtility");

//			// Call utility
//			return JsonUtility.FromJson(args[0] as string, (Type)args[1]);
//		}

//		[Preserve]
//		[CLRMethodBinding(typeof(JsonUtility), "FromJsonOverwrite", typeof(string), typeof(object))]
//		public static void FromJsonOverwriteOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//		{
//			// Check for CLR instance
//			if (args[1] != null && args[1].IsCLRInstance() == true)
//				throw new NotSupportedException("CLR types are not supported by JsonUtility");

//			// Call utility
//			JsonUtility.FromJsonOverwrite(args[0] as string, args[1]);
//		}
//	}
//}
