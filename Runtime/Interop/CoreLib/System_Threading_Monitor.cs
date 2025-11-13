using System.Threading;

namespace dotnow.Interop.CoreLib
{
    internal static class System_Threading_Monitor
    {
        // Methods
        [Preserve]
        [CLRMethodBinding(typeof(Monitor), nameof(Monitor.Enter), typeof(object))]
        public static void MonitorEnter_O_Override(StackContext context)
        {
            // Enter monitor
            Monitor.Enter(context.ReadArgObject<object>(0));
        }

        [Preserve]
        [CLRMethodBinding(typeof(Monitor), nameof(Monitor.Enter), typeof(object), typeof(bool))]
        public static void MonitorEnter_OB_Override(StackContext context)
        {
            // Get ref bool
            bool refBool = context.ReadArgValueType<bool>(1);

            // Enter monitor
            Monitor.Enter(context.ReadArgObject<object>(0), ref refBool);

            // Push bool back
            context.WriteArgValueType<bool>(1, refBool);
        }

        [Preserve]
        [CLRMethodBinding(typeof(Monitor), nameof(Monitor.Exit), typeof(object))]
        public static void MonitorExit_Override(StackContext context)
        {
            // Exit monitor
            Monitor.Exit(context.ReadArgObject<object>(0));
        }
    }
}
