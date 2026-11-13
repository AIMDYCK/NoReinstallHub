# Applied update plan — NoReinstallHub 1.1.2

The update keeps Microsoft .NET 8 and Windows Forms.

1. Use Fluent-inspired hierarchy, spacing, layers, and themes without introducing another framework.
2. Keep the two steps: real files and the new link location. Compact buttons and selectors; round the path fields.
3. Localize the interface and help in English, Spanish, Portuguese, and French. Add keyboard support, high contrast, and first-run setup.
4. Replace mklink with native Windows APIs, without elevation or command consoles. Preserve existing destinations.
5. Save only theme, language, and welcome state; protect unreadable preferences and unknown fields.
6. Verify Debug/Release, real junctions, collisions, and 32 simulated language/theme/scaling combinations.
7. Update documentation and reproducible client-area screenshots using example paths.
8. Preserve LICENSE and remote history. Publish from a branch/PR without force-push; keep binaries out of source control.

Design and platform references:
- [Windows principles](https://learn.microsoft.com/en-us/windows/apps/design/design-principles)
- [Typography](https://learn.microsoft.com/en-us/windows/apps/design/signature-experiences/typography)
- [WinUI Gallery, visual reference](https://github.com/microsoft/WinUI-Gallery)
- [.NET Windows Forms configuration](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/application-configuration)
- [Reparse data](https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/ns-ntifs-_reparse_data_buffer)
- [DWM attributes](https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute)
