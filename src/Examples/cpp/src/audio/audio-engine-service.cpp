#include "audio-engine-service.h"

void AudioEngineService::InitAudioEngine(bool is3d)
{
    InteropService::GetAction<bool>(InteropFunctionNames::InitAudioEngine)(is3d);
}

void AudioEngineService::StopAudioEngine()
{
    InteropService::GetAction(InteropFunctionNames::StopAudioEngine)();
}