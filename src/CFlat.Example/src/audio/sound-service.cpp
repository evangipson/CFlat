#include "sound-service.h"

void SoundService::PlayPianoNote(char note, bool flat, short octave)
{
    InteropService::GetAction<char, bool, short>(InteropFunctionNames::PlayPianoNote)(note, flat, octave);
};