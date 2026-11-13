# Privacy — NoReinstallHub 1.1

NoReinstallHub runs locally. It does not use accounts, ads, analytics, telemetry, or network connections. It does not send files, paths, or preferences to third parties.

Paths you enter are used to validate and create the junction you request. The app does not scan disk contents or move files. Paths and activity remain in memory only for the current session.

Theme, language, and welcome-screen state are saved in `preferences.json` under `LocalAppData/NoReinstallHub`. A packaged app may redirect preferences to package-local storage. An unreadable file is preserved. No global Windows settings are changed.

The Copy path action puts the requested path on the Windows clipboard. Open destination asks the Windows shell to open that folder. Neither happens until you select its button.

Test reports and screenshots are generated only when development tools are run, under artifacts/. Those scripts do not upload them to any service.
