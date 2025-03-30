using System.Runtime.InteropServices;
using CFlat.Application.Services;
using CFlat.Core.Constants;

namespace CFlat.Application.Events;

public static class PianoEvents
{
    [UnmanagedCallersOnly(EntryPoint = "CFLAT_EVENTS_PLAY_PIANO_NOTE")]
    public static void PlayPianoNote(char note, bool flat = false, short octave = 3)
    {
        var noteName = $"{string.Concat([$"{note.ToString().ToUpper()}", flat ? "b" : null, $"{octave}"])}";
        SoundService.LoadAndPlaySound(ChannelConstants.SoundEffectsChannelGroup, "piano", noteName);
    }
}
