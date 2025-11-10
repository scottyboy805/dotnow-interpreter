using System;
using System.Reflection;

namespace dotnow.Interop.CoreLib.Proxy
{
    [Preserve]
    [CLRProxyBinding(typeof(Exception))]
    internal sealed class System_Exception_Proxy : Exception, ICLRProxy
    {
        // Private
        private ICLRInstance instance;

        private MethodInfo getMessageOverride;
        private MethodInfo getStackTraceOverride;
        private MethodInfo getSourceOverride;
        private MethodInfo setSourceOverride;

        // Properties
        public ICLRInstance Instance => instance;

        public override string Message
        {
            get
            {
                // Call override
                if (getMessageOverride != null)
                    return (string)getMessageOverride.Invoke(instance, null);

                // Fallback to base
                return base.Message;
            }
        }

        public override string StackTrace
        {
            get
            {
                // Call override
                if(getStackTraceOverride != null)
                    return (string)getStackTraceOverride.Invoke(instance, null);

                // Fallback to base
                return base.StackTrace;
            }
        }

        public override string Source
        {
            get
            {
                // Call override
                if(getSourceOverride != null)
                    return (string)getSourceOverride.Invoke(instance, null);

                // Fallback to base
                return base.Source;
            }
            set
            {
                // Call override
                if (setSourceOverride != null)
                {
                    setSourceOverride.Invoke(instance, new[] { value });
                }
                // Fallback to base
                else
                {
                    base.Source = value;
                }
            }
        }

        // Methods
        public void Initialize(AppDomain appDomain, Type type, ICLRInstance instance)
        {
            this.instance = instance;

            getMessageOverride = type.GetMethod("get_Message", BindingFlags.Instance | BindingFlags.Public);
            getStackTraceOverride = type.GetMethod("get_StackTrace", BindingFlags.Instance | BindingFlags.Public);
            getSourceOverride = type.GetMethod("get_Source", BindingFlags.Instance | BindingFlags.Public);
            setSourceOverride = type.GetMethod("set_Source", BindingFlags.Instance | BindingFlags.Public);
        }


    }
}
