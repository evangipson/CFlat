using CFlat.Core.Constants;
using CFlat.Core.Extensions;
using CFlat.Core.Models;
using FMOD;

namespace CFlat.Application.Services;

/// <summary>Responsible for interacting with and managing any <see cref="ChannelGroup"/> or <see cref="Channel"/>.</summary>
public static class ChannelService
{
    private static AudioEngine? AudioEngine => EngineService.AudioEngine;

    /// <summary>Adds a <see cref="ChannelGroup"/> to the audio engine.</summary>
    /// <param name="newChannelGroupName">The name of the <see cref="ChannelGroup"/> to add.</param>
    /// <param name="startingVolume">The starting volume of the <see cref="ChannelGroup"/>, defaults to <c>1.0f</c> (100%).</param>
    /// <param name="willVolumeRamp">A flag to indicate if the audio will ramp up and down, defaults to <see langword="false"/>.</param>
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

    /// <summary>Finds a <see cref="ChannelGroup"/> from the audio engine by <paramref name="channelGroupName"/>.</summary>
    /// <param name="channelGroupName">The name of the <see cref="ChannelGroup"/> to find.</param>
    /// <returns>The <see cref="ChannelGroup"/> that was found, or <see langword="null"/>.</returns>
    public static ChannelGroup? FindChannelGroup(string channelGroupName) => AudioEngine?.ChannelGroups.First(channelGroup =>
    {
        channelGroup.getName(out string name, 50)
            .OnFailure("Unable to get the name of a channel group from the Audio Engine.");

        return channelGroupName.Equals(name);
    });

    /// <summary>Spatializes the <paramref name="channel"/> if the audio engine supports 3d sound. Does nothing otherwise.</summary>
    /// <param name="channel">The <see cref="Channel"/> to spatialize.</param>
    /// <param name="horizontalDistance">The horizontal distance the listener is from the sound, defaults to <c>1.0f</c>.</param>
    /// <param name="verticalDistance">The vertical distance the listener is from the sound, defaults to <c>1.0f</c>.</param>
    /// <param name="depthDistance">The distance forward or backward the listener is from the sound, defaults to <c>1.0f</c>.</param>
    /// <returns>The <paramref name="channel"/> with spatialization set if 3d sound is supported, otherwise will return <paramref name="channel"/> unmodified.</returns>
    public static Channel SpatializeChannel(this Channel channel, float horizontalDistance = 1.0f, float verticalDistance = 1.0f, float depthDistance = 1.0f)
    {
        if (AudioEngine?.Is3D != true)
        {
            return channel;
        }

        VECTOR velocity = VectorConstants.Zero;
        VECTOR position = new()
        {
            x = horizontalDistance,
            y = verticalDistance,
            z = depthDistance
        };

        channel.set3DAttributes(ref position, ref velocity)
            .OnFailure("Unable to spatialize channel for 3d.");

        return channel;
    }
}
