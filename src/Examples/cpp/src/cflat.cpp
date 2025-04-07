#include "cflat.h"

int main()
{
    AudioEngineService::InitAudioEngine(false);
    LoggingService::LogInfo("Started audio engine.");

    SoundService::PlayPianoNote('D', false, 3);
    LoggingService::LogInfo("Playing D3 piano sound.");

    std::this_thread::sleep_for(std::chrono::seconds(3));
    LoggingService::LogInfo("Played D3 piano sound.");

    AudioEngineService::StopAudioEngine();
    LoggingService::LogInfo("Stopped audio engine.");

    std::this_thread::sleep_for(std::chrono::seconds(1));

    AudioEngineService::InitAudioEngine(true);
    LoggingService::LogInfo("Started a new 3d audio engine.");

    SoundService::PlayPianoNote('D', false, 3);
    SoundService::PlayPianoNote('G', true, 3);
    SoundService::PlayPianoNote('A', false, 3);
    SoundService::PlayPianoNote('C', false, 4);
    LoggingService::LogInfo("Playing Dm7 chord.");

    std::this_thread::sleep_for(std::chrono::seconds(3));
    LoggingService::LogInfo("Played Dm7 chord.");

    AudioEngineService::StopAudioEngine();
    LoggingService::LogInfo("Stopped 3d audio engine.");
};