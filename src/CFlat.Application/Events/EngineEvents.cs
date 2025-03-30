using System.Runtime.InteropServices;
using CFlat.Application.Services;

namespace CFlat.Application.Events;

public static class EngineEvents
{
    [UnmanagedCallersOnly(EntryPoint = "CFLAT_EVENTS_INIT_AUDIO_ENGINE")]
    public static void InitAudioEngine() => EngineService.InitializeEngine();

    [UnmanagedCallersOnly(EntryPoint = "CFLAT_EVENTS_STOP_AUDIO_ENGINE")]
    public static void StopAudioEngine() => EngineService.StopEngine();
}
