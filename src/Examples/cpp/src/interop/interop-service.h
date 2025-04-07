#pragma once

#if defined(_WIN32)
#define PathToLibrary ".\\lib\\CFlat.Native.dll"
#elif defined(__APPLE__)
#define PathToLibrary "./lib/CFlat.Native.dylib"
#else
#define PathToLibrary "./lib/CFlat.Native.so"
#endif

#ifdef _WIN32
#include "windows.h"
#define symLoad GetProcAddress
#pragma comment (lib, "ole32.lib")
#else
#include "dlfcn.h"
#include <unistd.h>
#define symLoad dlsym
#define CoTaskMemFree free
#endif

#ifndef F_OK
#define F_OK    0
#endif

#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <string>
#include <unordered_map>
#include <functional>
#include "../logging/logging-service.h"

/// @brief Responsible for loading native AoT libraries and the functions they provide.
class InteropService
{
    public:
        /// @brief Gets a generic-returning function with variable parameters from a native AoT library.
        /// 
        /// @c GetFunction<int>() - Creates a pointer to an int-returning function with no arguments.
        /// 
        /// @c GetFunction<int, char, int>() - Creates a pointer to an int-returning function with a char and int argument.
        /// 
        /// @tparam TReturn - The return type of the native AoT function.
        /// @tparam ...TArgs - A list of argument types the function will use.
        /// @param interopFunctionName - The name of the function to load.
        /// @return The loaded native AoT function.
        /// @throws std::runtime_error - If the function cannot be cast properly.
        template<typename TReturn, typename... TArgs>
        static std::function<TReturn(TArgs...)> GetFunction(const std::string& interopFunctionName)
        {
            // Return the function pointer if it already exists in the loaded symbol cache.
            if (_loadedSymbols.find(interopFunctionName) != _loadedSymbols.end())
            {
                // Safe to not try/catch because the symbol is only put into loaded symbol cache if it's cast correctly.
                LoggingService::LogDebug("Already had the " + interopFunctionName + " function loaded, returning loaded symbol from cache.");
                return std::function<TReturn(TArgs...)>(reinterpret_cast<TReturn(*)(TArgs...)>(_loadedSymbols[interopFunctionName]));
            }

            // Load the symbol if it hasn't been loaded
            void* symbol = GetInteropFunction(interopFunctionName);

            // Try and return the function pointer loaded from the native AoT library.
            // (note: reinterpret_cast doesn't throw an exception, so null checking after is sufficient).
            std::function<TReturn(TArgs...)> loadedFunction = std::function<TReturn(TArgs...)>(reinterpret_cast<TReturn(*)(TArgs...)>(symbol));

            // Ensure the function was cast correctly by throwing if the output of reinterpret_cast was NULL.
            if (loadedFunction == nullptr)
            {
                std::string errorMessage = LoggingService::AppendLastSystemError("Error casting the function " + interopFunctionName);
                throw std::runtime_error(errorMessage);
            }

            // Add the symbol to the loaded symbol cache once it has been loaded and cast correctly, to avoid null checks when using the loaded symbol cache.
            LoggingService::LogDebug("Adding the " + interopFunctionName + " function to the loaded symbol cache.");
            _loadedSymbols[interopFunctionName] = symbol;

            return loadedFunction;
        };

        /// @brief Gets a void-returning function with variable parameters from a native AoT library.
        /// 
        /// @c GetAction<int>() - Creates a pointer to a void-returning function with an int argument.
        /// 
        /// @c GetAction<int, char, bool>() - Creates a pointer to a void-returning function with an int, char, and bool arguments.
        /// 
        /// @tparam ...TArgs - A list of argument types the function will use.
        /// @param interopFunctionName - The name of the void-returning function to load.
        /// @return The loaded native AoT function.
        /// @throws std::runtime_error - If the function cannot be cast properly.
        template<typename... TArgs>
        static std::function<void(TArgs...)> GetAction(const std::string& interopFunctionName)
        {
            return GetFunction<void, TArgs...>(interopFunctionName);
        };

    private:
        /// @brief Loads the native AoT library defined by @c PathToLibrary which contains symbols for all the callable functions.
        /// @throws std::runtime_error - If the native AoT library cannot be loaded.
        static void LoadInteropLibrary();

        /// @brief Ensures the interop library is loaded, and gets the symbol definition for the @c interopFunctionName from it.
        /// @param interopFunctionName - The name of the function to load.
        /// @return A pointer to the native AoT function, which must be cast to the apporpriate type using @c Func.
        /// @throws std::runtime_error - If the native AoT function cannot be loaded.
        static void* GetInteropFunction(const std::string& interopFunctionName);

        static HINSTANCE _libraryHandle;
        static std::unordered_map<std::string, void*> _loadedSymbols;
};