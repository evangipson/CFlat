namespace CFlat.Core.Constants;

public static class DirectoryConstants
{
    public static readonly string CurrentDirectory = Environment.CurrentDirectory;

    public static readonly string BaseDomainDirectory = AppDomain.CurrentDomain.BaseDirectory;

    public static readonly string UserDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile, Environment.SpecialFolderOption.DoNotVerify), Environment.UserName);

    public static readonly string ProjectRootDirectory = Directory.GetParent(BaseDomainDirectory)?.Parent?.Parent?.FullName ?? BaseDomainDirectory;

    public static readonly string AudioAssetsDirectory = Path.Combine("assets", "audio");
}
