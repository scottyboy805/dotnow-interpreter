using dotnow.Reflection;
using System;

namespace dotnow.Interop
{
    /// <summary>
    /// Represents an interpreted class, struct enum or array to be used by the host for interop purposes.
    /// </summary>
    public interface ICLRInstance
    {
        // Methods
        /// <summary>
        /// Get the <see cref="CLRType"/> of the instance.
        /// </summary>
        /// <returns></returns>
        Type GetInterpretedType();

        bool Equals(ICLRInstance otherInstance);

        /// <summary>
        /// Attempt to unwrap the type for use by an interop call or similar.
        /// Will return an instance that is safe to be passed to any interop method.
        /// </summary>
        /// <returns>An unwrapped object representing the interop base of this object</returns>
        object Unwrap();

        /// <summary>
        /// Attempt to unwrap the type for use by an interop call or similar as the specified base or interface type.
        /// Will return an instance that is safe to be passed to any interop method, or null if the instance does not derive from or implement the specified type.
        /// </summary>
        /// <param name="asType">The type to try and unwrap as. The type cannot be an interpreted type (<see cref="CLRType"/>) but must instead be an interop types defined by the host or the runtime</param>
        /// <returns>An unwrapped object of the specified type, or null if the instance does not derive from or implement the specified type</returns>
        object UnwrapAsType(Type asType);
    }

    

    
}
