using System.Runtime.InteropServices;
using FMOD;
using CFlat.Core.Models;
using Thread = System.Threading.Thread;
using MemoryExtensions = CFlat.Core.Extensions.MemoryExtensions;

namespace CFlat.Application.Factories;

/// <summary>Responsible for creating <see cref="AudioEngine"/> objects.</summary>
public class EngineFactory
{
    private static Action? engineUpdateLoop;
    private static Task? engineUpdateTask;

    /// <summary>Creates a new <see cref="AudioEngine"/>. The created <see cref="AudioEngine"/> must be freed by calling <c>cflat_free()</c>.</summary>
    /// <param name="maxChannels">The amount of channels the <see cref="AudioEngine"/> can support. Defaults to <c>512</c>.</param>
    /// <returns>The newly created <see cref="AudioEngine"/>.</returns>
    [UnmanagedCallersOnly(EntryPoint = "cflat_create_engine")]
    public static IntPtr CreateEngine(int maxChannels = 512)
    {
        Factory.System_Create(out FMOD.System system);
        AudioEngine audioEngine = new(system);

        var initializeResult = audioEngine.System.init(maxChannels, INITFLAGS.NORMAL, (IntPtr)OUTPUTTYPE.AUDIOOUT);
        audioEngine.LatestResult = initializeResult;

        return MemoryExtensions.GetPointer(audioEngine);
    }

    /// <summary>Creates a new <see cref="ChannelGroup"/> and attaches it to the<see cref="AudioEngine.ChannelGroups"/> list.</summary>
    /// <param name="audioEngine">Required audio engine to create the channel group for.</param>
    /// <param name="newChannelGroupName">Required name of the new channel group.</param>
    /// <param name="startingVolume">The starting volume of the channel, 1.0 is maximum volume, 0.0 is silent, defaults to 1.0.</param>
    /// <param name="willVolumeRamp"><c>true</c> if the volume will ramp, defaults to <c>false</c>.</param>
    /// <returns>The <see cref="RESULT"/> of the <see cref="ChannelGroup"/> creation and subsequent attachment to the <see cref="AudioEngine.ChannelGroups"/> list.</returns>
    [UnmanagedCallersOnly(EntryPoint = "cflat_add_channel_group")]
    public static RESULT AddChannelGroup(IntPtr audioEnginePointer, [MarshalAs(UnmanagedType.LPWStr)] IntPtr newChannelGroupNamePointer, float startingVolume = 1.0f, bool willVolumeRamp = false)
    {
        var audioEngine = MemoryExtensions.FromPointer<AudioEngine>(audioEnginePointer);
        var newChannelGroupName = Marshal.PtrToStringAuto(newChannelGroupNamePointer);
        if (newChannelGroupName == default)
        {
            return RESULT.ERR_MEMORY_CANTPOINT;
        }

        var result = audioEngine.System.createChannelGroup(newChannelGroupName, out ChannelGroup newChannelGroup);
        if (result == RESULT.OK)
        {
            newChannelGroup.setVolume(startingVolume);
            newChannelGroup.setVolumeRamp(willVolumeRamp);
            newChannelGroup.setPaused(false);
            audioEngine.ChannelGroups.Add(newChannelGroup);
        }

        return result;
    }

    [UnmanagedCallersOnly(EntryPoint = "cflat_start_engine")]
    public static void StartEngine(nint audioEnginePointer, int millisecondTickInterval = 50)
    {
        var audioEngine = MemoryExtensions.FromPointer<AudioEngine>(audioEnginePointer);

        engineUpdateTask = Task.Run(() =>
        {
            while (!engineUpdateTask?.IsCompleted ?? true)
            {
                audioEngine.System.update();
                Thread.Sleep(millisecondTickInterval);
            }

            return Task.CompletedTask;
        });
    }

    [UnmanagedCallersOnly(EntryPoint = "cflat_stop_engine")]
    public static void StopEngine() => engineUpdateTask = Task.CompletedTask;
}
