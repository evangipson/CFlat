# CFlat
A [native AoT](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot) .NET C# audio engine using [FMOD Core](https://www.fmod.com/core), with an example C++ project invoking the exposed C# methods.

All of the C# code is entirely native AoT, with a few methods exposed in [`CFlat.Native/Events`](src/CFlat.Native/Events/) intended to be called from unmanaged code.

## Getting Started
If publishing the native AoT C# to other runtimes or operating systems is desired, modify the `dotnet publish` command to target the desired runtime or operating system in the [`deploy.ps1`](scripts/deploy.ps1) script.
1. Download the repo
1. Navigate to the [`scripts`](scripts) directory
1. Run the [`deploy.ps1`](scripts/deploy.ps1) script
1. Open the solution in Visual Studio
1. Set [`CFlat.Example`](src/CFlat.Example/) as the startup project
1. Run the project to see C# code being invoked from C++
1. Optionally, run the tests in the [`tests`](tests) directory

## Architecture
### C#
The [`CFlat.Core`](src/CFlat.Core/) project mainly houses [models](src/CFlat.Core/Models/), [extensions](src/CFlat.Core/Extensions/), and [constants](src/CFlat.Core/Constants/) for the application.

The [`CFlat.Application`](src/CFlat.Application/) project contains all of the [services](src/CFlat.Application/Services/) and [factories](src/CFlat.Application/Factories/).

The [`CFlat.Native`](src/CFlat.Native/) project contains [events](src/CFlat.Application/Events/), which are "exposed" methods that are intended to be invoked from unmanaged code.

### C++
The [`CFlat.Example`](src/CFlat.Example/) project is an example of invoking the [exposed events from `CFlat.Application`](src/CFlat.Application/Events/).

The [`InteropService`](src/CFlat.Example/src/interop/interop-service.h) manages loading the native AoT libraries and creating function pointers from the [exposed events in `CFlat.Application`](src/CFlat.Application/Events/).

The [`AudioEngineService`](src/CFlat.Example/src/audio/audio-engine-service.h) and [`SoundService`](src/CFlat.Example/src/audio/sound-service.h) invoke the functions created by the [`InteropService`](src/CFlat.Example/src/interop/interop-service.h).

The [`main function`](src/CFlat.Example/src/cflat.cpp) shows the final result, a simple and easy way to invoke methods defined in C# from C++!