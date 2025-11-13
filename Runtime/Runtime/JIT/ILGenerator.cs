#define DEBUG_EMIT_INSTRUCTIONS

using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace dotnow.Runtime.JIT
{
    public sealed class ILGenerator
    {
        // Private
        private AppDomain domain = null;
        private MemoryStream instructionBuffer = null;
        private BinaryWriter instructionWriter = null;
        private bool validateInstructions = true;

        public ILGenerator(AppDomain domain, MemoryStream buffer = null)
        {
            this.domain = domain;
            this.instructionBuffer = buffer;

            // Check for null
            if (buffer == null)
                this.instructionBuffer = new MemoryStream(4096);

            // Create writer
            this.instructionWriter = new BinaryWriter(instructionBuffer);
        }

        internal ILGenerator(AppDomain domain, MemoryStream buffer = null, bool validateInstructions = true)
            : this(domain, buffer)
        {
            this.validateInstructions = validateInstructions;
        }

        // Constructor
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            instructionBuffer.SetLength(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GenerateBytecode()
        {
            return instructionBuffer.ToArray();
        }

        #region Emit
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Emit(ILOpCode op)
        {
#if DEBUG_EMIT_INSTRUCTIONS
            Debug.LineFormat("Emit: {0}: {1}", instructionBuffer.Position.ToString("X4"), op);
#endif

            instructionWriter.Write((byte)op);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Emit(ILOpCode op, sbyte operand)
        {
#if DEBUG_EMIT_INSTRUCTIONS
            Debug.LineFormat("Emit: {0}: {1} {2}", instructionBuffer.Position.ToString("X4"), op,
                op.IsBranch() == true ? operand.ToString("X4") : operand);
#endif

            instructionWriter.Write((byte)op);
            instructionWriter.Write(operand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Emit(ILOpCode op, short operand)
        {
#if DEBUG_EMIT_INSTRUCTIONS
            Debug.LineFormat("Emit: {0}: {1} {2}", instructionBuffer.Position.ToString("X4"), op, operand);
#endif

            instructionWriter.Write((byte)op);
            instructionWriter.Write(operand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Emit(ILOpCode op, int operand)
        {
#if DEBUG_EMIT_INSTRUCTIONS
            Debug.LineFormat("Emit: {0}: {1} {2}", instructionBuffer.Position.ToString("X4"), op, 
                op.IsBranch() == true ? operand.ToString("X4") : operand);
#endif

            instructionWriter.Write((byte)op);
            instructionWriter.Write(operand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Emit(ILOpCode op, long operand)
        {
#if DEBUG_EMIT_INSTRUCTIONS
            Debug.LineFormat("Emit: {0}: {1} {2}", instructionBuffer.Position.ToString("X4"), op, operand);
#endif

            instructionWriter.Write((byte)op);
            instructionWriter.Write(operand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Emit(ILOpCode op, float operand)
        {
#if DEBUG_EMIT_INSTRUCTIONS
            Debug.LineFormat("Emit: {0}: {1} {2}", instructionBuffer.Position.ToString("X4"), op, operand);
#endif

            instructionWriter.Write((byte)op);
            instructionWriter.Write(operand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Emit(ILOpCode op, double operand)
        {
#if DEBUG_EMIT_INSTRUCTIONS
            Debug.LineFormat("Emit: {0}: {1} {2}", instructionBuffer.Position.ToString("X4"), op, operand);
#endif

            instructionWriter.Write((byte)op);
            instructionWriter.Write(operand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Emit(ILOpCode op, int[] operand)
        {
#if DEBUG_EMIT_INSTRUCTIONS
            Debug.LineFormat("Emit: {0}: {1} {2}", instructionBuffer.Position.ToString("X4"), op, operand);
#endif

            instructionWriter.Write((byte)op);
            instructionWriter.Write((ushort)operand.Length);

            for (int i = 0; i < operand.Length; i++)
                instructionWriter.Write(operand[i]);
        }
        #endregion
    }
}
