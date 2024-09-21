# shim files for the `required` feature

Functional polyfill copied from https://github.com/dotnet/runtime[^1] to allow use of `required` keyword in frameworks below .NET7 (if  `LangVersion`is set high enough to offer that feature.)

---

[^1]:[SetsRequiredMemberAttribute.cs](https://github.com/dotnet/runtime/blob/c1ab6c5b2524880ed9368841a0a5d8ead78ea09d/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/SetsRequiredMembersAttribute.cs), [RequiredMemberAttribute.cs](https://github.com/dotnet/runtime/blob/c1ab6c5b2524880ed9368841a0a5d8ead78ea09d/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/RequiredMemberAttribute.cs), [CompilerFeatureRequiredAttribute.cs](https://github.com/dotnet/runtime/blob/c1ab6c5b2524880ed9368841a0a5d8ead78ea09d/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/CompilerFeatureRequiredAttribute.cs)


