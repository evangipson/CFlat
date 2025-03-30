using CFlat.Application.Factories;
using CFlat.Core.Constants;
using CFlat.Core.Extensions;
using CFlat.Core.Models;
using FMOD;
using Thread = System.Threading.Thread;

namespace CFlat.Application.Services;

public static class EngineService
{
    private static CancellationTokenSource _cancelTokenSource = new();
    private static Task? _updateTask;

    private static AudioEngine? _audioEngine;

    public static AudioEngine? AudioEngine => _audioEngine;

    public static bool IsRunning => _updateTask != null && !_cancelTokenSource.IsCancellationRequested;

    private static CancellationToken CancelToken => _cancelTokenSource.Token;

    public static void InitializeEngine()
    {
        if (IsRunning)
        {
            StopEngine();
        }

        _audioEngine = EngineFactory.CreateEngine();
        AddEngineChannelGroups();
        StartEngine();
    }

    public static void StopEngine()
    {
        _cancelTokenSource.Cancel();
        _cancelTokenSource = new();

        CleanUpEngine();

        _updateTask = null;
        _audioEngine = null;
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
}
