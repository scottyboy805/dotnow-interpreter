#if ROSLYNCSHARP_20
using System;
using System.Reflection;
using UnityEngine;
using AppDomain = dotnow.AppDomain;

namespace RoslynCSharp
{
    internal sealed class InterpretedScriptType : ScriptType
    {
        // Private
        private static readonly MethodInfo addComponentMethod = typeof(GameObject)
            .GetMethod(nameof(GameObject.AddComponent), new Type[] { typeof(Type) });

        private readonly Type clrType;

        // Properties
        public override Type SystemType => clrType;

        // Constructor
        public InterpretedScriptType(ScriptAssembly assembly, ScriptType parent, Type clrType)
            : base(assembly, parent)
        {
            this.clrType = clrType;
        }

        // Methods
        protected override ScriptProxy CreateInstanceComponentImpl(GameObject parent)
        {
            // Get the interpret domain
            AppDomain appDomain = Assembly.Domain.GetAppDomain();

            // Get the add component method override
            MethodBase addComponentOverride = appDomain.GetOverrideMethodBinding(addComponentMethod);

            // Call add component override
            object instance = addComponentOverride.Invoke(parent, new object[] { clrType });

            // Create proxy
            if (instance != null)
                return new InterpretedScriptProxy(this, instance);

            // Error adding component
            return null;
        }

        protected override ScriptProxy CreateInstanceScriptableObjectImpl()
        {
            throw new NotSupportedException();
        }

        protected override ScriptProxy CreateInstanceImpl(object[] args)
        {
            // Get the interpret domain
            AppDomain appDomain = Assembly.Domain.GetAppDomain();

            // Create instance of type
            object instance = appDomain.CreateInstance(clrType, args);

            // Create proxy
            if (instance != null)
                return new InterpretedScriptProxy(this, instance);

            // Error creating instance
            return null;
        }

        public override bool IsSubTypeOf<T>()
        {
            return clrType.IsSubclassOf(typeof(T));
        }

        public override bool IsSubTypeOf(Type subType)
        {
            return clrType.IsSubclassOf(subType);
        }
    }
}
#endif