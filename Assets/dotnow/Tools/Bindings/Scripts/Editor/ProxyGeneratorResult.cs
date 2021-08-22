using System;
using System.Collections.Generic;

namespace dotnow.ProxyTool
{
    public sealed class ProxyGeneratorResult
    {
        // Private
        private string generateInFolder = "";
        private List<string> generatedSourceFiles = new List<string>();
        private string errorMessage = "";

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
        public ProxyGeneratorResult(string generateInFolder)
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
