#include "interop-service.h"

HMODULE InteropService::_libraryHandle = nullptr;
std::unordered_map<std::string, void*> InteropService::_loadedSymbols = {};

void InteropService::LoadInteropLibrary()
{
    // If the library is already loaded, there is nothing to do here.
    if (_libraryHandle != nullptr)
    {
        LoggingService::LogDebug("Already had the interop library loaded, returning cached handle.");
        return;
    }

    // Try and load the library if it's not loaded, or throw an exception if it can't be loaded.
    // (note: LoadLibraryA and dlopen won't throw an exception, so null checking after is sufficient).
    #ifdef _WIN32
    _libraryHandle = LoadLibraryA(PathToLibrary);
    #else
    _libraryHandle = dlopen(PathToLibrary, RTLD_LAZY);
    #endif

    // Ensure the library was loaded correctly by throwing if the output was NULL.
    if (_libraryHandle == nullptr)
    {
        std::string errorMessage = LoggingService::AppendLastSystemError("Error loading native AoT library " + std::string(PathToLibrary));
        throw std::runtime_error(errorMessage);
    }
};

void* InteropService::GetInteropFunction(const std::string& interopFunctionName)
{
    // Load the library if it hasn't been loaded
    LoadInteropLibrary();

    // Try to return the loaded symbol, otherwise throw an exception
    // (note: symLoad won't throw an exception, so null checking after is sufficient).
    void* symbol = symLoad(_libraryHandle, interopFunctionName.c_str());

    // Ensure the symbol was loaded correctly by throwing if the output of symLoad was NULL.
    if (symbol == nullptr)
    {
        std::string errorMessage = LoggingService::AppendLastSystemError("Error loading native AoT function " + interopFunctionName);
        throw std::runtime_error(errorMessage);
    }

    return symbol;
};