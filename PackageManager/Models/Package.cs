namespace PackageManager.Models;

public class Package
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string NewVersion { get; set; }
    public long Size { get; set; }
}