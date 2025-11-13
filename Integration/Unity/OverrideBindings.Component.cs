//#if !UNITY_DISABLE
//#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_WSA || UNITY_WEBGL || UNITY_SWITCH)
//using dotnow;
//using dotnow.Interop;
//using System;
//using System.Collections.Generic;
//using System.Reflection;

//namespace UnityEngine
//{
//	public static partial class OverrideBindings
//	{
//		private static readonly List<ICLRProxy> getComponentsNonAlloc = new List<ICLRProxy>();

//		#region GetComponent
//		[Preserve]
//		[CLRGenericMethodBinding(typeof(Component), "GetComponent", 1)]
//		public static object UnityEngine_Component_GetComponentTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
//			=> UnityEngine_Component_GetComponentOverride(domain, overrideMethod, instance, new object[1] { genericTypes[0] });

//		[Preserve]
//		[CLRMethodBinding(typeof(Component), "GetComponent", typeof(Type))]
//		public static object UnityEngine_Component_GetComponentOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//		{
//			// Get component
//			Component comp = instance as Component;

//			// Get argument
//			Type componentType = args[0] as Type;

//			// Check for clr type
//			if (componentType.IsCLRType() == false)
//			{
//				return comp.GetComponent(componentType);
//			}

//			// Get all proxies
//			getComponentsNonAlloc.Clear();
//			comp.GetComponents<ICLRProxy>(getComponentsNonAlloc);

//			// Check for matching instance
//			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
//			{
//				// Check for matching type then return the clr instance
//				if (getComponentsNonAlloc[i].Instance.Type == componentType)
//					return getComponentsNonAlloc[i].Instance;
//			}

//			// No component found
//			return null;
//		}
//		#endregion

//		#region TryGetComponent
//		[Preserve]
//		[CLRGenericMethodBinding(typeof(Component), "TryGetComponent", 1, typeof(MakeByRef<GenericType>))]
//		public static object UnityEngine_Component_TryGetComponentTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
//			=> UnityEngine_Component_TryGetComponent(instance, genericTypes[0], ref args[0]);

//		[Preserve]
//		[CLRMethodBinding(typeof(Component), "TryGetComponent", typeof(Type), typeof(MakeByRef<Component>))]
//		public static object UnityEngine_Component_TryGetComponentOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//			=> UnityEngine_Component_TryGetComponent(instance, (Type)args[0], ref args[1]);


//		private static object UnityEngine_Component_TryGetComponent(object instance, Type componentType, ref object _out)
//		{
//			// Get object
//			Component comp = instance as Component;

//			// Check for clr type
//			if (componentType.IsCLRType() == false)
//			{
//				bool result = comp.TryGetComponent(componentType, out Component component);
//				_out = component;
//				return result;
//			}

//			// Get all proxies
//			getComponentsNonAlloc.Clear();
//			comp.GetComponents<ICLRProxy>(getComponentsNonAlloc);

//			// Check for matching instance
//			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
//			{
//				// Check for matching type then return the clr instance
//				if (getComponentsNonAlloc[i].Instance.Type == componentType)
//				{
//					_out = getComponentsNonAlloc[i].Instance;
//					return true;
//				}
//			}

//			// No component found
//			return false;
//		}
//		#endregion

//		#region GetComponents
//		[Preserve]
//		[CLRGenericMethodBinding(typeof(Component), "GetComponents", 1)]
//		public static object UnityEngine_Component_GetComponentsTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
//			=> UnityEngine_Component_GetComponentsOverride(domain, overrideMethod, instance, new object[1] { genericTypes[0] });

//		[Preserve]
//		[CLRMethodBinding(typeof(Component), "GetComponents", typeof(Type))]
//		public static object UnityEngine_Component_GetComponentsOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//		{
//			// Get component
//			Component comp = instance as Component;

//			// Get argument
//			Type componentType = args[0] as Type;

//			// Check for clr type
//			if (componentType.IsCLRType() == false)
//			{
//				return comp.GetComponents(componentType);
//			}

//			// Get all proxies
//			getComponentsNonAlloc.Clear();
//			comp.GetComponents<ICLRProxy>(getComponentsNonAlloc);

//			// Get results collection
//			List<CLRInstance> results = new List<CLRInstance>(getComponentsNonAlloc.Count);

//			// Check for matching instance
//			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
//			{
//				// Check for matching type then return the clr instance
//				if (getComponentsNonAlloc[i].Instance.Type == componentType)
//				{
//					results.Add(getComponentsNonAlloc[i].Instance);
//				}
//			}

//			// No component found
//			return results.ToArray();

