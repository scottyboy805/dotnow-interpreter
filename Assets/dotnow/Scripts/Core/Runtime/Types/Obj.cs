using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace dotnow.Runtime.Types
{
    [Flags]
    internal enum ObjFlags : byte
    {
        ValueType = 1 << 1,
        BoxedPrimitive = 1 << 2,
        Interop = 1 << 3,
    }

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct Obj
    {
        // Internal
        [FieldOffset(0)]
        internal int typeToken;     // Points to the System.Type metadata token of the object
        [FieldOffset(4)]
        internal IntPtr ptr;        // Points to pinned memory instance - could be 4 or 8 bytes - but must allow space for 8 bytes in all cases

        [FieldOffset(12)]
        internal ObjFlags flags;    // Info about the type of object - struct, boxed, etc
        [FieldOffset(13)]
        internal TypeID type;

        // Public
        public static readonly int Size = Marshal.SizeOf<Obj>() - 1;
        public static readonly int SizeTyped = Marshal.SizeOf<Obj>();
        public static readonly Obj Null = new Obj { type = TypeID.Object };

        // Properties
        public bool IsNull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ptr == IntPtr.Zero; }
        }

        public CLRInstance CLRInstance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return *(CLRInstance*)ptr; }
        }

        public object ManagedObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return __memory.GetManagedObject((int)ptr); }
        }

        // Methods
        public Type GetRuntimeType(AppDomain domain)
        {
            // Check null
            if (IsNull == true)
                throw new NullReferenceException();

            // Check for interop
            if((flags & ObjFlags.Interop) != 0)
            {
                // Get system type
                return ManagedObject.GetType();
            }

            // Get instance type
            return CLRInstance.GetRuntimeType(domain);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", type, IsNull ? "null" : ptr.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Obj FromCLRObject(IntPtr obj)
        {
            // Check for null
            if (obj == IntPtr.Zero)
                return Null;

            // Get instance
            CLRInstance instance = *(CLRInstance*)obj;

            // Get object
            return new Obj
            {
                typeToken = instance.type.typeToken,
                ptr = obj,
                type = TypeID.Object,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Obj FromInteropObject(IntPtr obj)
        {
            // Fetch obj from memory
            object managedObject = __memory.GetManagedObject((int)obj);

            // Check for null
            if (managedObject == null)
                return Null;

            // Get flags
            ObjFlags flags = ObjFlags.Interop;

            // Check for value type

            // Get object
            return new Obj
            {
                typeToken = managedObject.GetType().MetadataToken,
                ptr = obj,
                flags = flags,
                type = TypeID.Object,
            };
        }
    }
}
