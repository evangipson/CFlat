using CFlat.Core.Constants;

namespace CFlat.Application.Services;

public static class FileSystemService
{
    public static string GetProjectRootDirectory() => string.Join(Path.DirectorySeparatorChar, DirectoryConstants.CurrentDirectory
        .Split(Path.DirectorySeparatorChar)
        .TakeWhile(path => !path.Equals("CFlat", StringComparison.OrdinalIgnoreCase))
        .Append("CFlat"));
}
