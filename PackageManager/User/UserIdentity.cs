namespace PackageManager.User;

using System.Runtime.InteropServices;

public static partial class UserIdentity
{
    [LibraryImport("libc")]
    private static partial uint getuid();
    
    public static bool IsRoot() => getuid() == 0;
}