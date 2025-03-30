using FMOD;
using CFlat.Core.Models;
using CFlat.Core.Extensions;

namespace CFlat.Application.Factories;

/// <summary>Responsible for creating <see cref="AudioEngine"/> objects.</summary>
public class EngineFactory
{
    /// <summary>Creates a new <see cref="AudioEngine"/>.</summary>
    /// <param name="maxChannels">The amount of channels the <see cref="AudioEngine"/> can support. Defaults to <c>512</c>.</param>
    /// <returns>The newly created <see cref="AudioEngine"/>.</returns>
    public static AudioEngine CreateEngine(int maxChannels = 512)
    {
        Factory.System_Create(out FMOD.System system)
            .OnFailure("Unable to create System for Audio Engine.");

        system.init(maxChannels, INITFLAGS.NORMAL, nint.Zero)
            .OnFailure("Unable to initialize System for Audio Engine.");

        return new(system);
    }
}
