using CFlat.Core.Models;
using FMOD;

namespace CFlat.Core.Constants;

/// <summary>A <see langword="static"/> collection of constant <see cref="AudioEngine"/> values.</summary>
public static class EngineConstants
{
    /// <summary>The amount of milliseconds between each <see cref="AudioEngine"/> update cycle.</summary>
    public const int UpdateMillisecondTick = 50;

    /// <summary>Units per meter of hearing distance (i.e.: feet would be <c>3.28f</c>, centimeters would be <c>100f</c>).</summary>
    public const float DistanceFactor = 1.0f;

    /// <summary>The default listener position for a 3d <see cref="AudioEngine"/>.</summary>
    public static readonly VECTOR ListenerPosition = new()
    {
        x = 0.0f,
        y = 0.0f,
        z = -1.0f * DistanceFactor
    };
}
