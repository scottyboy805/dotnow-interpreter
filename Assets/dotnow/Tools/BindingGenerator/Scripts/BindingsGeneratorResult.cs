
#if UNITY_EDITOR && UNITY_DISABLE == false
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace dotnow.BindingGenerator
{
    public sealed class BindingsGeneratorResult
    {
        // Private
        private string generateInFolder = "";
        private List<string> generatedSourceFiles = new List<string>();
        private string errorMessage = "";

        // Public
        public CodeGeneratorOptions codeOptions = new CodeGeneratorOptions
        {
            BracingStyle = "C",
            BlankLinesBetweenMembers = false,
        };

        // Properties
        public string GenerateInFolder
        {
            get { return generateInFolder; }
        }

        public IList<string> GeneratedSourceFiles
        {
            get { return generatedSourceFiles; }
        }

        public int GeneratedSourceFileCount
        {
            get { return generatedSourceFiles.Count; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
        }

        // Constructor
        public BindingsGeneratorResult(string generateInFolder)
        {
            this.generateInFolder = generateInFolder;
        }

        // Methods
        internal void AddGeneratedSourceFile(string generatedSourceFile)
        {
            // Check for error
            if (string.IsNullOrEmpty(generatedSourceFile) == true) throw new ArgumentException(nameof(generatedSourceFile) + " cannot be null or empty");

            if(generatedSourceFiles.Contains(generatedSourceFile) == false)
                generatedSourceFiles.Add(generatedSourceFile);
        }

        internal void SetError(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }
    }
}
#endif
