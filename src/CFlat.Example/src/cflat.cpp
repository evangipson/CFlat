#include "cflat.h"

int main()
{
    AudioEngineService::InitAudioEngine(true);
    LoggingService::LogInfo("Started 3d audio engine.");

    SoundService::PlayPianoNote('D', false, 3);
    LoggingService::LogInfo("Playing D3 piano sound.");

    std::this_thread::sleep_for(std::chrono::seconds(3));
    LoggingService::LogInfo("Played D3 piano sound.");

    AudioEngineService::StopAudioEngine();
    LoggingService::LogInfo("Stopped audio engine.");
};