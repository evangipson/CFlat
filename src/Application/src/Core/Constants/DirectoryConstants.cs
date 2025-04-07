namespace CFlat.Core.Constants;

/// <summary>A <see langword="static"/> collection of constant directory values.</summary>
public static class DirectoryConstants
{
    /// <summary>The current directory of the running application.</summary>
    public static readonly string CurrentDirectory = Environment.CurrentDirectory;

    /// <summary>The base directory the assembly resolver uses to probe for assemblies.</summary>
    public static readonly string BaseDomainDirectory = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>The user's directory on the local filesystem.</summary>
    public static readonly string UserDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile, Environment.SpecialFolderOption.DoNotVerify), Environment.UserName);

    /// <summary>The directory for audio assets.</summary>
    public static readonly string AudioAssetsDirectory = Path.Combine("assets", "audio");
}
