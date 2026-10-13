#Requires -Version 5.1
param(
    [Parameter(Mandatory = $true)]
    [string]$ProgramAutoTestsDir
)

$ErrorActionPreference = "Stop"
$all = @()
Get-ChildItem -LiteralPath $ProgramAutoTestsDir -Directory -ErrorAction SilentlyContinue | ForEach-Object {
    $rp = Join-Path $_.FullName "results.json"
    if (Test-Path -LiteralPath $rp) {
        $all += (Get-Content -LiteralPath $rp -Raw | ConvertFrom-Json)
    }
}
$summary = Join-Path $ProgramAutoTestsDir "results-summary.json"
@{ chunks = $all; collected_at = (Get-Date).ToString("o") } | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $summary -Encoding UTF8
Write-Host "Wrote $summary"
