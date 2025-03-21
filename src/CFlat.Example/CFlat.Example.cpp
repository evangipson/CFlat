#include <cstdint>
#include <stdio.h>
#include <string>
#include <thread>
#include <iostream>

extern "C" void cflat_free(uintptr_t);
extern "C" uintptr_t cflat_create_engine(int);
extern "C" int cflat_add_channel_group(uintptr_t, uintptr_t, float, bool);
extern "C" uintptr_t cflat_load_sound(uintptr_t, uintptr_t, unsigned int = 0x00000009);
extern "C" int cflat_play_sound(uintptr_t, uintptr_t, uintptr_t);
extern "C" int cflat_load_and_play_sound(uintptr_t, uintptr_t, uintptr_t, unsigned int = 0x00000009);
extern "C" void cflat_start_engine(uintptr_t, int = 50);
extern "C" void cflat_stop_engine();

int main()
{
    std::cout << "CFlat from C++!" << std::endl;

    auto audioEngine = cflat_create_engine(512);

    std::string ambianceChannelName = "AmbianceChannelGroup";
    auto addChannelResult = cflat_add_channel_group(audioEngine, (uintptr_t)&ambianceChannelName, 1.0f, false);

    std::string musicChannelName = "MusicChannelGroup";
    auto addChannelResult2 = cflat_add_channel_group(audioEngine, (uintptr_t)&musicChannelName, 1.0f, false);

    cflat_start_engine(audioEngine);

    std::string newSoundName = "..\\..\\assets\\audio\\ambiance\\sand-ambiance.wav";
    auto sandAmbianceSound = cflat_load_sound(audioEngine, (uintptr_t)&newSoundName);
    auto playSoundResult = cflat_play_sound(audioEngine, sandAmbianceSound, (uintptr_t)&ambianceChannelName);

    std::this_thread::sleep_for(std::chrono::seconds(10));

    cflat_stop_engine();

    // must free all pointers
    cflat_free(sandAmbianceSound);
    cflat_free(audioEngine);

    std::cout << "All done!" << std::endl;
}
