param(
    [Parameter(Mandatory = $true)]
    [string]$Source,
    [Parameter(Mandatory = $true)]
    [string]$Destination,
    [Parameter(Mandatory = $true)]
    [string]$BackupId,
    [Parameter(Mandatory = $true)]
    [string]$KeyOut,
    [string]$Branch = "unknown",
    [string]$Commit = "unknown",
    [string]$Migration = "none"
)

$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $false
$sourceFull = [System.IO.Path]::GetFullPath($Source)
$destinationFull = [System.IO.Path]::GetFullPath($Destination)
if ($destinationFull.StartsWith($sourceFull, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "The backup destination is inside the source directory. Choose a folder outside the repository."
}

$stage = Join-Path ([System.IO.Path]::GetTempPath()) ("clearhaul-stage-" + [guid]::NewGuid().ToString("N"))
New-Item -ItemType Directory -Path $stage | Out-Null
try {
    & robocopy $sourceFull $stage /E /XD bin obj .git node_modules TestResults .vs /NFL /NDL /NJH /NJS /NC /NS /NP | Out-Null
    if ($LASTEXITCODE -ge 8) {
        throw "Robocopy failed with exit code $LASTEXITCODE."
    }

    $git = Get-Command git -ErrorAction SilentlyContinue
    if ($null -ne $git -and (Test-Path (Join-Path $sourceFull ".git"))) {
        $recovery = Join-Path $stage "recovery"
        New-Item -ItemType Directory -Path $recovery | Out-Null
        & git -C $sourceFull bundle create (Join-Path $recovery "repository.bundle") --all
        if ($LASTEXITCODE -ne 0) {
            throw "git bundle failed with exit code $LASTEXITCODE."
        }
    }

    $repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
    $cli = Join-Path $repoRoot "tools\ClearHaul.BackupCli\ClearHaul.BackupCli.csproj"
    & dotnet run --project $cli --configuration Release --no-launch-profile -- create `
        --source $stage `
        --dest $destinationFull `
        --id $BackupId `
        --branch $Branch `
        --commit $Commit `
        --key-out $KeyOut `
        --migration $Migration
    if ($LASTEXITCODE -ne 0) {
        throw "Backup create failed with exit code $LASTEXITCODE."
    }
}
finally {
    if (Test-Path $stage) {
        Remove-Item -LiteralPath $stage -Recurse -Force
    }
}
