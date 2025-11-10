### Current progress Experimental-NoMono branch

The experimental branch is now pretty stable and can run most LLM generated code properly, along with a host of hand generated test scripts and a demo game on IL2CPP platforms. 

#### Supported features:
✓ 90% of MSIL bytecode instructions implemented  
✓ Suport for single and multi-dimensional arrays  
✓ Support for generic methods  
✓ Support for by ref, our and in  
✓ Suppoort for interpreted classes, structs and enums  
✓ Cross domain (interop) calls to the host game/Unity API  
✓ Cross domain inheritance using a proxy binding  
Probably more that I can't think of right now.  

### Pros
- Run code in native Unity scripting language C#, for which there are plenty of resources for LLM to pull from.
- Fast execution and minimal allocations.
- Good interoperability with game or Unity API's, including generic methods.
- Semi-sandboxed execution with ability to unload assemblies if required. When used with Roslyn C#, also has the benefit of static and runtime code security so things like infinite loops in interpreted code cannot crash the host game.
- Ability to inherit from any interop type if a proxy binding is created.
- Can use reflection from the host game side to modify behaviour/instance fields, which could be useful for tweaking game settings or exposing them in some UI.

### Cons
- No bindings (or tool) available currently for interop API's. To call into interop API's like Unity API, it would be best to have bindings available that provide a means to call the API directly rather than via reflection to improve performance and remove small allocation cost of a reflection call. dotnow can still work just fine and fallback to reflection if no binding is present though.  
For that it will require a tool to be created to generate the binding code using CodeDom or CodeAnalysis.
- Further work will be required. The experimental branch solves some of the underlying issues that dotnow had previously that would cause incorrect behaviour, but it is essentially a rewrite from scratch in less that a week. From unit tests and demos it now seems pretty stable, but will no doubt require further expansion (A few instructions and features not implemented yet) and bug fixes as issue come up moving forward.
- Some specific generic code is not possible to support on AOT platforms such as generic factory patterns or similar, where the method is implemented in the host game, but is called from interpreted code with an interpreted type. It will trigger a runtime error stating that no AOT code was generated for the type. 
