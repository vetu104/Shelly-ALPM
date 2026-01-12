using System;

namespace PackageManager.Alpm;

public class AlpmPackageOperationEventArgs(AlpmEventType eventType, string? packageName) : EventArgs
{
    public AlpmEventType EventType { get; } = eventType;
    public string? PackageName { get; } = packageName;
}
