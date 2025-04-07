#pragma once

#include "../interop/interop-constants.h"
#include "../interop/interop-service.h"

/// @brief Responsible for interacting with and managing sound.
class SoundService
{
    public:
        /// @brief Plays a piano note. Relies on @c AudioEngineService::InitAudioEngine() being called first.
        /// @param note - The name of the note to play (i.e.: 'D').
        /// @param flat - A flag indicating if the note is flat, defaults to false.
        /// @param octave - The octave of the note, defaults to 3.
        static void PlayPianoNote(char note, bool flat = false, short octave = 3);
};