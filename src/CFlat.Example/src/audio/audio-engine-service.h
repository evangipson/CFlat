#pragma once
#include "../interop/interop-constants.h"
#include "../interop/interop-service.h"

class AudioEngineService
{
    public:
        static void InitAudioEngine();
        static void StopAudioEngine();
};