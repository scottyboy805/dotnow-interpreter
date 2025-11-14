
namespace dotnow.Interop.CoreLib
{
    internal static class System_String
    {
        // Methods
        [Preserve]
        [CLRFieldBinding(typeof(string), nameof(string.Empty), CLRFieldAccess.Read)]
        public static void Empty_Read(StackContext context)
        {
            // Push string empty
            context.ReturnObject(string.Empty);
        }

        [Preserve]
        [CLRMethodBinding(typeof(string), "get_Length")]
        public static void GetLength_Override(StackContext context)
        {
            // Get the string instance
            string str = context.ReadArgObject<string>(0);

            // Push length
            context.ReturnValueType(str.Length);
        }

        [Preserve]
        [CLRMethodBinding(typeof(string), nameof(string.Compare), typeof(string), typeof(string))]
        public static void Compare_Override(StackContext context)
        {
            // Get the string instances
            string a = context.ReadArgObject<string>(0);
            string b = context.ReadArgObject<string>(1);

            // Push the result of the compare
            context.ReturnValueType(string.Compare(a, b));
        }
    }
}
