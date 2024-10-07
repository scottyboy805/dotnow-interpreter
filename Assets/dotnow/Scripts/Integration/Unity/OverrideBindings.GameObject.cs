#if !UNITY_DISABLE
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
using dotnow;
using dotnow.Interop;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine
{
	public static partial class OverrideBindings
	{
		#region AddComponent
		[Preserve]
		[CLRGenericMethodBinding(typeof(GameObject), "AddComponent", 1)]
		public static object AddComponentTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
			=> AddComponentOverride(domain, overrideMethod, instance, new object[1] { genericTypes[0] });

		[Preserve]
		[CLRMethodBinding(typeof(GameObject), "AddComponent", typeof(Type))]
		public static object AddComponentOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
		{
			// Get instance
			GameObject go = instance as GameObject;

			// Get argument
			Type componentType = args[0] as Type;

			// Check for clr type
			if (componentType.IsCLRType() == false)
			{
				// Use default unity behaviour
				return go.AddComponent(componentType);
			}

			// Handle add component manually
			Type proxyType = domain.GetCLRProxyBindingForType(componentType.BaseType);

			// Validate type
			if (typeof(MonoBehaviour).IsAssignableFrom(proxyType) == false)
				throw new InvalidOperationException("A type deriving from mono behaviour must be provided");

			// Create proxy instance
			ICLRProxy proxyInstance = (ICLRProxy)go.AddComponent(proxyType);

			// Create clr instance
			return domain.CreateInstanceFromProxy(componentType, proxyInstance);
		}
		#endregion

		#region GetComponent
		[Preserve]
		[CLRGenericMethodBinding(typeof(GameObject), "GetComponent", 1)]
		public static object UnityEngine_GameObject_GetComponentTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
			=> UnityEngine_GameObject_GetComponentOverride(domain, overrideMethod, instance, new object[1] { genericTypes[0] });

		[Preserve]
		[CLRMethodBinding(typeof(GameObject), "GetComponent", typeof(Type))]
		public static object UnityEngine_GameObject_GetComponentOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
		{
			// Get object
			GameObject go = instance as GameObject;

			// Get argument
			Type componentType = args[0] as Type;

			// Check for clr type
			if (componentType.IsCLRType() == false)
			{
				return go.GetComponent(componentType);
			}

			// Get all proxies
			getComponentsNonAlloc.Clear();
			go.GetComponents<ICLRProxy>(getComponentsNonAlloc);

			// Check for matching instance
			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
			{
				// Check for matching type then return the clr instance
				if (getComponentsNonAlloc[i].Instance.Type == componentType)
					return getComponentsNonAlloc[i].Instance;
			}

			// No component found
			return null;
		}
		#endregion

		#region TryGetComponent
		[Preserve]
		[CLRGenericMethodBinding(typeof(GameObject), "TryGetComponent", 1, typeof(MakeByRef<GenericType>))]
		public static object UnityEngine_GameObject_TryGetComponentTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
			=> UnityEngine_GameObject_TryGetComponentOverride(instance, genericTypes[0], ref args[0]);

		[Preserve]
		[CLRMethodBinding(typeof(GameObject), "TryGetComponent", typeof(Type), typeof(MakeByRef<Component>))]
		public static object UnityEngine_GameObject_TryGetComponentOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
			=> UnityEngine_GameObject_TryGetComponentOverride(instance, (Type)args[0], ref args[1]);


		private static object UnityEngine_GameObject_TryGetComponentOverride(object instance, Type componentType, ref object _out)
		{
			// Get object
			GameObject go = instance as GameObject;

			// Check for clr type
			if (componentType.IsCLRType() == false)
			{
				bool result = go.TryGetComponent(componentType, out Component component);
				_out = component;
				return result;
			}

			// Get all proxies
			getComponentsNonAlloc.Clear();
			go.GetComponents<ICLRProxy>(getComponentsNonAlloc);

			// Check for matching instance
			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
			{
				// Check for matching type then return the clr instance
				if (getComponentsNonAlloc[i].Instance.Type == componentType)
				{
					_out = getComponentsNonAlloc[i].Instance;
					return true;
				}
			}

			// No component found
			return false;
		}
		#endregion

		#region GetComponents
		[Preserve]
		[CLRGenericMethodBinding(typeof(GameObject), "GetComponents", 1)]
		public static object UnityEngine_GameObject_GetComponentsTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
			=> UnityEngine_GameObject_GetComponentsOverride(domain, overrideMethod, instance, new object[1] { genericTypes[0] });

		[Preserve]
		[CLRMethodBinding(typeof(GameObject), "GetComponents", typeof(Type))]
		public static object UnityEngine_GameObject_GetComponentsOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
		{
			// Get object
			GameObject go = instance as GameObject;

			// Get argument
			Type componentType = args[0] as Type;

			// Check for clr type
			if (componentType.IsCLRType() == false)
			{
				return go.GetComponents(componentType);
			}

			// Get all proxies
			getComponentsNonAlloc.Clear();
			go.GetComponents<ICLRProxy>(getComponentsNonAlloc);

			// Get results collection
			List<CLRInstance> results = new List<CLRInstance>(getComponentsNonAlloc.Count);

			// Check for matching instance
			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
			{
				// Check for matching type then return the clr instance
				if (getComponentsNonAlloc[i].Instance.Type == componentType)
					results.Add(getComponentsNonAlloc[i].Instance);
			}

			// No component found
			return results.ToArray();
		}
		#endregion

		#region GetComponentsInChildren
		[Preserve]
		[CLRGenericMethodBinding(typeof(GameObject), "GetComponentsInChildren", 1)]
		public static object UnityEngine_GameObject_GetComponentsInChildrenTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
			=> GameObject_GetComponentsInChildren(instance, genericTypes[0], false);

		[Preserve]
		[CLRMethodBinding(typeof(GameObject), "GetComponentsInChildren", typeof(Type))]
		public static object UnityEngine_GameObject_GetComponentsInChildrenOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
			=> GameObject_GetComponentsInChildren(instance, (Type)args[0], false);


		[Preserve]
		[CLRGenericMethodBinding(typeof(GameObject), "GetComponentsInChildren", 1, typeof(bool))]
		public static object UnityEngine_GameObject_GetComponentsInChildren_Bool_TOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
			=> GameObject_GetComponentsInChildren(instance, genericTypes[0], (bool)args[0]);

		[Preserve]
		[CLRMethodBinding(typeof(GameObject), "GetComponentsInChildren", typeof(Type), typeof(bool))]
		public static object UnityEngine_GameObject_GetComponentsInChildren_Bool_Override(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
			=> GameObject_GetComponentsInChildren(instance, (Type)args[0], (bool)args[1]);


		private static object GameObject_GetComponentsInChildren(object instance, Type componentType, bool includeInactive)
		{
			// Get object
			GameObject go = instance as GameObject;

			// Check for clr type
			if (componentType.IsCLRType() == false)
			{
				return go.GetComponentsInChildren(componentType, includeInactive);
			}

			// Get all proxies
			getComponentsNonAlloc.Clear();
			go.GetComponentsInChildren(includeInactive, getComponentsNonAlloc);

			// Get results collection
			List<CLRInstance> results = new List<CLRInstance>(getComponentsNonAlloc.Count);

			// Check for matching instance
			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
			{
				// Check for matching type then return the clr instance
				if (getComponentsNonAlloc[i].Instance.Type == componentType)
				{
					ICLRProxy proxy = getComponentsNonAlloc[i].Instance.Unwrap() as ICLRProxy;
					results.Add(proxy.Instance);
				}
			}

			// No component found
			return results.ToArray();
		}
		#endregion

		#region GetComponentInChildren
		[Preserve]
		[CLRGenericMethodBinding(typeof(GameObject), "GetComponentInChildren", 1)]
		public static object UnityEngine_GameObject_GetComponentInChildrenTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
			=> GetComponentInChildren(instance, genericTypes[0], false);

		[Preserve]
		[CLRMethodBinding(typeof(GameObject), "GetComponentInChildren", typeof(Type))]
		public static object UnityEngine_GameObject_GetComponentInChildrenOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
			=> GetComponentInChildren(instance, (Type)args[0], false);


		[Preserve]
		[CLRGenericMethodBinding(typeof(GameObject), "GetComponentInChildren", 1, typeof(bool))]
		public static object UnityEngine_GameObject_GetComponentInChildren_Bool_TOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
			=> GetComponentInChildren(instance, genericTypes[0], (bool)args[0]);

		[Preserve]
		[CLRMethodBinding(typeof(GameObject), "GetComponentInChildren", typeof(Type), typeof(bool))]
		public static object UnityEngine_GameObject_GetComponentInChildren_Bool_Override(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
			=> GetComponentInChildren(instance, (Type)args[0], (bool)args[1]);


		private static object GetComponentInChildren(object instance, Type componentType, bool includeInactive)
		{
			// Get object
			GameObject go = instance as GameObject;

			// Check for clr type
			if (componentType.IsCLRType() == false)
			{
				return go.GetComponentInChildren(componentType, includeInactive);
			}

			// Get all proxies
			getComponentsNonAlloc.Clear();
			go.GetComponentsInChildren<ICLRProxy>(includeInactive, getComponentsNonAlloc);

			// Check for matching instance
			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
			{
				// Check for matching type then return the clr instance
				if (getComponentsNonAlloc[i].Instance.Type == componentType)
					return getComponentsNonAlloc[i].Instance;
			}

			// No component found
			return null;
		}
		#endregion
	}
}
#endif
#endif
