using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace dotnow.Reflection
{
    public struct MetadataSequencePoint
    {
        // Public
        public int Offset;
        public string Document;
        public int Line;
        public int Column;
    }

    public sealed class MetadataDebugInformation
    {
        // Internal
        internal readonly MetadataReferenceProvider metadataProvider = null;
        internal readonly MethodDebugInformation debugInfo = default;

        // Private
        private Lazy<string> fileName = null;
        private Lazy<IReadOnlyList<MetadataSequencePoint>> sequencePoints = null;

        // Properties
        public string Document => fileName.Value;
        public IReadOnlyList<MetadataSequencePoint> SequencePoints => sequencePoints.Value;

        // Constructor
        internal MetadataDebugInformation(MetadataReferenceProvider metadataProvider, in MethodDefinitionHandle handle)
        {
            this.metadataProvider = metadataProvider;
            this.debugInfo = metadataProvider.MetadataReader.GetMethodDebugInformation(handle);

            this.fileName = new(InitFileName);
        }

        // Methods
        private string InitFileName()
        {
            // Check for nil
            if (debugInfo.Document.IsNil == true)
                return null;
            
            // Get the document
            Document doc = metadataProvider.DebugSymbolsReader.GetDocument(debugInfo.Document);

            // Get the file name
            return metadataProvider.DebugSymbolsReader.GetString(doc.Name);
        }

        private IReadOnlyList<MetadataSequencePoint> InitSequencePoints()
        {
            // Get result
            List<MetadataSequencePoint> sequencePoints = new();

            // Get all sequence points
            foreach(SequencePoint sequencePoint in debugInfo.GetSequencePoints())
            {
                // Get the document
                Document doc = metadataProvider.DebugSymbolsReader.GetDocument(sequencePoint.Document);

                // Add sequence point
                sequencePoints.Add(new MetadataSequencePoint
                {
                    Offset = sequencePoint.Offset,
                    Document = metadataProvider.DebugSymbolsReader.GetString(doc.Name),
                    Line = sequencePoint.StartLine,
                    Column = sequencePoint.StartColumn,
                });
            }
            return sequencePoints;
        }
    }
}
