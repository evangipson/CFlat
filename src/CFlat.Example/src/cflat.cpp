#include "cflat.h"

int main()
{
    std::cout << "Starting audio engine." << std::endl;
    AudioEngineService::InitAudioEngine();
    std::cout << "Started audio engine." << std::endl;

    PianoService::PlayPianoNote('D', false, 3);
    std::cout << "Playing D3 piano sound." << std::endl;

    std::this_thread::sleep_for(std::chrono::seconds(3));
    std::cout << "Played D3 piano sound." << std::endl;

    std::cout << "Stopping audio engine." << std::endl;
    AudioEngineService::StopAudioEngine();
    std::cout << "Stopped audio engine." << std::endl;
};