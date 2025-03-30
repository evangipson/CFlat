using CFlat.Core.Constants;
using CFlat.Core.Extensions;
using CFlat.Core.Models;
using FMOD;
using Thread = System.Threading.Thread;

namespace CFlat.Application.Services;

public static class SoundService
{
    private static AudioEngine? AudioEngine => EngineService.AudioEngine;

    private static readonly string _assetsDirectory = Path.Combine(FileSystemService.GetProjectRootDirectory(), DirectoryConstants.AudioAssetsDirectory);

    public static void LoadAndPlaySound(string channelGroupName, string relativeSoundPath, string soundName, string format = "mp3", MODE mode = MODE.CREATECOMPRESSEDSAMPLE | MODE.LOOP_OFF)
    {
        LoadSound(relativeSoundPath, soundName, format, mode)?.PlaySound(channelGroupName);
    }

    public static void LoadAndPlayStream(string channelGroupName, string relativeSoundPath, string soundName, string format = "mp3", MODE mode = MODE.CREATESTREAM | MODE.NONBLOCKING | MODE.LOOP_OFF)
    {
        LoadStream(relativeSoundPath, soundName, format, mode)?.PlaySound(channelGroupName);
    }

    private static Sound? LoadSound(string relativeSoundPath, string soundName, string format, MODE mode)
    {
        var absoluteSoundPath = Path.Combine(_assetsDirectory, relativeSoundPath);
        var fullSoundPath = Path.Combine(absoluteSoundPath, $"{soundName}.{format}");

        return AudioEngine?.System.createSound(fullSoundPath, mode, out Sound sound)
            .OnFailure($"Can't create {soundName}.{format} sound from the {absoluteSoundPath} directory.")
            .GetValueOrDefault(() => sound);
    }

    private static Sound? LoadStream(string relativeSoundPath, string soundName, string format, MODE mode)
    {
        var absoluteSoundPath = Path.Combine(_assetsDirectory, relativeSoundPath);
        var fullSoundPath = Path.Combine(absoluteSoundPath, $"{soundName}.{format}");

        return AudioEngine?.System.createStream(fullSoundPath, mode, out Sound stream)
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

                return stream;
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

                channel.setPaused(false)
                    .OnFailure("Unable to set paused to false for channel.");
            });
    }
}
