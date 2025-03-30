#include "interop-service.h"

HMODULE InteropService::_libraryHandle = nullptr;
std::unordered_map<std::string, void*> InteropService::_loadedSymbols = {};

void InteropService::LoadInteropLibrary()
{
    if (_libraryHandle == nullptr)
    {
        #ifdef _WIN32
            _libraryHandle = LoadLibraryA(PathToLibrary);
        #else
            _libraryHandle = dlopen(PathToLibrary, RTLD_LAZY);
        #endif
    }

    if (_libraryHandle == nullptr)
    {
        std::cerr << "Error loading library: " << PathToLibrary << std::endl;
        return;
    }
};

void* InteropService::GetInteropFunction(const std::string& interopFunctionName)
{
    // Load the library if it hasn't been loaded
    if (_libraryHandle == nullptr)
    {
        LoadInteropLibrary();
    }

    // Load the symbol
    void* symbol = symLoad(_libraryHandle, interopFunctionName.c_str());
    if (symbol == nullptr)
    {
        std::cerr << "Error loading symbol: " << interopFunctionName << std::endl;
        return nullptr;
    }

    // Return the loaded symbol
    return symbol;
};

std::function<void()> InteropService::GetFunctionVoid(const std::string& interopFunctionName)
{
    // Return the symbol if it has been loaded
    if (_loadedSymbols.find(interopFunctionName) != _loadedSymbols.end())
    {
        return std::function<void()>(reinterpret_cast<void(*)()>(_loadedSymbols[interopFunctionName]));
    }

    // Load the symbol if it hasn't been loaded
    void* symbol = GetInteropFunction(interopFunctionName);

    // Add the symbol to the loaded symbols once it's loaded to avoid re-loading
    _loadedSymbols[interopFunctionName] = symbol;

    // Return the function pointer to the loaded symbol
    return std::function<void()>(reinterpret_cast<void(*)()>(symbol));
};

std::function<void(char, bool, short)> InteropService::GetFunctionCharBoolShort(const std::string& interopFunctionName)
{
    // Return the symbol if it has been loaded
    if (_loadedSymbols.find(interopFunctionName) != _loadedSymbols.end())
    {
        return std::function<void(char, bool, short)>(reinterpret_cast<void(*)(char, bool, short)>(_loadedSymbols[interopFunctionName]));
    }

    // Load the symbol if it hasn't been loaded
    void* symbol = GetInteropFunction(interopFunctionName);

    // Add the symbol to the loaded symbols once it's loaded to avoid re-loading
    _loadedSymbols[interopFunctionName] = symbol;

    return std::function<void(char, bool, short)>(reinterpret_cast<void(*)(char, bool, short)>(symbol));
};