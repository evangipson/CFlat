using FMOD;

namespace CFlat.Core.Constants;

/// <summary>A <see langword="static"/> collection of constant <see cref="VECTOR"/> values.</summary>
public static class VectorConstants
{
    /// <summary>A <see cref="VECTOR"/> filled with <c>0.0f</c> for x, y, and z.</summary>
    public static readonly VECTOR Zero = new()
    {
        x = 0f,
        y = 0f,
        z = 0f
    };
}
