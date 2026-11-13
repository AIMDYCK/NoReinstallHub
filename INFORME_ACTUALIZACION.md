# NoReinstallHub 1.1.2 update report

NoReinstallHub is a Windows Forms desktop app on Microsoft .NET 8. It creates NTFS junctions so existing files remain in place while accessible through another path. The starting app had a dated interface and used a command interpreter for junction creation.

## Interface
- Two clear steps for real files and the new link location.
- Light, dark, and system themes; compact rounded buttons and selectors; rounded path fields with visible focus.
- English, Spanish, Portuguese, and French; keyboard navigation and first-run preferences.
- Expandable activity, inline results, progress, and actions to open or copy a path.

## Robustness
- Create junctions through native Windows APIs, without a shell or automatic elevation.
- Reserve the destination exclusively; never replace even an empty existing destination. Reject overlapping, network, and intermediate-link paths.
- Save preferences atomically without storing paths; preserve unreadable settings and unknown fields.

## Verification
- Clean, restore, and build Debug/Release without warnings or errors.
- 1,660 checks in the 1.1.2 review across path handling, data preservation, real junctions, collisions, preferences, menus, and editing.
- 32 simulated combinations across four languages, two themes, and 100/125/150/200% scaling.
- Screenshots are reproducible client-area captures using example paths.
- No direct or transitive NuGet packages before or after; vulnerability/outdated checks had no package references to report.

## Limits and follow-up
Simulations do not replace testing on physical DPI monitors, Narrator, or high-contrast systems. DWM title bar behavior could not be verified in the isolated build environment; the supplied user screenshot confirmed dark title bar behavior on a desktop. No operating system or runtime security audit is claimed. Microsoft .NET 8 was retained as required; its support ended November 10, 2026.

The repository retains its existing MIT license. Revert source changes with `git revert`; changing branches does not remove preferences or links created by users.
