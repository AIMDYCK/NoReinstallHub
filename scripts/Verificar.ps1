# NoReinstallHub: local clean builds, native NTFS integration and visual checks.
param([switch]$SkipVisual)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$previousAppData = $env:APPDATA
Push-Location -LiteralPath $projectRoot
try {
    $env:APPDATA = Join-Path $projectRoot 'artifacts/build-profile'
    New-Item -ItemType Directory -Path $env:APPDATA -Force | Out-Null
    $packages = Join-Path $projectRoot 'artifacts/packages'
    foreach ($project in @('NoReinstallHub.csproj', 'tests/NoReinstallHub.Tests/NoReinstallHub.Tests.csproj')) {
        foreach ($configuration in @('Debug', 'Release')) {
            & dotnet clean $project -c $configuration --nologo
            if ($LASTEXITCODE -ne 0) { throw "Clean failed: $project/$configuration" }
        }
        & dotnet restore $project --configfile NuGet.Config --packages $packages
        if ($LASTEXITCODE -ne 0) { throw "Restore failed: $project" }
        foreach ($configuration in @('Debug', 'Release')) {
            & dotnet build $project -c $configuration --no-restore -warnaserror
            if ($LASTEXITCODE -ne 0) { throw "Build failed: $project/$configuration" }
        }
    }
    $testArguments = @()
    if ($SkipVisual) { $testArguments = @('--', '--skip-visual') }
    & dotnet run --project tests/NoReinstallHub.Tests/NoReinstallHub.Tests.csproj -c Release --no-build --no-restore @testArguments
    if ($LASTEXITCODE -ne 0) { throw 'Tests failed' }
    & dotnet list NoReinstallHub.csproj package --vulnerable --include-transitive --no-restore --format json > artifacts/nuget-vulnerabilities.json
    if ($LASTEXITCODE -ne 0) { throw 'NuGet vulnerability query failed' }
    & dotnet list NoReinstallHub.csproj package --outdated --include-transitive --no-restore --format json > artifacts/nuget-outdated.json
    if ($LASTEXITCODE -ne 0) { throw 'NuGet version query failed' }
}
finally {
    $env:APPDATA = $previousAppData
    Pop-Location
}
