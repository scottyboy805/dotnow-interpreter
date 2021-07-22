using System;
using System.Reflection;
using TrivialCLR.Reflection;

namespace TrivialCLR.Runtime.JIT
{
    public interface IJITOptimizable
    {
        // Methods
        void EnsureJITOptimized();
    }

    public static class JITOptimize
    {
        // Methods
        /// <summary>
        /// Ensure that the specified assembly has been optimized. The assembly must be a <see cref="CLRModule"/> otherwise this method will do nothing.
        /// The optimize stage runs before the first invocation usually, but can be run ahead of time if needed.
        /// It usually takes a little bit of time to optimize the method instructions into an structure that can be executed quickly.
        /// Typically during this stage, member references will be resolved, and field access or method invocation instructions will be stored so that they can be executed as fast as possible. DirectAccessBindings and DirectCallBindings will be hooked up during this stage.
        /// // This will cause all constructors and methods for all types declared in the module to be optimized.
        /// </summary>
        /// <param name="module">The module to optimize</param>
        public static void EnsureJITOptimized(Assembly module)
        {
            if (module is IJITOptimizable)
                (module as IJITOptimizable).EnsureJITOptimized();
        }

        /// <summary>
        /// Ensure that the specified type has been optimized.
        /// The optimize stage runs before the first invocation usually, but can be run ahead of time if needed.
        /// It usually takes a little bit of time to optimize the method instructions into an structure that can be executed quickly.
        /// Typically during this stage, member references will be resolved, and field access or method invocation instructions will be stored so that they can be executed as fast as possible. DirectAccessBindings and DirectCallBindings will be hooked up during this stage.
        /// This will cause all declared constructors and methods of the type to be optimized.
        /// </summary>
        /// <param name="type">The type to optimize</param>
        public static void EnsureJITOptimized(Type type)
        {
            if (type is IJITOptimizable)
                (type as IJITOptimizable).EnsureJITOptimized();
        }

        /// <summary>
        /// Ensure that the specified method has been optimized.
        /// The optimize stage runs before the first invocation usually, but can be run ahead of time if needed.
        /// It usually takes a little bit of time to optimize the method instructions into an structure that can be executed quickly.
        /// Typically during this stage, member references will be resolved, and field access or method invocation instructions will be stored so that they can be executed as fast as possible. DirectAccessBindings and DirectCallBindings will be hooked up during this stage.
        /// </summary>
        /// <param name="method">The method to optimize</param>
        public static void EnsureJITOptimized(MethodBase method)
        {
            if (method is IJITOptimizable)
                (method as IJITOptimizable).EnsureJITOptimized();
        }

        /// <summary>
        /// Ensure that the specified method body has been optimized.
        /// The optimize stage runs before the first invocation usually, but can be run ahead of time if needed.
        /// It usually takes a little bit of time to optimize the method instructions into an structure that can be executed quickly.
        /// Typically during this stage, member references will be resolved, and field access or method invocation instructions will be stored so that they can be executed as fast as possible. DirectAccessBindings and DirectCallBindings will be hooked up during this stage.
        /// </summary>
        /// <param name="body">The method body to optimize</param>
        public static void EnsureJITOptimized(CLRMethodBodyBase body)
        {
            if (body is IJITOptimizable)
                (body as IJITOptimizable).EnsureJITOptimized();
        }
    }
}
