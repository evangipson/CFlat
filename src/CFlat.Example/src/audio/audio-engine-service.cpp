#include "audio-engine-service.h"

void AudioEngineService::InitAudioEngine(bool is3d)
{
    auto initAudioEngineFunc = InteropService::GetFunctionBool(InteropFunctionNames::InitAudioEngine);
    if (initAudioEngineFunc == nullptr)
    {
        std::cerr << "Error running AudioEngineService::InitAudioEngine: symbol was null" << std::endl;
    }

    initAudioEngineFunc(is3d);
}

void AudioEngineService::StopAudioEngine()
{
    auto stopAudioEngineFunc = InteropService::GetFunctionVoid(InteropFunctionNames::StopAudioEngine);
    if (stopAudioEngineFunc == nullptr)
    {
        std::cerr << "Error running AudioEngineService::StopAudioEngine: symbol was null" << std::endl;
    }

    stopAudioEngineFunc();
}