#pragma once

#ifdef _WIN32
#include "windows.h"
#else
#include "dlfcn.h"
#include <unistd.h>
#endif

#include <chrono>
#include <cstdio>
#include <iomanip>
#include <iostream>
#include <stdio.h>
#include <sstream>
#include <string>
#include <unordered_map>

/// @brief Responsible for logging to the console.
class LoggingService
{
    public:
        /// @brief Logs @c message as information to the console.
        /// @param message The message to log to the console.
        static void LogInfo(const std::string& message, const char* callerFunctionName = __builtin_FUNCTION(), int callerLine = __builtin_LINE(), const char* callerFile = __builtin_FILE());

        /// @brief Logs @c message as a debug statement to the console.
        /// @param message The message to log to the console.
        static void LogDebug(const std::string& message, const char* callerFunctionName = __builtin_FUNCTION(), int callerLine = __builtin_LINE(), const char* callerFile = __builtin_FILE());

        /// @brief Logs @c message as a warning to the console.
        /// @param message The message to log to the console.
        static void LogWarning(const std::string& message, const char* callerFunctionName = __builtin_FUNCTION(), int callerLine = __builtin_LINE(), const char* callerFile = __builtin_FILE());

        /// @brief Logs @c message as an error to the console, and provides system-level error information based on the operating system.
        /// @param message The message to log to the console.
        static void LogError(const std::string& message, const char* callerFunctionName = __builtin_FUNCTION(), int callerLine = __builtin_LINE(), const char* callerFile = __builtin_FILE());

        /// @brief Appends the last system-level error based on the operating system to the @c message.
        /// @param message The message to append the last system error information to.
        /// @return The @c message with the last system-level error information as a constant string.
        static const std::string AppendLastSystemError(const std::string& message);

    private:
        enum LogSeverity
        {
            LOGGING_LEVEL_DEBUG,
            LOGGING_LEVEL_INFORMATION,
            LOGGING_LEVEL_WARNING,
            LOGGING_LEVEL_ERROR
        };

        struct LogSeverityInfo
        {
            LogSeverityInfo(const char* _color, const char* _name) : color(_color), name(_name) {};
            const char* color;
            const char* name;
        };

        static void Log(LogSeverity severity, const std::string& message, const char* callerFunctionName, int callerLine, const char* callerFile);

        static std::string GetCurrentTimestamp();

        static std::unordered_map<LogSeverity, LogSeverityInfo> _severityInfo;
        static const std::string COLOR_GREY;
        static const std::string COLOR_RESET;
};