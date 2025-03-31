using CFlat.Application.Factories;
using CFlat.Core.Constants;
using CFlat.Core.Extensions;
using CFlat.Core.Models;
using FMOD;
using Thread = System.Threading.Thread;

namespace CFlat.Application.Services;

/// <summary>Responsible for interacting with and managing the <see cref="Core.Models.AudioEngine"/>.</summary>
public static class EngineService
{
    private static CancellationTokenSource _cancelTokenSource = new();
    private static Task? _updateTask;
    private static AudioEngine? _audioEngine;

    /// <summary>The main engine for audio in the application.</summary>
    public static AudioEngine? AudioEngine => _audioEngine;

    /// <summary>A flag that determines if the <see cref="AudioEngine"/> is currently updating.</summary>
    public static bool IsRunning => _updateTask != null && !_cancelTokenSource.IsCancellationRequested;

    private static CancellationToken CancelToken => _cancelTokenSource.Token;

    /// <summary>Initializes the <see cref="AudioEngine"/> by adding spatialization, adding a collection of <see cref="ChannelGroup"/>, and starting the update loop.</summary>
    /// <param name="is3d">A flag to determine if the engine will be loaded supporting 3d sound, defaults to <see langword="false"/>.</param>
    public static void InitializeEngine(bool is3d = false)
    {
        if (IsRunning)
        {
            StopEngine();
        }

        _audioEngine = EngineFactory.CreateEngine(is3d);
        AddSpatialization();
        AddEngineChannelGroups();
        StartEngine();
    }

    /// <summary>Stops the <see cref="AudioEngine"/> update loop, then releases and cleans up all attached entities.</summary>
    public static void StopEngine()
    {
        _cancelTokenSource.Cancel();
        _cancelTokenSource = new();

        CleanUpEngine();

        _updateTask = null;
        _audioEngine = null;
    }

    /// <summary>Gets the spatial mode for <see cref="Sound"/> for the <see cref="AudioEngine"/>, based on if 3d sound is supported.</summary>
    /// <returns><see cref="MODE._3D"/> if 3d sound is supported, <see cref="MODE._2D"/> otherwise.</returns>
    public static MODE GetSpatialMode()
    {
        return _audioEngine?.Is3D == true ? MODE._3D : MODE._2D;
    }

    private static void StartEngine()
    {
        CreateEngineUpdateThread();
    }

    private static void CreateEngineUpdateThread()
    {
        _updateTask ??= Task.Run(() =>
        {
            while (!CancelToken.IsCancellationRequested)
            {
                Thread.Sleep(EngineConstants.UpdateMillisecondTick);

                CancelToken.ThrowIfCancellationRequested();

                _audioEngine?.System.update()
                    .OnFailure("Audio Engine update loop encountered an issue.");
            }
        }, CancelToken);
    }

    private static void AddEngineChannelGroups()
    {
        ChannelService.AddChannelGroup(ChannelConstants.SoundEffectsChannelGroup);
    }

    private static void CleanUpEngine()
    {
        foreach (var channelGroup in _audioEngine?.ChannelGroups.ToList() ?? [])
        {
            channelGroup.getNumChannels(out int numChannels)
                .OnFailure("Could not get number of channels from channel group.");

            for (int i = 0; i < numChannels; i++)
            {
                channelGroup.getChannel(i, out Channel channel)
                    .OnFailure("Could not get channel from channel group.");

                channel.getCurrentSound(out Sound sound)
                    .OnFailure("Could not get sound from channel.");

                sound.release()
                    .OnFailure("Could not get release sound.");
            }

            channelGroup.release()
                .OnFailure("Could not release channel group.");
        }

        _audioEngine?.System.release()
            .OnFailure("Could not release Audio Engine system.")
            .OnSuccess(() => _audioEngine?.ChannelGroups.ToList().Clear());
    }

    private static void AddSpatialization()
    {
        if (_audioEngine?.Is3D != true)
        {
            return;
        }

        _audioEngine?.System.set3DSettings(1.0f, EngineConstants.DistanceFactor, 1.0f)
            .OnFailure("Could not set 3d settings for the Audio Engine.");
    }
}
