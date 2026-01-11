namespace PackageManager.Alpm;

public enum AlpmEventType
{
    CheckDepsStart = 1,
    CheckDepsDone = 2,
    InterConflictsStart = 3,
    InterConflictsDone = 4,
    TransactionStart = 5,
    TransactionDone = 6,
    PackageOperationStart = 7,
    PackageOperationDone = 8,
    IntegrityStart = 9,
    IntegrityDone = 10,
    LoadStart = 11,
    LoadDone = 12,
    ScriptletInfo = 13,
    RetrieveStart = 14,
    RetrieveDone = 15,
    RetrieveFailed = 16,
    PkgsignStart = 17,
    PkgsignDone = 18,
    DatabaseSyncStart = 19,
    DatabaseSyncDone = 20,
    DiskspaceStart = 21,
    DiskspaceDone = 22,
    HookStart = 23,
    HookDone = 24,
    HookRunStart = 25,
    HookRunDone = 26
}