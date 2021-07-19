# Trivial-CLR
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

A pure C# CIL interpreter designed to load and execute managed code on AOT/IL2CPP (Unity) platforms.

Being able to run dynamically loaded code can be a great thing, especially in game development as it allows for modding support and much more. Combine it iwith a runtime compiler like Roslyn and the possibilities are practically endless. Unfortunately, it is not possible to natviley load code dynamically if AOT is used, since JIT is not available. Trivial CLR was created to make this possible by interpreting the managed CIL instructions in software, and replicating the runtime environment.

# Project Goals
The end goal is to create a performant and feature complete runtime for managed code with support for AOT platforms including IL2CPP (Unity).
- Full support for generic types and members.
- Full reflection of interpreted types, as if they were loaded nativley.
- Implement all CIL instructions and runtime features.
- Support for inheritance of interop types (via proxy bindings).
- Execute code as fast as is reasonably possible.
- Support for debugging.

# Limitations
Trivial CLR is still very in early development although it is able to run most CIL code in its current form. There are however a few inherant limitations to the project:
- In order for user types to derive from interpreted types defined in external assemblies, a proxy binding must be created ahead of time.
- Invoking interop methods can be very slow unless a direct call binding is implemented ahead of time.
- Code stripping can cause issues in builds. You can use link.xml or direct call bindings to get around the problem. 

# Sponsors
You can sponsor this project to help it grow
[:heart: Sponsor](https://github.com/sponsors/scottyboy805)
