#if NEWTONSOFT
using dotnow;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json
{
	public static partial class OverrideBindings
	{
		private const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

		[Preserve]
		[CLRMethodBinding(typeof(JsonConvert), "SerializeObject", typeof(object))]
		public static object SerializeOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object _, object[] args)
		{
			object obj = args[0];

			// Check if obj can be serialized normally
			if (!obj.IsCLRInstance())
			{
				return JsonConvert.SerializeObject(obj);
			}

			CLRInstance clrInstance = obj as CLRInstance;

			// Pull values from the CLR instance
			FieldInfo[] fieldInfos = clrInstance.Type.GetFields(BINDING_FLAGS);
			PropertyInfo[] propertyInfos = clrInstance.Type.GetProperties(BINDING_FLAGS);

			Dictionary<string, object> result = new Dictionary<string, object>(fieldInfos.Length + propertyInfos.Length);

			foreach (var field in fieldInfos)
			{
				result[field.Name] = field.GetValue(clrInstance);
			}

			foreach (var property in propertyInfos)
			{
				result[property.Name] = property.GetValue(clrInstance);
			}

			return JsonConvert.SerializeObject(result);
		}


		[Preserve]
		[CLRGenericMethodBinding(typeof(JsonConvert), "DeserializeObject", 1, typeof(string))]
		public static object DeserializeOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object _, object[] args, Type[] genericTypes)
		{
			Type type = genericTypes[0];
			string json = (string)args[0];

			// Check if type can be deserialized normally
			if (!type.IsCLRType())
			{
				return JsonConvert.DeserializeObject(json, type);
			}

			CLRType clrType = type.GetCLRType();
			CLRInstance clrInstance = (CLRInstance)clrType.AppDomain.CreateInstance(clrType);

			FieldInfo[] fieldInfos = clrType.GetFields(BINDING_FLAGS);
			PropertyInfo[] propertyInfos = clrType.GetProperties(BINDING_FLAGS);

			var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

			// Apply deserialized values accordingly
			foreach (var field in fieldInfos)
			{
				object value = result[field.Name];

				if (value is JArray array)
				{
					value = array.ToObject(field.FieldType);
				}
				else if (value is double doubleValue)
				{
					// Floats are serialized as a double
					if (field.FieldType == typeof(float))
					{
						value = (float)doubleValue;
					}
				}

				field.SetValue(clrInstance, value);
			}

			foreach (var property in propertyInfos)
			{
				object value = result[property.Name];

				if (value is JArray array)
				{
					value = array.ToObject(property.PropertyType);
				}
				else if (value is double doubleValue)
				{
					// Floats are serialized as a double
					if (property.PropertyType == typeof(float))
					{
						value = (float)doubleValue;
					}
				}

				property.SetValue(clrInstance, value);
			}

			return clrInstance;
		}
	}
}
#endif