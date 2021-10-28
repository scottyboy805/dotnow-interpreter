using dotnow.Runtime.CIL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace dotnow.Runtime
{
    /// <summary>
    /// Not an actual GC tracked heap. Just keeps track of managed objects that need to remain in scope.
    /// GC still handles cleanup of these objects, but they must be freed for that to happen.
    /// </summary>
    public class __heapallocator
    {
        // Types
        private struct TrackedMemory
        {
            // Public
            public CILFieldAccess field;            // Field ptr
            public object instance;                 // Stack data / managed object / Array            
            public long index;                      // Array / stack index
        }

        // Private
        private const int heapInitialSize = 1024;
        private const int trackedSize = 256;

        private static readonly StackData[] directAccessTemp = new StackData[2];

        private static Dictionary<Thread, __heapallocator> threadHeaps = new Dictionary<Thread, __heapallocator>();
        private List<TrackedMemory> trackedMemory = new List<TrackedMemory>();

        // Properties
        public int Size
        {
            get { return trackedMemory.Count; }
        }

        // Constructor
        internal __heapallocator()
        {
            // Resize heap
            trackedMemory.Capacity = heapInitialSize;

            // Add dummy
            trackedMemory.Add(new TrackedMemory()); // Element 0 is equal to null

            // Check for already added
            if(threadHeaps.ContainsKey(Thread.CurrentThread) == true)
                throw new InvalidOperationException("Cannot create a heap for the current thread because it has already been allocated");

            lock (threadHeaps)
            {
                // Register heap
                threadHeaps[Thread.CurrentThread] = this;
            }
        }

        // Methods
        public void PinManagedObject(ref StackData obj, in object managedObject)
        {
            // Check for null
            if (managedObject == null)
            {
                obj.type = StackData.ObjectType.Null;
                obj.address = 0;
                return;
            }

            // Get address for item
            int pinnedAddr = trackedMemory.Count;

            // Pin object in memory
            trackedMemory.Add(new TrackedMemory
            {
                instance = managedObject,                
            });

            // Update stack
            obj.type = StackData.ObjectType.Ref;
            obj.address = pinnedAddr;
        }

        internal void PinFieldAddress(ref StackData obj, CILFieldAccess fieldAccess, in StackData instance)
        {
            int pinnedAddr = trackedMemory.Count;

            // Allocate field address
            trackedMemory.Add(new TrackedMemory
            {
                field = fieldAccess,
                instance = instance,
            });

            // Update stack
            obj.type = StackData.ObjectType.RefField;
            obj.address = pinnedAddr;
        }

        internal void PinElementAddress(ref StackData obj, Array arrayInstance, long index)
        {
            int pinnedAddr = trackedMemory.Count;

            // Allocate element address
            trackedMemory.Add(new TrackedMemory
            {
                instance = arrayInstance,
                index = index,
            });

            // Update stack
            obj.type = StackData.ObjectType.RefElement;
            obj.address = pinnedAddr;
        }

        internal void PinStackAddress(ref StackData obj, StackData[] stack, int stackPtr)
        {
            int pinnedAddr = trackedMemory.Count;

            // Allocate element address
            trackedMemory.Add(new TrackedMemory
            {
                instance = stack,
                index = stackPtr,
            });

            obj.type = StackData.ObjectType.RefStack;
            obj.address = pinnedAddr;
        }

        public object FetchPinnedValue(in StackData addr)
        {
            int pinnedAddr = addr.address;

            // Check for null
            if (addr.type == StackData.ObjectType.Null || pinnedAddr == 0)
                return null;

            // Check for memory access
            if (pinnedAddr < 0 || pinnedAddr >= trackedMemory.Count)
                throw new AccessViolationException("Address does not point to allocated memory");

            switch (addr.type)
            {
                case StackData.ObjectType.Ref:
                    {
                        return trackedMemory[pinnedAddr].instance;
                    }
                case StackData.ObjectType.RefField:
                    {
                        return trackedMemory[pinnedAddr].field.targetField.GetValue(((StackData)trackedMemory[pinnedAddr].instance).Box(this));
                    }
                case StackData.ObjectType.RefElement:
                    {
                        return ((Array)trackedMemory[pinnedAddr].instance).GetValue(trackedMemory[pinnedAddr].index);
                    }

                case StackData.ObjectType.RefStack:
                    {
                        return ((StackData[])trackedMemory[pinnedAddr].instance)[trackedMemory[pinnedAddr].index].Box(this);
                    }
            }
            throw new InvalidOperationException("Cannot read memory of unknown type");
        }

        public T FetchPinnedValue<T>(in StackData addr)
        {
            int pinnedAddr = addr.address;

            // Check for null
            if (addr.type == StackData.ObjectType.Null || pinnedAddr == 0)
                return default;

            // Check for memory access
            if (pinnedAddr < 0 || pinnedAddr >= trackedMemory.Count)
                throw new AccessViolationException("Address does not point to allocated memory");

            switch (addr.type)
            {
                case StackData.ObjectType.Ref:
                    {
                        return (T)trackedMemory[pinnedAddr].instance;
                    }
                case StackData.ObjectType.RefField:
                    {
                        CILFieldAccess fieldAccess = trackedMemory[pinnedAddr].field;

                        if (fieldAccess.directReadAccessDelegate != null)
                        {
                            // Set instance
                            directAccessTemp[0] = (StackData)trackedMemory[pinnedAddr].instance;

                            // Invoke direct access
                            fieldAccess.directReadAccessDelegate(directAccessTemp, 0);
                            return (T)directAccessTemp[0].Box(this);
                        }

                        // Get field using reflection
                        return (T)trackedMemory[pinnedAddr].field.targetField.GetValue(((StackData)trackedMemory[pinnedAddr].instance).Box(this));
                    }

                case StackData.ObjectType.RefElement:
                    {
                        return (T)((Array)trackedMemory[pinnedAddr].instance).GetValue(trackedMemory[pinnedAddr].index);
                    }

                case StackData.ObjectType.RefStack:
                    {
                        return (T)((StackData[])trackedMemory[pinnedAddr].instance)[trackedMemory[pinnedAddr].index].Box(this);
                    }
            }

            return default;
        }

        internal void WritePinnedValue<T>(in StackData addr, T value)
        {
            int pinnedAddr = addr.address;

            // Check for null
            if (addr.type == StackData.ObjectType.Null || pinnedAddr == 0)
                return;

            // Check for memory access
            if (pinnedAddr < 0 || pinnedAddr >= trackedMemory.Count)
                throw new AccessViolationException("Address does not point to allocated memory");

            switch (addr.type)
            {
                case StackData.ObjectType.Ref:
                    {
                        TrackedMemory element = trackedMemory[pinnedAddr];
                        element.instance = value;

                        trackedMemory[pinnedAddr] = element;
                        break;
                    }
                case StackData.ObjectType.RefField:
                    {
                        CILFieldAccess fieldAccess = trackedMemory[pinnedAddr].field;

                        if (fieldAccess.directWriteAccessDelegate != null)
                        {
                            // Set instance
                            directAccessTemp[0] = (StackData)trackedMemory[pinnedAddr].instance;

                            StackData.AllocTypedSlow(this, ref directAccessTemp[1], typeof(T), value);

                            // Invoke direct access
                            fieldAccess.directWriteAccessDelegate(directAccessTemp, 0);
                        }

                        TrackedMemory element = trackedMemory[pinnedAddr];
                        element.field.targetField.SetValue(((StackData)trackedMemory[pinnedAddr].instance).Box(this), value);

                        trackedMemory[pinnedAddr] = element;
                        break;
                    }

                case StackData.ObjectType.RefElement:
                    {
                        ((T[])trackedMemory[pinnedAddr].instance)[trackedMemory[pinnedAddr].index] = value;
                        break;
                    }

                case StackData.ObjectType.RefStack:
                    {
                        // Store stack value
                        StackData.AllocTypedSlow(this, ref ((StackData[])trackedMemory[pinnedAddr].instance)[trackedMemory[pinnedAddr].index], typeof(T), value);
                        break;
                    }
            }
        }

        public void FreeMemory(int targetSize)
        {
            if (targetSize == 0 || targetSize >= trackedMemory.Count)
                return;

            // Free memory
            while(trackedMemory.Count > targetSize)
            {
                trackedMemory.RemoveAt(trackedMemory.Count - 1);
            }
        }

        public static __heapallocator GetCurrent()
        {
            __heapallocator result;
            threadHeaps.TryGetValue(Thread.CurrentThread, out result);

            return result;
        }
    }
}
