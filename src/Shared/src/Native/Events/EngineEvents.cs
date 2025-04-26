using System.Runtime.InteropServices;
using CFlat.Application.Services;

namespace CFlat.Native.Events;

/// <summary>A <see langword="static"/> collection of <see cref="AudioEngine"/> methods that can be invoked from other systems or unmanaged code.</summary>
public static class EngineEvents
{
    /// <summary>Initializes the audio engine from unmanaged code.</summary>
    /// <param name="is3d">A flag to determine if the audio engine supports 3d, defaults to <see langword="false"/>.</param>
    [UnmanagedCallersOnly(EntryPoint = "CFLAT_EVENTS_INIT_AUDIO_ENGINE")]
    public static void InitAudioEngine(bool is3d = false) => EngineService.InitializeEngine(is3d);

    /// <summary>Stops the audio engine from unmanaged code.</summary>
    [UnmanagedCallersOnly(EntryPoint = "CFLAT_EVENTS_STOP_AUDIO_ENGINE")]
    public static void StopAudioEngine() => EngineService.StopEngine();
}
