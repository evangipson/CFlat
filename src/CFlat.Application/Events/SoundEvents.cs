using System.Runtime.InteropServices;
using CFlat.Application.Services;
using CFlat.Core.Constants;
using FMOD;

namespace CFlat.Application.Events;

/// <summary>A <see langword="static"/> collection of <see cref="Sound"/> methods that can be invoked from other systems or unmanaged code.</summary>
public static class SoundEvents
{
    /// <summary>Plays a piano note from unmanaged code. <see cref="EngineEvents.InitAudioEngine(bool)"/> must be called first.</summary>
    /// <param name="note">The name of the piano note to play (i.e.: <c>'D'</c>).</param>
    /// <param name="flat">A flag to indicate whether the note is flat, defaults to <see langword="false"/>.</param>
    /// <param name="octave">The octave of the note to play, defaults to <c>3</c>.</param>
    [UnmanagedCallersOnly(EntryPoint = "CFLAT_EVENTS_PLAY_PIANO_NOTE")]
    public static void PlayPianoNote(char note, bool flat = false, short octave = 3)
    {
        var noteName = $"{string.Concat([$"{note.ToString().ToUpper()}", flat ? "b" : null, $"{octave}"])}";
        SoundService.LoadAndPlaySound(ChannelConstants.SoundEffectsChannelGroup, "piano", noteName);
    }
}
