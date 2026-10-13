# Runs one named target from scripts/dev-terminals/targets.json
param([Parameter(Mandatory = $true)][string]$Target)
$ErrorActionPreference = "Stop"
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$repo = Split-Path -Parent (Split-Path -Parent $here)
$cwdFile = Join-Path $repo ".vibe-scaler\terminal-cwd.local"
if (Test-Path -LiteralPath $cwdFile) {
    $cwd = (Get-Content -LiteralPath $cwdFile -Raw).Trim()
    if ($cwd) { Set-Location -LiteralPath $cwd }
}
$targets = Get-Content -LiteralPath (Join-Path $here "targets.json") -Raw | ConvertFrom-Json
$cmd = $targets.targets.$Target.command
if (-not $cmd) { Write-Error "No target '$Target' in targets.json"; exit 1 }
Write-Host ">> $Target in $(Get-Location)"
Invoke-Expression $cmd
