using FMOD;

namespace CFlat.Core.Models;

/// <summary>
/// A base system object for interacting with FMOD.
/// </summary>
public struct AudioEngine(FMOD.System system)
{
    /// <summary>
    /// The <see cref="FMOD.System"/> for the project.
    /// </summary>
    public FMOD.System System { get; set; } = system;

    /// <summary>
    /// A list of <see cref="ChannelGroup"/> which will
    /// contain one or many <see cref="Channel"/>.
    /// </summary>
    public List<ChannelGroup> ChannelGroups { get; set; } = [];

    /// <summary>
    /// The latest <see cref="RESULT"/> of the <see cref="FMOD.System"/>.
    /// </summary>
    public RESULT LatestResult { get; set; } = RESULT.OK;
}
