namespace PackageManager.Alpm;

public enum AlpmPkgReason : int
{
    Explicit = 0,
    Depend = 1,
    Unknown = 2
}

public enum AlpmPkgFrom : int
{
    File = 1,
    LocalDb,
    SyncDb
}

[System.Flags]
public enum AlpmPkgValidation : int
{
    Unknown = 0,
    None = (1 << 0),
    Md5Sum = (1 << 1),
    Sha256Sum = (1 << 2),
    Signature = (1 << 3)
}
