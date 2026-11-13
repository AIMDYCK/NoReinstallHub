# Changelog

## 1.1.2 — 2026-11-12
- Clip button corners to remove visual residue.
- Replace native path borders with rounded frames and a visible focus state.
- Make theme and language selectors compact, with a drawn arrow and native menu instead of the white ComboBox border.
- Preserve keyboard use, selection, translations, and preference storage.

## 1.1.1 — 2026-11-12
- Compact buttons and cards with tighter window spacing.
- Synchronize the native Windows 11 title bar with light/dark/system themes and high contrast.
- Fix button backgrounds to remove residual corners and borders.
- Add checks for button height and native title bar color attribute.

## 1.1.0 — 2026-11-12
### Interface
- Two-step Fluent-inspired design with clear typography, cards, folder icons, spacing, and a primary action.
- Light, dark, and system themes; respect high contrast and system animation settings.
- Complete English, Spanish, Portuguese, and French localization.
- First-run setup, local preferences, localized help, keyboard shortcuts, and scrollable content.

### Functionality
- Validate paths without creating anything; show progress and inline results; open destination and copy its path.
- Collapsible session activity that is not persisted.
- Create junctions through native Windows APIs; no cmd.exe, PowerShell, or automatic elevation.
- Exclusive creation to prevent collisions; literal support for &, %, !, and ^.
- Preserve invalid settings files and unknown fields.

### Quality and distribution
- Real NTFS tests and a simulated visual matrix across four languages, two themes, and four scales.
- Clean/restore/build in Debug and Release with warnings treated as errors.
- No direct or transitive NuGet packages before or after.
- Keep Microsoft .NET 8 as explicitly required.

## 1.0.0
- Original baseline: create junctions with mklink /J in Windows Forms.
- Earlier robustness work was documented in INFORME_MEJORAS.md; those changes had no separate commits when 1.1 work began.
