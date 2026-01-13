### Implementing AUR Access in Shelly

To implement AUR (Arch User Repository) access within Shelly, the best approach is to create a dedicated service that interfaces with the AUR RPC API for searching and information gathering, while leveraging the existing `AlpmManager` and `AlpmWorkerClient` infrastructure for package handling.

Based on the project structure, here is the recommended implementation strategy:

#### 1. AUR API Service
Create an `AurService` (e.g., in `PackageManager/Aur/AurService.cs`) to handle communication with the [AUR RPC interface](https://aur.archlinux.org/rpc/).
- **Functionality**: Use `HttpClient` to perform searches (`type=search`) and retrieve package details (`type=info`).
- **Data Transfer Objects**: Define `AurPackageDto` to map the JSON responses from the AUR.

#### 2. Extend the Interface
The `IAlpmManager` interface (found in `PackageManager/Alpm/IAlpmManager.cs`) should be extended or a new `IAurManager` should be created to include AUR-specific methods:
- `SearchAur(string query)`
- `DownloadAurSource(string packageName)`
- `BuildAndInstallAur(string packageName)`

#### 3. AUR Helper Integration (The "Worker" Approach)
Since Shelly uses a worker-client architecture (`AlpmWorkerClient`), AUR operations—specifically building packages—should be handled by a worker process to maintain UI responsiveness and security:
- **Build Process**: AUR packages require downloading a `PKGBUILD` and running `makepkg`. Since Shelly is written in C#, you can automate this by:
    1. Downloading the snapshot from the AUR.
    2. Extracting it to a temporary directory.
    3. Executing `makepkg -si` via the worker (handling dependencies through the existing ALPM logic).
- **Elevation**: Use the existing `pkexec` logic in `AlpmWorkerClient` to handle the installation phase of the AUR build.

#### 4. UI Integration
- **Configuration**: Enable the `AurEnabled` flag in `ShellyConfig.cs`.
- **Settings**: Update `SettingWindow.axaml` and `SettingViewModel.cs` to allow users to toggle AUR support.
- **Unified Search**: In `PackageViewModel`, merge results from `GetAvailablePackages()` (official repos) and the new AUR search service to provide a seamless experience.

#### 5. Recommended Tools/Libraries
- **System.Text.Json**: Already used in the project for config and worker communication; perfect for parsing AUR RPC responses.
- **Libalpm**: Continue using the existing P/Invoke wrappers in `AlpmReference.cs` to check for locally installed AUR packages and version comparisons.

By following this pattern, you maintain the clean separation between the UI and the package management backend while reusing the established worker process for privileged operations.