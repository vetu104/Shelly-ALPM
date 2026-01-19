using System;
using System.Runtime.InteropServices;

namespace PackageManager.Alpm;

// Base event - just the type field
[StructLayout(LayoutKind.Sequential)]
internal struct AlpmEventAny
{
    public AlpmEventType Type;
}

// For ScriptletInfo event
[StructLayout(LayoutKind.Sequential)]
internal struct AlpmEventScriptletInfo
{
    public AlpmEventType Type;
    public IntPtr Line;  // const char* - the scriptlet message
}

// For PackageOperation events (Start/Done)
[StructLayout(LayoutKind.Sequential)]
internal struct AlpmPackageOperationEvent
{
    public AlpmEventType Type;      // 4 bytes
    public int Operation;           // 4 bytes (alpm_package_operation_t)
    public IntPtr OldPkgPtr;        // 8 bytes on 64-bit
    public IntPtr NewPkgPtr;        // 8 bytes on 64-bit
}

// For HookRun events
[StructLayout(LayoutKind.Sequential)]
internal struct AlpmEventHookRun
{
    public AlpmEventType Type;
    public IntPtr Name;             // const char* - hook name
    public IntPtr Desc;             // const char* - hook description
    public UIntPtr Position;        // size_t - position in hook list
    public UIntPtr Total;           // size_t - total hooks
}

// For DatabaseSync events
[StructLayout(LayoutKind.Sequential)]
internal struct AlpmEventDatabaseSync
{
    public AlpmEventType Type;
    public IntPtr DbName;           // const char* - database name
}