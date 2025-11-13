
//namespace dotnow.Integration
//{
//    [Preserve]
//    internal sealed class CommonAOT
//    {
//        // HACK - IL2CPP platforms will omit generation AOT code for types that are not used by game code.
//        // We define a few common types here to force IL2CPP backend to emit AOT code for these types which are more likely be used in interpreted code.

//        // AOT arrays - multidimensional
//        // Multidimensional arrays are compiled as types for which AOT code must be emitted to be supported. We can force that for primitive types via these declarations:
//        [Preserve]
//        private static readonly int[,] _int2 = new int[1,1];

//        // Ctor
//        [Preserve]
//        static CommonAOT()
//        {
//            PreserveCall();
//        }

//        // Methods
//        [Preserve]
//        private static void PreserveCall()
//        {
//            _int2[0, 0] = _int2[0, 0];
//        }
//    }
//}
