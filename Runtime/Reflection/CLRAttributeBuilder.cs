using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace dotnow.Reflection
{
    internal class CLRAttributeBuilder
    {
        // Private
        private AppDomain domain = null;
        private ICollection<CustomAttribute> attributes = null;
        private List<Attribute> attributeInstances = null;

        // Constructor
        public CLRAttributeBuilder(AppDomain domain, ICollection<CustomAttribute> attributes)
        {
            this.domain = domain;
            this.attributes = attributes;
        }

        // Methods
        public Attribute[] GetAttributeInstances()
        {
            // Create attributes
            BuildAttributeInstances();

            // Get all attributes
            return attributeInstances.ToArray();
        }

        public Attribute[] GetAttributeInstancesOfType(Type baseType)
        {
            // Create attributes
            BuildAttributeInstances();

            // Get cound
            List<Attribute> result = new List<Attribute>();

            foreach(Attribute attributeInstance in attributeInstances)
            {
                Type attribType = attributeInstance.GetInterpretedType();

                if (attribType == baseType || attribType.IsSubclassOf(baseType) == true)
                    result.Add(attributeInstance);
            }

            return result.ToArray();
        }

        public bool IsDefined(Type attributeType)
        {
            // Create attributes
            BuildAttributeInstances();

            foreach(object attributeInstance in attributeInstances)
            {
                // Get the type
                Type type = attributeInstance.GetInterpretedType();

                if (type == attributeType)
                    return true;
            }
            return false;
        }

        private void BuildAttributeInstances()
        {
            if(attributeInstances == null)
            {
                attributeInstances = new List<Attribute>();

                // Process all attributes
                foreach(CustomAttribute attribute in attributes)
                {
                    // Resolve type and constructor
                    Type attributeType = domain.ResolveType(attribute.AttributeType);
                    MethodBase ctor = domain.ResolveConstructor(attribute.Constructor);

                    // Create argument list
                    object[] args = new object[attribute.ConstructorArguments.Count];

                    for(int i = 0; i < args.Length; i++)
                    {
                        args[i] = attribute.ConstructorArguments[i].Value;
                    }

                    // Get as base attribute
                    object attrib = domain.CreateInstance(attributeType, ctor, args).UnwrapAs<Attribute>();

                    // Create instance of attribute
                    attributeInstances.Add(attrib as Attribute);
                }
            }
        }
    }
}
