#if ROSLYNCSHARP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dotnow;
using dotnow.Reflection;

namespace RoslynCSharp
{
    internal class InterpretedScriptFieldDataProxy : IScriptDataProxy
    {
        // Private
        private ScriptType scriptType = null;
        private ScriptProxy scriptProxy = null;
        private bool isStatic = false;
        private bool throwOnError = true;

        // Properties
        public object this[string name]
        {
            get { return GetValue(name); }
            set { SetValue(name, value); }
        }

        // Constructor
        public InterpretedScriptFieldDataProxy(ScriptType type, ScriptProxy proxy, bool isStatic, bool throwOnError)
        {
            this.scriptType = type;
            this.scriptProxy = proxy;
            this.isStatic = isStatic;
            this.throwOnError = throwOnError;
        }

        // Methods
        public virtual object GetValue(string name)
        {
            try
            {
                // Try to get a field with the specified name
                FieldInfo info = scriptType.FindCachedField(name, isStatic);

                // Check for field not found
                if (info == null)
                    throw new TargetException(string.Format("Type '{0}' does not define a field called '{1}'", scriptType, name));

                // Select the target instance
                object instance = (scriptProxy != null)
                    ? scriptProxy.Instance
                    : null;

                // Support interop
                if ((info is CLRField) == false && instance.IsCLRInstance() == true)
                    instance = instance.Unwrap();

                // Attempt to get the field value
                return info.GetValue(instance);
            }
            catch
            {
                if (throwOnError == true)
                    throw;
            }
            return null;
        }

        public void SetValue(string name, object value)
        {
            try
            {
                // Try to get a field with the specified name
                FieldInfo info = scriptType.FindCachedField(name, isStatic);

                // Check for field not found
                if (info == null)
                    throw new TargetException(string.Format("Type '{0}' does not define a field called '{1}'", scriptType, name));

                // Select the target instance
                object instance = (scriptProxy != null)
                    ? scriptProxy.Instance
                    : null;

                // Support interop
                if ((info is CLRField) == false && instance.IsCLRInstance() == true)
                    instance = instance.Unwrap();

                // Check assignable
                if (TypeExtensions.IsFieldAssignableSlow(info, value) == false)
                    throw new TargetException(string.Format("Cannot assign value `{0}` to field `{1}`. Incompatible type!", value, this));

                // Attempt to set the value
                info.SetValue(instance, value);
            }
            catch
            {
                // Check for safe handling
                if (throwOnError == true)
                    throw;
            }
        }
    }
}
#endif