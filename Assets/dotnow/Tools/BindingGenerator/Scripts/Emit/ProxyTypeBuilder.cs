#if !UNITY_DISABLE
#if UNITY_EDITOR 
using dotnow.Interop;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace dotnow.BindingGenerator.Emit
{
    internal sealed class ProxyTypeBuilder
    {
        // Private
        private Type type = null;

        // Public
        public const string appDomainVar = "appDomain";
        public const string instanceVar = "instance";

        // Constructor
        public ProxyTypeBuilder(Type type)
        {
            this.type = type;
        }

        // Methods
        public CodeTypeDeclaration BuildTypeProxy()
        {
            
            CodeTypeDeclaration codeType = new CodeTypeDeclaration(GetSafeTypeName(type) + "_ProxyBinding");
            codeType.Comments.Add(new CodeCommentStatement("Generated from type: " + type.AssemblyQualifiedName));
            codeType.Comments.Add(new CodeCommentStatement("Generated from assembly: " + type.Assembly.Location));
            codeType.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            // Add generated attribute
            codeType.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(GeneratedAttribute))));

            // Create base type
            codeType.BaseTypes.Add(new CodeTypeReference(type));
            codeType.BaseTypes.Add(new CodeTypeReference(typeof(ICLRProxy)));

            // Create attribute
            codeType.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(CLRProxyBindingAttribute)),
                new CodeAttributeArgument(new CodeTypeOfExpression(new CodeTypeReference(type)))));

            // Create app domain field
            codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(AppDomain)), appDomainVar));

            // Create instance field
            codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(CLRInstance)), instanceVar));

            CodeMemberProperty instanceProperty = BuildCLRProxyImplementation_Instance();
            CodeMemberMethod initializeMethod = BuildCLRProxyImplementation_InitializeProxy();

            // Implement interface
            codeType.Members.Add(instanceProperty);
            codeType.Members.Add(initializeMethod);

            ProcessConstructors(codeType);

            // Implement methods
            if(type.IsInterface == true)
            {

            }
            else
            {
                int memberIndex = 0;

                HashSet<string> definedMethodNames = new HashSet<string>();

                // Process all methods
                foreach(MethodInfo method in type.GetMethods())
                {
                    // Skip object methods
                    if (method.DeclaringType == typeof(object) || method.DeclaringType == typeof(MarshalByRefObject))
                        continue;

                    // Check for property - these will be handled by ProxyPropertyBuilder
                    if (method.IsSpecialName == true)
                        continue;

                    // Check for already added
                    if (definedMethodNames.Contains(method.ToString()) == true)
                        continue;

                    if(method.IsVirtual == true || method.IsAbstract == true)
                    {
                        ProxyMethodBuilder methodBuilder = new ProxyMethodBuilder(method, false, memberIndex++);

                        // Build field
                        codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(MethodBase)), methodBuilder.VariableName));

                        // Build method
                        codeType.Members.Add(methodBuilder.BuildMethodProxy());

                        definedMethodNames.Add(method.Name);
                    }
                }

                // Process all monobehaviour events
                if (typeof(MonoBehaviour).IsAssignableFrom(type) == true)
                {
                    foreach(MethodInfo monoBehaviourMethod in typeof(Template.MonoBehaviourTemplate).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        ProxyMethodBuilder methodBuilder = new ProxyMethodBuilder(monoBehaviourMethod, false, memberIndex++);

                        // Build field
                        codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(MethodBase)), methodBuilder.VariableName));

                        // Build method
                        codeType.Members.Add(methodBuilder.BuildMethodProxy());
                    }
                }

                // Process all properties
                foreach (PropertyInfo property in type.GetProperties())
                {
                    // Skip object methods
                    if (property.DeclaringType == typeof(object) || property.DeclaringType == typeof(MarshalByRefObject) || property.DeclaringType != property.ReflectedType)
                        continue;

                    
                    ProxyPropertyBuilder propertyBuilder = new ProxyPropertyBuilder(property, false, memberIndex++);

                    // Build fields
                    if(propertyBuilder.GenerateGetter == true)
                        codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(MethodBase)), propertyBuilder.VariableNameGetter));

                    if (propertyBuilder.GenerateSetter == true)
                        codeType.Members.Add(new CodeMemberField(new CodeTypeReference(typeof(MethodBase)), propertyBuilder.VariableNameSetter));

                    // Build property
                    if(propertyBuilder.GenerateGetterOrSetter == true)
                        codeType.Members.Add(propertyBuilder.BuildPropertyProxy());
                }
            }

            return codeType;
        }
        

        private void ProcessConstructors(CodeTypeDeclaration codeType)
        {
            foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                ProxyConstructorBuilder constructorBuilder = new ProxyConstructorBuilder(constructor, type);
                CodeConstructor proxyConstructor = constructorBuilder.BuildConstructorProxy();
                codeType.Members.Add(proxyConstructor);
            }
        }
        private bool ShouldProcessConstructor(ConstructorInfo constructor)
        {
            return !constructor.IsStatic && constructor.DeclaringType != typeof(object);
        }
        public string GetTypeFlattenedName()
        {
            // Check for namespace
            if (string.IsNullOrEmpty(type.Namespace) == true)
                return type.Name;

            return string.Concat(type.Namespace.Replace('.', '_'), "_", type.Name);
        }

        private CodeMemberProperty BuildCLRProxyImplementation_Instance()
        {
            // Create the property
            CodeMemberProperty codeProperty = new CodeMemberProperty();
            codeProperty.Name = nameof(ICLRProxy.Instance);
            codeProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            // Type
            codeProperty.Type = new CodeTypeReference(typeof(CLRInstance));

            // Generate getter
            CodeMemberMethod getter = new CodeMemberMethod();

            // Emit getter body
            getter.Statements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), instanceVar)));

            // Add getter
            codeProperty.GetStatements.AddRange(getter.Statements);

            return codeProperty;
        }

        private CodeMemberMethod BuildCLRProxyImplementation_InitializeProxy()
        {
            // Create the method
            CodeMemberMethod codeMethod = new CodeMemberMethod();
            codeMethod.Name = nameof(ICLRProxy.InitializeProxy);
            codeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            // Get target method
            MethodInfo implementMethod = typeof(ICLRProxy).GetMethod(nameof(ICLRProxy.InitializeProxy), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            ParameterInfo[] implementMethodParameters = implementMethod.GetParameters();

            // Create parameters
            foreach(ParameterInfo parameter in implementMethodParameters)
            {
                codeMethod.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(parameter.ParameterType), parameter.Name));
            }

            // Create assign app domain
            codeMethod.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), appDomainVar),
                new CodeVariableReferenceExpression(implementMethodParameters[0].Name)));

            // Create assign instance
            codeMethod.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), instanceVar),
                new CodeVariableReferenceExpression(implementMethodParameters[1].Name)));

            return codeMethod;
        }
        
        private string GetSafeTypeName(Type type)
        {
            string name = type.IsNested 
                ? $"{GetSafeTypeName(type.DeclaringType)}_{type.Name}"
                : type.Name;

            if (type.IsGenericType)
            {
                name = name.Split('`')[0];
                name += $"_{type.GetGenericArguments().Length}";
            }

            return name;
        }
    }
}
#endif
#endif