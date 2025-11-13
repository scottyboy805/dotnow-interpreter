# dotnow Interpreter
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

A pure C# CIL interpreter designed to load and execute managed code on AOT/IL2CPP (Unity) platforms.

Being able to run dynamically loaded code can be a great thing, especially in game development as it allows for modding support and much more. Combine it iwith a runtime compiler like Roslyn and the possibilities are practically endless. Unfortunately, it is not possible to natviley load code dynamically if AOT is used, since JIT is not available. dotnow was created to make this possible by interpreting the managed CIL instructions in software, and replicating the runtime environment.

# Project Goals
The end goal is to create a performant and feature complete runtime for managed code with support for AOT platforms including IL2CPP (Unity).
- Full support for generic types and members.
- Full reflection of interpreted types, as if they were loaded nativley.
- Implement all CIL instructions and runtime features.
- Support for inheritance of interop types (via proxy bindings).
- Execute code as fast as is reasonably possible.
- Support for debugging.

# Dependencies
- System.Reflection.Metadata (8.0.0.0) or newer. Used to read managed pe assembly images and metadata. 

# Limitations
dotnow is still very in early development although it is able to run most CIL code in its current form. There are however a few inherant limitations to the project:
- In order for user types to derive from interpreted types defined in external assemblies, a proxy binding must be created ahead of time.
- Invoking interop methods can be very slow unless a direct call binding is implemented ahead of time.
- Code stripping can cause issues in builds. You can use link.xml or direct call bindings to get around the problem. 
- AOT code for some generic types may not be emitted at build, causing runtime errors. For example If 'List`(int)' is not used inside the main project at any time but runtime interpreted code does use this generic type, it will cause a runtime exception stating (correctly) that no AOT code was generated. The workarounds for this are nasty hacks at the moment and involve declaring variables for all potential generic combinations ahead or time.

# Installation
dotnow can be installed using the Unity package manager via the following git URL. Follow [instructions here](https://docs.unity3d.com/Manual/upm-ui-giturl.html) for installing git packages in Unity.
`https://github.com/scottyboy805/dotnow-interpreter.git`

Alternativley check the [releases section](https://github.com/scottyboy805/dotnow-interpreter/releases) for .unitypackage versions.

# Usage
A dotnow `AppDomain` is required as a container for all interpreted code, and 1 or more assemblies can be loaded into the same domain. It is also possible to have multiple app domains in the same process.  
dotnow implementes the full `System.Reflection` interface, meaning that once an assembly is loaded, you can use the normal reflection API to find types and invoke members.
```cs
// Create dotnow app domain as the main container
// Be sure to use dotnow.AppDomain rather than System.AppDomain
AppDomain domain = new AppDomain();

// Load a managed assembly from file
// Returns a System.Assembly representing the dotnow interpreted module. Any members invoked from this assembly will run under dotnow interpeter rather than Mono/Jit
Assembly assembly = domain.LoadAssemblyFrom("MyAssembly.dll");

// Use reflection as normal to find a type
Type type = assembly.GetType("MyType");

// Find a method that we can invoke
MethodInfo method = type.GetMethod("MyMethod", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder, new Type[]{ typeof(int), typeof(string) }, null);

// Now invoke the method the same as normal reflection with asome arguments, and dotnow will interpret the bytecode
method.Invoke(null, new object[]{ 123, "Hello World" });

// Unlike Mono/Jit, dotnow domains can be fully unloaded once you are finished with them
domain.Dispose();
```

dotnow assemblies can also be unloaded without destroying the domain via the assemby load context. The domain caches all interop members, so it makes sense to reuse it if possible:
```cs
// Get the load context for a dotnow assembly
AssemblyLoadContext loadContext = domain.GetLoadContext(assembly);

// Now can can dispose just this context, and the domai and any other assemblies will remain in memory
loadContext.Dispose();
```

Take a look at the [wiki](https://github.com/scottyboy805/dotnow-interpreter/wiki) for more samples.

# Demos
- Snake: A simple snake game that is dynamically loaded and interpreted on Unity WebGL with IL2CPP backend. Demo scripts use arrays, collections, generics, enums and derive from MonoBehaviour. Find the demo game [here](https://trivialinteractive.co.uk/products/demo/trivialclr/snake). Find the demo project in 'ExampleProjects/SnakeExample' (Game runtime scripts - dynamically loaded) and 'Assets/Examples/Snake' (Loading and setup code).

# Sponsors
You can sponsor this project to help it grow
[:heart: Sponsor](https://github.com/sponsors/scottyboy805)
