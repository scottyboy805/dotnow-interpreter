#if ROSLYNCSHARP
using System;
using TrivialCLR;
using UnityEngine;

namespace RoslynCSharp
{
    internal class TrivialCLRScriptProxy : ScriptProxy
    {
        // Private
        private ScriptType type = null;
        private object instance = null;

        private IScriptDataProxy fields = null;
        private IScriptDataProxy safeFields = null;
        private IScriptDataProxy properties = null;
        private IScriptDataProxy safeProperties = null;

        // Properties
        public override ScriptAssembly Assembly
        {
            get { return type.Assembly; }
        }

        public override ScriptType ScriptType
        {
            get { return type; }
        }

        public override object Instance
        {
            get { return instance.Unwrap(); }
        }

        public override bool IsDisposed
        {
            get { return instance != null; }
        }

        public override IScriptDataProxy Fields
        {
            get
            {
                if (fields == null)
                    fields = new TrivialCLRScriptFieldDataProxy(type, this, false, true);

                return fields;
            }
        }

        public override IScriptDataProxy SafeFields
        {
            get
            {
                if (safeFields == null)
                    safeFields = new TrivialCLRScriptFieldDataProxy(type, this, false, false);

                return safeFields;
            }
        }

        public override IScriptDataProxy Properties
        {
            get
            {
                if (properties == null)
                    properties = new TrivialCLRScriptPropertyDataProxy(type, this, false, true);

                return properties;
            }
        }

        public override IScriptDataProxy SafeProperties
        {
            get
            {
                if (safeProperties == null)
                    safeProperties = new TrivialCLRScriptPropertyDataProxy(type, this, false, false);

                return safeProperties;
            }
        }

        public override IScriptEventProxy Events
        {
            get { throw new NotSupportedException("Events are not yet supported in interpreter mode"); }
        }

        public override IScriptEventProxy SafeEvents
        {
            get { throw new NotSupportedException("Events are not yet supported in interpreter mode"); }
        }

        // Construction
        protected override void ConstructInstance(ScriptType type, object instance)
        {
            this.type = type;
            this.instance = instance;
        }

        // Methods
        public override T GetInstanceAs<T>(bool throwOnError, T errorValue = default)
        {
            // Try a direct cast
            if (throwOnError == true)
                return (T)instance.UnwrapAs(typeof(T));

            try
            {
                // Try to cast and catch any InvalidCast exceptions.
                T result = (T)instance.UnwrapAs(typeof(T));

                // Return the result
                return result;
            }
            catch
            {
                // Error value
                return errorValue;
            }
        }

        public override void Dispose()
        {
            // Make sure the object has not already been disposed
            CheckDisposed();

            // Check for Unity object
            if (IsUnityObject == true)
            {
                if (Application.isPlaying == true)
                    UnityEngine.Object.Destroy(UnityInstance);
                else
                    UnityEngine.Object.DestroyImmediate(UnityInstance, false);
            }

            // Call the dispose method correctly
            if (instance is IDisposable)
                (instance as IDisposable).Dispose();

            // Unset reference
            type = null;
            instance = null;
        }
    }
}
#endif