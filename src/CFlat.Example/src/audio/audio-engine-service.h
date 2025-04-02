#pragma once
#include "../interop/interop-constants.h"
#include "../interop/interop-service.h"

/// @brief Responsible for interacting with and managing the audio engine.
class AudioEngineService
{
    public:
        /// @brief Initializes the audio engine.
        /// @param is3d - A flag indicating if the engine should support 3d sound, defaults to @c false.
        static void InitAudioEngine(bool is3d = false);

        /// @brief Stops the audio engine.
        static void StopAudioEngine();
};