#pragma once

#include <string>

/// @brief A static collection of constant native AoT function names.
class InteropFunctionNames
{
    public:
        /// @brief The name of the native AoT function that initializes the audio engine.
        static const std::string InitAudioEngine;

        /// @brief The name of the native AoT function that stops the audio engine.
        static const std::string StopAudioEngine;

        /// @brief The name of the native AoT function that plays a piano note.
        static const std::string PlayPianoNote;
};