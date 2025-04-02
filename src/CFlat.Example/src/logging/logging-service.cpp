#include "logging-service.h"

const std::string LoggingService::COLOR_GREY = "\033[90m";
const std::string LoggingService::COLOR_RESET = "\033[0m";

std::unordered_map<LoggingService::LogSeverity, LoggingService::LogSeverityInfo> LoggingService::_severityInfo =
{
    { LOGGING_LEVEL_DEBUG, { "\033[96m", "[Debug] " }},
    { LOGGING_LEVEL_INFORMATION, { "\033[92m", "[Info]  " }},
    { LOGGING_LEVEL_WARNING, { "\033[93m", "[Warn]  " }},
    { LOGGING_LEVEL_ERROR, { "\033[91m", "[Error] " }},
};

void LoggingService::LogInfo(const std::string& message, const char* callerFunctionName, int callerLine, const char* callerFile)
{
    Log(LOGGING_LEVEL_INFORMATION, message, callerFunctionName, callerLine, callerFile);
}

void LoggingService::LogDebug(const std::string& message, const char* callerFunctionName, int callerLine, const char* callerFile)
{
    Log(LOGGING_LEVEL_DEBUG, message, callerFunctionName, callerLine, callerFile);
}

void LoggingService::LogWarning(const std::string& message, const char* callerFunctionName, int callerLine, const char* callerFile)
{
    Log(LOGGING_LEVEL_WARNING, message, callerFunctionName, callerLine, callerFile);
}

void LoggingService::LogError(const std::string& message, const char* callerFunctionName, int callerLine, const char* callerFile)
{
    Log(LOGGING_LEVEL_ERROR, LoggingService::AppendLastSystemError(message), callerFunctionName, callerLine, callerFile);
}

const std::string LoggingService::AppendLastSystemError(const std::string& message)
{
    std::string errorInfo;

    #ifdef _WIN32
    errorInfo = std::to_string(GetLastError());
    #else
    errorInfo = dlerror();
    #endif

    return message + "\nError: " + errorInfo;
}

std::string LoggingService::GetCurrentTimestamp()
{
    auto now = std::chrono::system_clock::now();
    auto time_t_now = std::chrono::system_clock::to_time_t(now);
    const auto nowMs = std::chrono::duration_cast<std::chrono::milliseconds>(now.time_since_epoch()) % 1000;

    struct tm buf;
    localtime_s(&buf, &time_t_now);

    std::stringstream ss;
    ss << "[" << std::put_time(&buf, "%m/%d/%Y %H:%M:%S") << "." << std::setfill('0') << std::setw(3) << nowMs.count() << "]";

    return ss.str();
}

void LoggingService::Log(LogSeverity severity, const std::string& message, const char* callerFunctionName, int callerLine, const char* callerFile)
{
    // Trim the caller file down to just the file name.
    std::string callerFileName = std::string(callerFile);
    size_t lastSlash = callerFileName.find_last_of("\\");
    if (lastSlash != std::string::npos)
    {
        callerFileName = callerFileName.substr(lastSlash + 1);
    }

    // Get the relevant info for the log based on the severity.
    LogSeverityInfo severityInfo = _severityInfo.at(severity);

    // Log the formatted message out.
    std::cout
        << severityInfo.color << severityInfo.name
        << LoggingService::COLOR_GREY << callerFileName << "[" << callerLine << "] " << callerFunctionName << "()\n  "
        << LoggingService::COLOR_GREY << GetCurrentTimestamp() << " "
        << LoggingService::COLOR_RESET << message << std::endl;
}