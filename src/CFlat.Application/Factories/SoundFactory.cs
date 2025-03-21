using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using FMOD;
using CFlat.Core.Models;
using MemoryExtensions = CFlat.Core.Extensions.MemoryExtensions;

namespace CFlat.Application.Factories;

/// <summary>Responsible for creating <see cref="Sound"/> objects.</summary>
public class SoundFactory
{
    [UnmanagedCallersOnly(EntryPoint = "cflat_load_sound")]
    public static nint LoadSound(nint audioEnginePointer, [MarshalAs(UnmanagedType.LPWStr)] nint soundNamePointer, MODE mode = MODE.LOOP_OFF | MODE._2D)
    {
        var audioEngine = MemoryExtensions.FromPointer<AudioEngine>(audioEnginePointer);
        var soundName = Marshal.PtrToStringAuto(soundNamePointer) ?? throw new InvalidCastException($"[LoadSound]: Unable to cast {nameof(soundNamePointer)} to string");

        audioEngine.System.createStream(soundName, mode, out Sound sound);
        return MemoryExtensions.GetPointer(sound);
    }

    [UnmanagedCallersOnly(EntryPoint = "cflat_play_sound")]
    public static RESULT PlaySound(nint audioEnginePointer, nint soundPointer, [MarshalAs(UnmanagedType.LPWStr)] nint channelNamePointer)
    {
        var audioEngine = MemoryExtensions.FromPointer<AudioEngine>(audioEnginePointer);
        var sound = MemoryExtensions.FromPointer<Sound>(soundPointer);
        var channelName = Marshal.PtrToStringAuto(channelNamePointer);
        if (channelName == null)
        {
            return RESULT.ERR_MEMORY_CANTPOINT;
        }

        var channelGroup = audioEngine.ChannelGroups.FirstOrDefault(channelGroup => channelGroup.getName(out string name, 50) == RESULT.OK && name.Equals(channelName));
        if (channelGroup.getName(out string channelGroupName, 50) != RESULT.OK || !channelGroupName.Equals(channelName))
        {
            return RESULT.ERR_CHANNEL_STOLEN;
        }

        audioEngine.System.playSound(sound, channelGroup, false, out Channel channel);
        channel.setChannelGroup(channelGroup);

        return RESULT.OK;
    }

    [RequiresUnreferencedCode("LoadAndPlaySound infers a function pointer, which requires unreferenced code.")]
    [UnmanagedCallersOnly(EntryPoint = "cflat_load_and_play_sound")]
    public static RESULT LoadAndPlaySound(nint audioEnginePointer,
        [MarshalAs(UnmanagedType.LPWStr)] nint soundNamePointer,
        [MarshalAs(UnmanagedType.LPWStr)] nint channelNamePointer,
        MODE mode = MODE.LOOP_OFF | MODE._2D)
    {
        var loadSoundPointer = MemoryExtensions.GetFunctionPointer("LoadSound");
        var loadSound = Marshal.GetDelegateForFunctionPointer<Func<nint, nint, MODE, nint>>(loadSoundPointer);
        var soundPointer = loadSound.Invoke(audioEnginePointer, soundNamePointer, mode);

        var playSoundPointer = MemoryExtensions.GetFunctionPointer("PlaySound");
        var playSound = Marshal.GetDelegateForFunctionPointer<Func<nint, nint, nint, RESULT>>(playSoundPointer);
        return playSound.Invoke(audioEnginePointer, soundPointer, channelNamePointer);
    }
}
