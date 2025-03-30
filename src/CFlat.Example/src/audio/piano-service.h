#pragma once

#include "../interop/interop-constants.h"
#include "../interop/interop-service.h"

class PianoService
{
    public:
        static void PlayPianoNote(char note, bool flat, short octave);
};