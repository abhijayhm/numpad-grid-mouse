#Requires -Version 5.1
param(
    [Parameter(Mandatory = $true)]
    [string]$ProgramId
)

$ErrorActionPreference = "Stop"
$root = Join-Path (Get-Location) "docs\idea-to-reality-playground"
$src = Join-Path $root $ProgramId
$arch = Join-Path $root "archives"
$dst = Join-Path $arch $ProgramId

if (-not (Test-Path -LiteralPath $src)) { throw "Program not found: $src" }
New-Item -ItemType Directory -Path $arch -Force | Out-Null
if (Test-Path -LiteralPath $dst) { throw "Archive exists: $dst" }
Move-Item -LiteralPath $src -Destination $dst
Write-Host "Archived -> $dst" -ForegroundColor Green
Write-Host "Update playground index.md status=archived and folder link."
