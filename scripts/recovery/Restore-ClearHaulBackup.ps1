param(
    [Parameter(Mandatory = $true)]
    [string]$Manifest,
    [Parameter(Mandatory = $true)]
    [string]$Destination,
    [Parameter(Mandatory = $true)]
    [string]$Key
)

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$cli = Join-Path $repoRoot "tools\ClearHaul.BackupCli\ClearHaul.BackupCli.csproj"
& dotnet run --project $cli --configuration Release --no-launch-profile -- restore `
    --manifest ([System.IO.Path]::GetFullPath($Manifest)) `
    --dest ([System.IO.Path]::GetFullPath($Destination)) `
    --key ([System.IO.Path]::GetFullPath($Key))
if ($LASTEXITCODE -ne 0) {
    throw "Backup restore failed with exit code $LASTEXITCODE."
}
