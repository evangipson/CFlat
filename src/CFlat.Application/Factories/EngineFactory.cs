using FMOD;
using CFlat.Core.Models;
using CFlat.Core.Extensions;

namespace CFlat.Application.Factories;

/// <summary>Responsible for creating <see cref="AudioEngine"/> objects.</summary>
public class EngineFactory
{
    /// <summary>Creates a new <see cref="AudioEngine"/>.</summary>
    /// <param name="is3d">A flag to determine if the engine supports 3D sound. Defaults to <see langword="false"/>.</param>
    /// <param name="maxChannels">The amount of channels the <see cref="AudioEngine"/> can support. Defaults to <c>512</c>.</param>
    /// <param name="initFlags">One or many bitwise-OR <see cref="INITFLAGS"/> to intialize the engine. Defaults to <see cref="INITFLAGS.NORMAL"/>.</param>
    /// <returns>The newly created <see cref="AudioEngine"/>.</returns>
    public static AudioEngine CreateEngine(bool is3d = false, int maxChannels = 512, INITFLAGS initFlags = INITFLAGS.NORMAL)
    {
        Factory.System_Create(out FMOD.System system)
            .OnFailure("Unable to create System for Audio Engine.");

        system.init(maxChannels, initFlags, nint.Zero)
            .OnFailure("Unable to initialize System for Audio Engine.");

        return new(system, is3d);
    }
}
