using CFlat.Core.Extensions;
using CFlat.Core.Models;
using FMOD;

namespace CFlat.Application.Services;

public static class ChannelService
{
    private static AudioEngine? AudioEngine => EngineService.AudioEngine;

    public static void AddChannelGroup(string newChannelGroupName, float startingVolume = 1.0f, bool willVolumeRamp = false)
    {
        AudioEngine?.System
            .createChannelGroup(newChannelGroupName, out ChannelGroup newChannelGroup)
            .OnFailure($"Could not create the {newChannelGroupName} channel group.")
            .OnSuccess(() =>
            {
                newChannelGroup.setVolume(startingVolume);
                newChannelGroup.setVolumeRamp(willVolumeRamp);
                newChannelGroup.setPaused(false);

                AudioEngine?.ChannelGroups.Add(newChannelGroup);
            });
    }

    public static ChannelGroup? FindChannelGroup(string channelGroupName) => AudioEngine?.ChannelGroups.First(channelGroup =>
    {
        channelGroup.getName(out string name, 50)
            .OnFailure("Unable to get the name of a channel group from the Audio Engine.");

        return channelGroupName.Equals(name);
    });
}
