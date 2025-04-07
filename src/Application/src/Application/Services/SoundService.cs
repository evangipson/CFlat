using CFlat.Core.Constants;
using CFlat.Core.Extensions;
using CFlat.Core.Models;
using FMOD;
using Thread = System.Threading.Thread;

namespace CFlat.Application.Services;

/// <summary>Responsible for interacting with and managing <see cref="Sound"/>.</summary>
public static class SoundService
{
    private static readonly string _assetsDirectory = Path.Combine(FileSystemService.GetProjectRootDirectory(), DirectoryConstants.AudioAssetsDirectory);

    private static AudioEngine? AudioEngine => EngineService.AudioEngine;

    /// <summary>Loads and plays a <see cref="Sound"/>.</summary>
    /// <param name="channelGroupName">The name of the <see cref="ChannelGroup"/> to attach the <see cref="Sound"/> to.</param>
    /// <param name="relativeSoundPath">The relative path of the audio asset from the <see cref="DirectoryConstants.AudioAssetsDirectory"/>.</param>
    /// <param name="soundName">The name of the audio asset, without the extension.</param>
    /// <param name="format">The extension type, without a prefixed dot, defaults to <c>"mp3"</c>.</param>
    /// <param name="mode">The <see cref="MODE"/> to load the <see cref="Sound"/> with, defaults to <see cref="MODE.CREATECOMPRESSEDSAMPLE"/> and <see cref="MODE.LOOP_OFF"/>.</param>
    public static void LoadAndPlaySound(string channelGroupName, string relativeSoundPath, string soundName, string format = "mp3", MODE mode = MODE.CREATECOMPRESSEDSAMPLE | MODE.LOOP_OFF)
    {
        LoadSound(relativeSoundPath, soundName, format, mode)?.PlaySound(channelGroupName);
    }

    /// <summary>Loads and plays a <see cref="Sound"/> as a stream.</summary>
    /// <param name="channelGroupName">The name of the <see cref="ChannelGroup"/> to attach the <see cref="Sound"/> to.</param>
    /// <param name="relativeSoundPath">The relative path of the audio asset from the <see cref="DirectoryConstants.AudioAssetsDirectory"/>.</param>
    /// <param name="soundName">The name of the audio asset, without the extension.</param>
    /// <param name="format">The extension type, without a prefixed dot, defaults to <c>"mp3"</c>.</param>
    /// <param name="mode">The <see cref="MODE"/> to load the <see cref="Sound"/> with, defaults to <see cref="MODE.CREATESTREAM"/>, <see cref="MODE.NONBLOCKING"/>, and <see cref="MODE.LOOP_OFF"/>.</param>
    public static void LoadAndPlayStream(string channelGroupName, string relativeSoundPath, string soundName, string format = "mp3", MODE mode = MODE.CREATESTREAM | MODE.NONBLOCKING | MODE.LOOP_OFF)
    {
        LoadStream(relativeSoundPath, soundName, format, mode)?.PlaySound(channelGroupName);
    }

    private static Sound? LoadSound(string relativeSoundPath, string soundName, string format, MODE mode)
    {
        var absoluteSoundPath = Path.Combine(_assetsDirectory, relativeSoundPath);
        var fullSoundPath = Path.Combine(absoluteSoundPath, $"{soundName}.{format}");
        var spatialMode = mode | EngineService.GetSpatialMode();

        return AudioEngine?.System.createSound(fullSoundPath, spatialMode, out Sound sound)
            .OnFailure($"Can't create {soundName}.{format} sound from the {absoluteSoundPath} directory.")
            .GetValueOrDefault(() => sound.SpatializeSound());
    }

    private static Sound? LoadStream(string relativeSoundPath, string soundName, string format, MODE mode)
    {
        var absoluteSoundPath = Path.Combine(_assetsDirectory, relativeSoundPath);
        var fullSoundPath = Path.Combine(absoluteSoundPath, $"{soundName}.{format}");
        var spatialMode = mode | EngineService.GetSpatialMode();

        return AudioEngine?.System.createStream(fullSoundPath, spatialMode, out Sound stream)
            .OnFailure($"Can't create {soundName}.{format} stream from the {absoluteSoundPath} directory.")
            .GetValueOrDefault(() =>
            {
                stream.getOpenState(out OPENSTATE state, out _, out _, out _);
                while (state != OPENSTATE.READY)
                {
                    Thread.Sleep(EngineConstants.UpdateMillisecondTick);
                    stream.getOpenState(out state, out _, out _, out _)
                        .OnFailure($"Can't get the open state of the {soundName}.{format} sound from the {absoluteSoundPath} directory");
                }

                return stream.SpatializeSound();
            });
    }

    private static void PlaySound(this Sound sound, string channelGroupName)
    {
        var channelGroup = ChannelService.FindChannelGroup(channelGroupName);
        if (channelGroup == null)
        {
            return;
        }

        AudioEngine?.System.playSound(sound, channelGroup.Value, true, out Channel channel)
            .OnFailure("Can't play loaded sound.")
            .OnSuccess(() =>
            {
                channel.setChannelGroup(channelGroup.Value)
                    .OnFailure("Unable to set channel group for loaded sound's channel.");

                channel.setVolume(1.0f)
                    .OnFailure("Unable to set volume for channel");

                channel.SpatializeChannel();

                channel.setPaused(false)
                    .OnFailure("Unable to set paused to false for channel.");
            });
    }

    private static Sound SpatializeSound(this Sound sound)
    {
        if (EngineService.AudioEngine?.Is3D != true)
        {
            return sound;
        }

        return sound.set3DMinMaxDistance(0.5f * EngineConstants.DistanceFactor, 5000.0f * EngineConstants.DistanceFactor)
            .OnFailure("Could not set 3D min max distance for sound.")
            .GetValueOrDefault(() => sound);
    }
}
