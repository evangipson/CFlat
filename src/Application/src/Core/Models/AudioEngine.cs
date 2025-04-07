using FMOD;

namespace CFlat.Core.Models;

/// <summary>A <see langword="readonly"/> object that maintains top-level engine information for the application.</summary>
/// <param name="system">The main system that is used throughout the application.</param>
/// <param name="is3d">A flag that determines if the engine will support 3d sound or not. Defaults to <see langword="false"/>.</param>
public readonly struct AudioEngine(FMOD.System system, bool is3d = false)
{
    /// <summary>The <see cref="FMOD.System"/> for the project.</summary>
    public readonly FMOD.System System { get; } = system;

    /// <summary>A list of <see cref="ChannelGroup"/> which will contain one or many <see cref="Channel"/>.</summary>
    public readonly List<ChannelGroup> ChannelGroups { get; } = [];

    /// <summary>A flag that indicates whether the Audio Engine is configured for 3D sound.</summary>
    public readonly bool Is3D { get; } = is3d;
}
