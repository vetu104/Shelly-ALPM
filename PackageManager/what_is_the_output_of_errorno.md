In `libalpm`, the function `alpm_errno()` (which you have mapped as `ErrorNumber` in your C# code) returns an integer corresponding to the `alpm_errno_t` enumeration. This value represents the last error that occurred on the given library handle.

### Return Type and Translation
The output is an **integer error code**. To make this human-readable, `libalpm` provides another function:
```c
const char *alpm_strerror(alpm_errno_t err);
```
In C#, you would map it as:
```csharp
[LibraryImport(LibName, EntryPoint = "alpm_strerror", StringMarshalling = StringMarshalling.Utf8)]
public static partial string StrError(int err);
```

### Common Error Codes (`alpm_errno_t`)
The enumeration values are defined in `alpm.h`. Here are the most common outputs you might see:

| Value | Constant | Description |
| :--- | :--- | :--- |
| `0` | `ALPM_ERR_OK` | No error occurred. |
| `1` | `ALPM_ERR_MEMORY` | Failed to allocate memory. |
| `2` | `ALPM_ERR_SYSTEM` | A system error occurred (check `errno`). |
| `3` | `ALPM_ERR_BADPERMS` | Permission denied (e.g., not running as root). |
| `11` | `ALPM_ERR_HANDLE_LOCK` | Failed to acquire the database lock (another process is using pacman). |
| `17` | `ALPM_ERR_DB_NOT_FOUND` | The database could not be found. |
| `23` | `ALPM_ERR_SERVER_NONE` | The database has no configured servers (mirrors). |
| `30` | `ALPM_ERR_TRANS_ABORT` | The transaction was aborted. |
| `50` | `ALPM_ERR_RETRIEVE` | Download failed (common during `Update`). |

### How to use it in your code
In your test `UpdateDatabaseSucceeds`, if `result` is not `0`, you should check the error number and ideally translate it to a string for debugging:

```csharp
var result = Update(_handle, databases, false);
if (result != 0) {
    int errCode = ErrorNumber(_handle);
    string message = StrError(errCode);
    Console.WriteLine($"Update failed: {message} (Code: {errCode})");
}
```

### Summary of Output
*   **Success:** Returns `0`.
*   **Failure:** Returns a positive integer (usually between 1 and 55) mapped to specific error conditions like permission issues, network failures, or database locks.