//		}
//		#endregion

//		#region GetComponentInChildren
//		[Preserve]
//		[CLRGenericMethodBinding(typeof(Component), "GetComponentInChildren", 1)]
//		public static object UnityEngine_Component_GetComponentInChildrenTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
//			=> Component_GetComponentInChildren(instance, genericTypes[0], false);

//		[Preserve]
//		[CLRMethodBinding(typeof(Component), "GetComponentInChildren", typeof(Type))]
//		public static object UnityEngine_Component_GetComponentInChildrenOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//			=> Component_GetComponentInChildren(instance, (Type)args[0], false);


//		[Preserve]
//		[CLRGenericMethodBinding(typeof(Component), "GetComponentInChildren", 1, typeof(bool))]
//		public static object UnityEngine_Component_GetComponentInChildren_Bool_TOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
//			=> Component_GetComponentInChildren(instance, genericTypes[0], (bool)args[0]);

//		[Preserve]
//		[CLRMethodBinding(typeof(Component), "GetComponentInChildren", typeof(Type), typeof(bool))]
//		public static object UnityEngine_Component_GetComponentInChildren_Bool_Override(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//			=> Component_GetComponentInChildren(instance, (Type)args[0], (bool)args[1]);


//		public static object Component_GetComponentInChildren(object instance, Type componentType, bool includeInactive)
//		{
//			// Get component
//			Component comp = instance as Component;

//			// Check for clr type
//			if (componentType.IsCLRType() == false)
//			{
//				return comp.GetComponentInChildren(componentType, includeInactive);
//			}

//			// Get all proxies
//			getComponentsNonAlloc.Clear();
//			comp.GetComponentsInChildren<ICLRProxy>(includeInactive, getComponentsNonAlloc);

//			// Check for matching instance
//			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
//			{
//				// Check for matching type then return the clr instance
//				if (getComponentsNonAlloc[i].Instance.Type == componentType)
//					return getComponentsNonAlloc[i].Instance;
//			}

//			// No component found
//			return null;
//		}
//		#endregion

//		#region GetComponentsInChildren
//		[Preserve]
//		[CLRGenericMethodBinding(typeof(Component), "GetComponentsInChildren", 1)]
//		public static object UnityEngine_Component_GetComponentsInChildrenTOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
//			=> Component_GetComponentsInChildren(instance, genericTypes[0], false);

//		[Preserve]
//		[CLRMethodBinding(typeof(Component), "GetComponentsInChildren", typeof(Type))]
//		public static object UnityEngine_Component_GetComponentsInChildrenOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//			=> Component_GetComponentsInChildren(instance, (Type)args[0], false);


//		[Preserve]
//		[CLRGenericMethodBinding(typeof(Component), "GetComponentsInChildren", 1, typeof(bool))]
//		public static object UnityEngine_Component_GetComponentsInChildren_Bool_TOverride(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args, Type[] genericTypes)
//			=> Component_GetComponentsInChildren(instance, genericTypes[0], (bool)args[0]);

//		[Preserve]
//		[CLRMethodBinding(typeof(Component), "GetComponentsInChildren", typeof(Type), typeof(bool))]
//		public static object UnityEngine_Component_GetComponentsInChildren_Bool_Override(dotnow.AppDomain domain, MethodInfo overrideMethod, object instance, object[] args)
//			=> Component_GetComponentsInChildren(instance, (Type)args[0], (bool)args[1]);


//		private static object Component_GetComponentsInChildren(object instance, Type componentType, bool includeInactive)
//		{
//			// Get component
//			Component comp = instance as Component;

//			// Check for clr type
//			if (componentType.IsCLRType() == false)
//			{
//				return comp.GetComponentsInChildren(componentType, includeInactive);
//			}

//			// Get all proxies
//			getComponentsNonAlloc.Clear();
//			comp.GetComponentsInChildren<ICLRProxy>(includeInactive, getComponentsNonAlloc);

//			// Get results collection
//			List<CLRInstance> results = new List<CLRInstance>(getComponentsNonAlloc.Count);

//			// Check for matching instance
//			for (int i = 0; i < getComponentsNonAlloc.Count; i++)
//			{
//				// Check for matching type then return the clr instance
//				if (getComponentsNonAlloc[i].Instance.Type == componentType)
//				{
//					ICLRProxy proxy = getComponentsNonAlloc[i].Instance.Unwrap() as ICLRProxy;
//					results.Add(proxy.Instance);
//				}
//			}

//			// No component found
//			return results.ToArray();
//		}
//		#endregion
//	}
//}
//#endif
//#endif