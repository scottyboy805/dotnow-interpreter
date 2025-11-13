#if ROSLYNCSHARP
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using AppDomain = dotnow.AppDomain;

namespace RoslynCSharp
{
    internal class InterpretedScriptType : ScriptType
    {
        // Private
        private static readonly MethodInfo addComponentMethod = typeof(GameObject)
            .GetMethod(nameof(GameObject.AddComponent), new Type[] { typeof(Type) });
        private static readonly MethodInfo createInstanceMethod = typeof(ScriptableObject)
            .GetMethod(nameof(ScriptableObject.CreateInstance), new Type[] { typeof(Type) });

        private ScriptAssembly assembly = null;
        private ScriptType parent = null;
        private ScriptType[] nestedTypes = null;
        private Type clrType = null;

        private IScriptDataProxy fields = null;
        private IScriptDataProxy safeFields = null;
        private IScriptDataProxy properties = null;
        private IScriptDataProxy safeProperties = null;

        // Properties
        public override ScriptAssembly Assembly
        {
            get { return assembly; }
        }

        public override ScriptType Parent
        {
            get { return parent; }
        }

        public override Type SystemType
        {
            get { return clrType; }
        }

        public override bool IsNestedType
        {
            get { return parent != null; }
        }

        public override bool HasNestedTypes
        {
            get { return nestedTypes != null && nestedTypes.Length > 0; }
        }

        public override ScriptType[] NestedTypes
        {
            get { return nestedTypes; }
        }

        public override ICollection<object> CustomAttributes
        {
            get { throw new NotSupportedException(); }
        }

        public override IScriptDataProxy FieldsStatic
        {
            get
            {
                if (fields == null)
                    fields = new InterpretedScriptFieldDataProxy(this, null, true, true);

                return fields;
            }
        }

        public override IScriptDataProxy SafeFieldsStatic
        {
            get
            {
                if (safeFields == null)
                    safeFields = new InterpretedScriptFieldDataProxy(this, null, true, false);

                return safeFields;
            }
        }

        public override IScriptDataProxy PropertiesStatic
        {
            get
            {
                if (properties == null)
                    properties = new InterpretedScriptPropertyDataProxy(this, null, true, true);

                return properties;
            }
        }

        public override IScriptDataProxy SafePropertiesStatic
        {
            get
            {
                if (safeProperties == null)
                    safeProperties = new InterpretedScriptPropertyDataProxy(this, null, false, false);

                return safeProperties;
            }
        }

        public override IScriptEventProxy EventsStatic
        {
            get { throw new NotSupportedException("Events are not yet supported in interpreter mode"); }
        }

        public override IScriptEventProxy SafeEventsStatic
        {
            get { throw new NotSupportedException("Events are not yet supported in interpreter mode"); }
        }

        // Construction
        protected override void ConstructInstance(ScriptAssembly assembly, ScriptType parent, ScriptType[] nestedTypes, Type systemType)
        {
            this.assembly = assembly;
            this.parent = parent;
            this.nestedTypes = nestedTypes;
            this.clrType = systemType;
        }

        // Methods
        public override bool IsSubTypeOf(Type baseClass)
        {
            return clrType.IsSubclassOf(baseClass);
        }

        protected override ScriptProxy CreateMonoBehaviourInstanceImpl(GameObject parent)
        {
            // Get the interpret domain
            AppDomain appDomain = Assembly.Domain.GetAppDomain();

            // Get the add component method override
            MethodBase addComponentOverride = appDomain.GetOverrideMethodBinding(addComponentMethod);

            // Call add component override
            object instance = addComponentOverride.Invoke(parent, new object[] { clrType });

            // Create proxy
            if (instance != null)
                return ScriptProxy.CreateScriptProxy<InterpretedScriptProxy>(this, instance);

            // Error adding component
            return null;
        }

        protected override ScriptProxy CreateScriptableObjectInstanceImpl()
        {
            // Get the interpret domain
            AppDomain appDomain = Assembly.Domain.GetAppDomain();

            // Get the create instance method override
            MethodBase createInstanceOverride = appDomain.GetOverrideMethodBinding(createInstanceMethod);

            // Call create instance override
            object instance = createInstanceOverride.Invoke(parent, new object[] { clrType });

            // Create proxy
            if (instance != null)
                return ScriptProxy.CreateScriptProxy<InterpretedScriptProxy>(this, instance);

            // Error creating instance
            return null;
        }

        protected override ScriptProxy CreateInstanceImpl(object[] args)
        {
            // Get the interpret domain
            AppDomain appDomain = Assembly.Domain.GetAppDomain();

            // Create instance of type
            object instance = appDomain.CreateInstance(clrType, args);

            // Create proxy
            if (instance != null)
                return ScriptProxy.CreateScriptProxy<InterpretedScriptProxy>(this, instance);

            // Error creating instance
            return null;
        }

        protected override FieldInfo FindFieldImpl(string name, BindingFlags bindingAttrib)
        {
            return clrType.GetField(name, bindingAttrib);
        }

        protected override PropertyInfo FindPropertyImpl(string name, BindingFlags bindingAttib)
        {
            return clrType.GetProperty(name, bindingAttib);
        }

        protected override MethodInfo FindMethodImpl(string name, BindingFlags bindingAttrib)
        {
            return clrType.GetMethod(name, bindingAttrib);
        }

        protected override EventInfo FindEventImpl(string name, BindingFlags bindingAttrib)
        {
            return clrType.GetEvent(name, bindingAttrib);
        }
    }
}
#endif