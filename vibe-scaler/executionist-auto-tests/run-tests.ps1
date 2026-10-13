#Requires -Version 5.1
param(
    [Parameter(Mandatory = $true)]
    [string]$SpecPath
)

$ErrorActionPreference = "Stop"
$kitRoot = $PSScriptRoot
$spec = Get-Content -LiteralPath $SpecPath -Raw | ConvertFrom-Json

$profileId = if ($spec.profile_override) { $spec.profile_override } else { $spec.target }
$profilePath = Join-Path $kitRoot "profiles\$profileId.json"
if (-not (Test-Path -LiteralPath $profilePath)) {
    throw "Unknown profile/target: $profileId"
}
$profile = Get-Content -LiteralPath $profilePath -Raw | ConvertFrom-Json

$runDir = Split-Path -Parent $SpecPath
$resultsPath = Join-Path $runDir "results.json"
$started = (Get-Date).ToString("o")

$runs = @()
foreach ($cmd in @($spec.verify_commands)) {
    if ([string]::IsNullOrWhiteSpace($cmd)) { continue }
    Write-Host ">> $cmd" -ForegroundColor Cyan
    $out = & powershell -NoProfile -Command $cmd 2>&1 | Out-String
    $code = $LASTEXITCODE
    $runs += @{ command = $cmd; exit_code = $code; output = $out.Substring(0, [Math]::Min(8000, $out.Length)) }
    if ($code -ne 0) { break }
}

if ($profile.default_run -and ($runs.Count -eq 0 -or ($runs[-1].exit_code -eq 0))) {
    Write-Host ">> $($profile.default_run)" -ForegroundColor Cyan
    $out = & powershell -NoProfile -Command $profile.default_run 2>&1 | Out-String
    $runs += @{ command = $profile.default_run; exit_code = $LASTEXITCODE; output = $out.Substring(0, [Math]::Min(8000, $out.Length)) }
}

# Filesystem assertions
$fs = @()
foreach ($a in @($spec.filesystem_assertions)) {
    $p = $a.path
    $ok = $false
    $detail = ""
    switch ($a.expect) {
        "exists" { $ok = Test-Path -LiteralPath $p; $detail = "exists=$ok" }
        "not_empty" { $ok = (Test-Path -LiteralPath $p) -and ((Get-Item -LiteralPath $p).Length -gt 0); $detail = "not_empty=$ok" }
        "contains" {
            if (Test-Path -LiteralPath $p) {
                $ok = (Get-Content -LiteralPath $p -Raw) -match $a.value
                $detail = "contains=$ok"
            }
        }
    }
    $fs += @{ path = $p; expect = $a.expect; ok = $ok; detail = $detail }
}

$allOk = ($runs | Where-Object { $_.exit_code -ne 0 }).Count -eq 0 -and ($fs | Where-Object { -not $_.ok }).Count -eq 0

$result = @{
    program_id = $spec.program_id
    chunk_id   = $spec.chunk_id
    target     = $spec.target
    profile    = $profileId
    started_at = $started
    finished_at = (Get-Date).ToString("o")
    passed     = $allOk
    runs       = $runs
    filesystem = $fs
}
$result | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $resultsPath -Encoding UTF8
Write-Host "Wrote $resultsPath (passed=$allOk)" -ForegroundColor $(if ($allOk) { "Green" } else { "Red" })
exit $(if ($allOk) { 0 } else { 1 })
