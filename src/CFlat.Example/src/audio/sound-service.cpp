#include "sound-service.h"

void SoundService::PlayPianoNote(char note, bool flat, short octave)
{
    auto playPianoNoteFunc = InteropService::GetFunctionCharBoolShort(InteropFunctionNames::PlayPianoNote);
    if (playPianoNoteFunc == nullptr)
    {
        std::cerr << "Error running PianoService::PlayPianoNote: symbol was null" << std::endl;
    }
    playPianoNoteFunc(note, flat, octave);
};