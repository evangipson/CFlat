using FMOD;

namespace CFlat.Core.Models;

/// <summary>A base object that maintains top-level system information for FMOD.</summary>
public readonly struct AudioEngine(FMOD.System system)
{
    /// <summary>The <see cref="FMOD.System"/> for the project.</summary>
    public readonly FMOD.System System { get; } = system;

    /// <summary>A list of <see cref="ChannelGroup"/> which will contain one or many <see cref="Channel"/>.</summary>
    public readonly List<ChannelGroup> ChannelGroups { get; } = [];
}
