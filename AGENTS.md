# NoReinstallHub

C# / Microsoft .NET 8 / Windows Forms. Keep the framework, purpose, and product name. The explicit .NET 8 requirement takes precedence over historical migration suggestions.

## Verification
- Full: `pwsh -NoProfile -File scripts/Verificar.ps1`.
- Quick, without screenshots: `pwsh -NoProfile -File scripts/Verificar.ps1 -SkipVisual`.
- The script cleans known outputs, restores with NuGet.Config, builds Debug and Release with warnings as errors, runs the test runner, and queries NuGet.
- Do not use `dotnet test` as a substitute for the executable runner.
- Local publish: `dotnet publish NoReinstallHub.csproj -c Release --no-restore --self-contained false -o artifacts/portable`.
- Keep source out of bin/obj/artifacts. A temporary delivery under obj is not a new architecture.

## Architecture
- JunctionService: path normalization, validation, and asynchronous operation.
- NativeJunction: exclusive creation and NTFS reparse data through SafeFileHandle and Win32 APIs. No shells, services, or elevation.
- MainForm/Designer: responsive layout, two steps, inline results and actions. Keep disk operations off the UI thread.
- UiTheme: palettes, buttons, icons, and selectors. Preserve button accessibility, native menus, and the inner TextBox. Respect high contrast and system animation settings.
- Texts: all user-facing strings in en/es/pt/fr. Translate error codes in the UI; avoid mixing in technical messages.
- AppPreferences: theme/language/welcome only; atomic JSON in LocalAppData. Never save paths, overwrite unreadable settings, or discard unknown fields.
- Version.props: assembly and package version. Keep changelog and documentation synchronized.

## Conventions
Nullable enabled; follow .editorconfig. Dispose GDI resources, SafeFileHandle, RegistryKey, and timers when finished.
Never replace or delete destinations, including empty folders. Never move user data or traverse reparse points. Do not create paths in the background without an explicit user action.
Tests use a GUID directory under artifacts and remove only links they created before cleaning their own data.
Creation uses exclusive CreateDirectory; do not replace it with Directory.CreateDirectory to reserve a destination.
There are no NuGet dependencies; do not add UI packages without a concrete need. When adding a dependency, enable the required source deliberately and review license and vulnerabilities.
Do not upload or publish, sign, install system packages, or change Windows security without authorization. An explicit request to update GitHub authorizes that publication only.
Simulated screenshots do not certify real DPI behavior, screen reader support, or a clean installation.

## Git
Work on an update branch and preserve remote history and LICENSE. Never force-push or bypass branch protections. Review the diff, verify, and publish through a pull request containing the exact tested commit. Keep binaries, caches, preferences, and secrets out of the repository; distribution ZIP files belong in Releases.
