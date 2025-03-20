#include <cstdint>
#include <stdio.h>
#include <string>
#include <iostream>

extern "C" void cflat_free(uintptr_t);
extern "C" uintptr_t cflat_create_engine(int);
extern "C" uintptr_t cflat_add_channel_group(uintptr_t, uintptr_t, float, bool);

int main()
{
    std::cout << "CFlat from C++!" << std::endl;

    auto audioEngine = cflat_create_engine(512);
    std::cout << "Pointer to new audio engine: " << audioEngine << "." << std::endl;

    std::string newChannelName = "New Channel";
    auto result = cflat_add_channel_group(audioEngine, (uintptr_t)&newChannelName, 1.0f, false);
    std::cout << "Result from new channel group add: " << result << "." << std::endl;

    // must free all pointers
    cflat_free(audioEngine);
    std::cout << "All done!" << std::endl;
}
