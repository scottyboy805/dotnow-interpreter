using System;

namespace dotnow.Runtime
{
    public class StackLocal
    {
        // Public
        public Type localType = null;
        public StackData defaultValue = default;

        // Internal
        internal bool isCLRValueType = false;
        internal int clrValueTypeSize = 0;

        // Constructor
        public StackLocal(AppDomain domain, Type localType)
        {
            this.localType = localType;

            // Check for clr type
            if(localType.IsCLRType() == true && localType.IsValueType == true && localType.IsEnum == false)
            {
                // Get the clr type
                CLRType type = localType.GetCLRType();

                // Set value type flag
                isCLRValueType = true;
                clrValueTypeSize = type.SizeOfInstance();
            }
            else
            {
                // Get default value
                StackData.AllocTypedSlow(__heapallocator.GetCurrent(), ref defaultValue, localType, localType.GetDefaultValue(domain));
            }
        }
    }
}
