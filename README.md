# CFlat
A [native AoT](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot) .NET C# audio engine using [FMOD Core](https://www.fmod.com/core), with an example C++ project invoking the exposed C# methods.

All of the C# code is entirely native AoT, with a few methods exposed in [`Shared Native project`](src/Shared/src/Native/Events/) intended to be called from unmanaged code.

## Getting Started
1. Download the repo
1. Run `dotnet publish src\Shared -c Release --use-current-runtime --property:PublishDir=src\Examples\cpp\lib` to publish the Shared native AoT .dll, .lib, and .pdb files to the C++ example project's `lib` directory
1. Open the [`C++ example solution`](src/Examples/cpp) in Visual Studio
1. Run the project to see C# code being invoked from C++

## Architecture
### C#
The [`CFlat.Core`](src/Application/src/Core/) project mainly houses [models](src/Application/src/Core/Models/), [extensions](src/Application/src/Core/Extensions/), and [constants](src/Application/src/Core/Constants/) for the application.

The [`CFlat.Application`](src/Application/src/Application/) project contains all of the [services](src/Application/src/Application/Services/) and [factories](src/Application/src/Application/Factories/).

The [`CFlat.Native`](src/Shared/src/Native/) project contains [events](src/Shared/src/Native/Events/), which are "exposed" methods that are intended to be invoked from unmanaged code.

### C++
The [`CFlat.Example`](src/Examples/cpp/) project is an example of invoking the [exposed events from the Shared Native project](src/Shared/src/Native/Events/).

The [`InteropService`](src/Examples/cpp/src/interop/interop-service.h) manages loading the native AoT libraries and creating function pointers from the [exposed events in `CFlat.Application`](src/Application/src/Application/Events/).

The [`AudioEngineService`](src/Examples/cpp/src/audio/audio-engine-service.h) and [`SoundService`](src/Examples/cpp/src/audio/sound-service.h) invoke the functions created by the [`InteropService`](src/Examples/cpp/src/interop/interop-service.h).

The [`main function`](src/Examples/cpp/src/cflat.cpp) shows the final result, a simple and easy way to invoke methods defined in C# from C++!