# Build an unsigned local MSIX only from an explicitly supplied self-contained publish.
# No installation, signing, certificate creation or upload.
param(
    [Parameter(Mandatory)][ValidatePattern('^[A-Za-z0-9][A-Za-z0-9.-]{2,49}$')][string]$IdentityName,
    [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string]$Publisher,
    [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string]$PublisherDisplayName,
    [Parameter(Mandatory)][string]$PublishDirectory,
    [Parameter(Mandatory)][string]$MakeAppxPath
)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$publish = (Resolve-Path -LiteralPath $PublishDirectory).Path
foreach ($file in @('NoReinstallHub.exe', 'hostfxr.dll', 'System.Private.CoreLib.dll')) {
    if (-not (Test-Path -LiteralPath (Join-Path $publish $file))) {
        throw "A self-contained win-x64 publish is required. Missing: $file"
    }
}
if (-not (Test-Path -LiteralPath $MakeAppxPath)) { throw 'Provide the installed Windows SDK makeappx.exe path.' }
if ($Publisher -notmatch '^CN=') { throw 'Use the exact Publisher from Partner Center (CN=...). No identity is generated.' }
[xml]$versions = Get-Content -LiteralPath (Join-Path $projectRoot 'Version.props') -Raw
$version = [string]$versions.Project.PropertyGroup.StorePackageVersion
$folder = Join-Path $projectRoot ('artifacts/store-' + [Guid]::NewGuid().ToString('N'))
$payload = Join-Path $folder 'payload'
New-Item -ItemType Directory -Path $payload -Force | Out-Null
Copy-Item -Path (Join-Path $publish '*') -Destination $payload -Recurse
$manifest = [IO.File]::ReadAllText((Join-Path $PSScriptRoot 'Package.appxmanifest.template'))
$manifest = $manifest.Replace('__IDENTITY_NAME__', [Security.SecurityElement]::Escape($IdentityName)).
    Replace('__PUBLISHER__', [Security.SecurityElement]::Escape($Publisher)).
    Replace('__PUBLISHER_DISPLAY_NAME__', [Security.SecurityElement]::Escape($PublisherDisplayName)).
    Replace('__VERSION__', $version)
[IO.File]::WriteAllText((Join-Path $payload 'AppxManifest.xml'), $manifest, [Text.UTF8Encoding]::new($false))
Add-Type -AssemblyName System.Drawing
$assets = Join-Path $payload 'Assets'
New-Item -ItemType Directory -Path $assets -Force | Out-Null
$icon = [Drawing.Icon]::new((Join-Path $projectRoot 'app_icon.ico'), 256, 256)
$original = $icon.ToBitmap()
try {
    foreach ($asset in @(@('Square44x44Logo.png', 44), @('Square150x150Logo.png', 150), @('StoreLogo.png', 50))) {
        $bitmap = [Drawing.Bitmap]::new([int]$asset[1], [int]$asset[1])
        $graphics = [Drawing.Graphics]::FromImage($bitmap)
        try {
            $graphics.Clear([Drawing.Color]::Transparent)
            $graphics.DrawImage($original, 0, 0, [int]$asset[1], [int]$asset[1])
            $bitmap.Save((Join-Path $assets $asset[0]), [Drawing.Imaging.ImageFormat]::Png)
        }
        finally { $graphics.Dispose(); $bitmap.Dispose() }
    }
}
finally { $original.Dispose(); $icon.Dispose() }
$output = Join-Path $folder "NoReinstallHub-$version-unsigned.msix"
& $MakeAppxPath pack /d $payload /p $output
if ($LASTEXITCODE -ne 0) { throw 'makeappx failed; no package has been installed or uploaded.' }
Write-Output "Unsigned local package: $output"
Write-Output 'Partner Center identity, signing, WACK, clean-PC install and Store certification remain pending.'
