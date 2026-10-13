#Requires -Version 5.1
param(
    [switch]$Force,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$Paths
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$pyScript = Join-Path $scriptDir "graphify-update-changed.py"

if (-not (Test-Path -LiteralPath $pyScript)) {
    Write-Host "ERROR: graphify-update-changed.py not found: $pyScript" -ForegroundColor Red
    exit 1
}

if (-not $Paths -or $Paths.Count -eq 0) {
    Write-Host "usage: graphify-update-changed.ps1 [-Force] <file> [file ...]" -ForegroundColor Yellow
    exit 2
}

$python = Get-Command python -ErrorAction SilentlyContinue
if (-not $python) { $python = Get-Command py -ErrorAction SilentlyContinue }
if (-not $python) {
    Write-Host "ERROR: Python not found." -ForegroundColor Red
    exit 1
}

$args = @($pyScript)
if ($Force) { $args += "--force" }
$args += $Paths

& $python.Source @args
exit $LASTEXITCODE
