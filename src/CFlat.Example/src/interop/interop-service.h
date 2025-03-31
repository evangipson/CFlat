#pragma once

#if defined(_WIN32)
#define PathToLibrary ".\\lib\\CFLat.Application.dll"
#elif defined(__APPLE__)
#define PathToLibrary "./lib/CFLat.Application.dylib"
#else
#define PathToLibrary "./lib/CFLat.Application.so"
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

/// @brief Responsible for loading native AoT libraries and the functions they provide.
class InteropService
{
    public:
        /// @brief Loads a void returning function with no parameters from a native AoT library.
        /// @param interopFunctionName - The name of the function to load.
        /// @return The loaded native AoT function.
        static std::function<void()> GetFunctionVoid(const std::string& interopFunctionName);

        /// @brief Loads a void returning function with a bool parameter from a native AoT library.
        /// @param interopFunctionName - The name of the function to load.
        /// @return The loaded native AoT function.
        static std::function<void(bool)> GetFunctionBool(const std::string& interopFunctionName);

        /// @brief Loads a void returning function with char, bool, and short parameters from a native AoT library.
        /// @param interopFunctionName - The name of the function to load.
        /// @return The loaded native AoT function.
        static std::function<void(char, bool, short)> GetFunctionCharBoolShort(const std::string& interopFunctionName);

    private:
        static void LoadInteropLibrary();
        static void* GetInteropFunction(const std::string& interopFunctionName);

        static HMODULE _libraryHandle;
        static std::unordered_map<std::string, void*> _loadedSymbols;
};