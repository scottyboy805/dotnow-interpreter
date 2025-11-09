using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnow.Interop.CoreLib
{
    internal sealed class System_Object
    {
        // Methods
        [Preserve]
        [CLRMethodBinding(typeof(object), nameof(object.GetType))]
        public static void GetType_Override(StackContext context)
        {
            // Get the instance
            object inst = context.ReadArgObject<object>(0);

            // Get the interpreted type
            Type instanceType = inst.GetInterpretedType();

            // Return the type
            context.ReturnObject(instanceType);
        }
    }
}
