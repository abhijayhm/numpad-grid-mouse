#Requires -Version 5.1
param(
    [Parameter(Mandatory = $true)]
    [string]$ResultsPath
)

$ErrorActionPreference = "Stop"
$r = Get-Content -LiteralPath $ResultsPath -Raw | ConvertFrom-Json
$outDir = Split-Path -Parent $ResultsPath
$mdPath = Join-Path $outDir "generated-checklist.md"

$lines = @(
    "# Generated checklist — chunk $($r.chunk_id)",
    "",
    "| Field | Value |",
    "|-------|--------|",
    "| Program | ``$($r.program_id)`` |",
    "| Target | $($r.target) / $($r.profile) |",
    "| Agent run passed | **$($r.passed)** |",
    "",
    "## Commands",
    ""
)
foreach ($run in @($r.runs)) {
    $status = if ($run.exit_code -eq 0) { "OK" } else { "FAIL" }
    $lines += "- [$(if ($status -eq 'OK') { 'x' } else { ' ' })] ``$($run.command)`` ($status)"
}
$lines += ""
$lines += "## Filesystem"
$lines += ""
foreach ($f in @($r.filesystem)) {
    $lines += "- [$(if ($f.ok) { 'x' } else { ' ' })] $($f.path) — $($f.expect)"
}
$lines += ""
$lines += "## Human-verify (from acceptance HTML)"
$lines += ""
$lines += "_Items requiring device, login, or visual review — tick in verification/chunks HTML._"
$lines += ""

Set-Content -LiteralPath $mdPath -Value ($lines -join "`n") -Encoding UTF8
Write-Host "Wrote $mdPath"
