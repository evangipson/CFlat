using CFlat.Core.Constants;

namespace CFlat.Application.Services;

/// <summary>Responsible for interacting with and managing the file system of the application environment.</summary>
public static class FileSystemService
{
    /// <summary>Gets the project's root directory.</summary>
    /// <returns>The project's root directory.</returns>
    public static string GetProjectRootDirectory() => string.Join(Path.DirectorySeparatorChar, DirectoryConstants.CurrentDirectory
        .Split(Path.DirectorySeparatorChar)
        .TakeWhile(path => !path.Equals("CFlat", StringComparison.OrdinalIgnoreCase))
        .Append("CFlat"));
}
