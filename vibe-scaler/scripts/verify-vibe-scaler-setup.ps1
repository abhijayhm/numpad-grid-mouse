#Requires -Version 5.1
<#
.SYNOPSIS
  Verify Vibe Scaler + multi-IDE agent mirrors (Cursor, Antigravity, Claude Code, Codex).

.DESCRIPTION
  Simulates what each IDE should see without launching external products.
  Exit 0 = all required checks passed; 1 = failures (see report).
#>
param(
    [string]$ProjectRoot = "",
    [switch]$Json,
    [switch]$FixSync
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($ProjectRoot)) {
    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
    $kitRoot = Split-Path -Parent $scriptDir
    $ProjectRoot = Split-Path -Parent $kitRoot
}

$ProjectRoot = (Resolve-Path -LiteralPath $ProjectRoot).Path

function Test-PathReport {
    param([string]$Label, [string]$Path, [bool]$Required = $true)
    $exists = Test-Path -LiteralPath $Path
    return @{
        label    = $Label
        path     = $Path
        required = $Required
        ok       = $exists -or (-not $Required)
        exists   = $exists
    }
}

function Test-FileContains {
    param([string]$Path, [string]$Pattern)
    if (-not (Test-Path -LiteralPath $Path)) { return $false }
    $c = Get-Content -LiteralPath $Path -Raw -ErrorAction SilentlyContinue
    return ($c -match $Pattern)
}

$script:results = @()
$script:failures = 0

$vibeKit = Join-Path $ProjectRoot "vibe-scaler"
$cursor = Join-Path $ProjectRoot ".cursor"
$claude = Join-Path $ProjectRoot ".claude"
$agents = Join-Path $ProjectRoot ".agents"
$agentsMd = Join-Path $ProjectRoot "AGENTS.md"

$requiredKitSkills = @(
    "vs-hub",
    "vs-help-teach",
    "vs-help-llm",
    "vs-terminal-keeper",
    "vs-jr-goat",
    "vs-performance",
    "vs-ideator",
    "vs-checker",
    "vs-executionist",
    "vs-planner",
    "vs-harness",
    "vs-auto-tests"
)

$requiredRules = @(
    "vibe-scaler-rule.mdc",
    "vibe-scaler-hub-governance.mdc",
    "ideator-governance.mdc",
    "ideator-checker-governance.mdc",
    "executionist-governance.mdc",
    "executionist-monitor.mdc"
)

function Add-Check {
    param([hashtable]$Check)
    $script:results += $Check
    if ($Check.required -and -not $Check.ok) { $script:failures++ }
}

Add-Check (Test-PathReport "vibe-scaler kit" $vibeKit)
Add-Check (Test-PathReport "SYSTEM-PATHS.md" (Join-Path $vibeKit "SYSTEM-PATHS.md"))
Add-Check (Test-PathReport ".cursor/" $cursor)

foreach ($skill in $requiredKitSkills) {
    $kitSkill = Join-Path $vibeKit "skills\$skill\SKILL.md"
    Add-Check (Test-PathReport "kit skill $skill" $kitSkill)
}

foreach ($rule in $requiredRules) {
    $pCursor = Join-Path $cursor "custom-rules\$rule"
    $pKit = Join-Path $vibeKit "rules\$rule"
    $exists = (Test-Path -LiteralPath $pCursor) -or (Test-Path -LiteralPath $pKit)
    Add-Check @{
        label = "rule $rule (.cursor or vibe-scaler/rules)"
        path = if (Test-Path -LiteralPath $pCursor) { $pCursor } else { $pKit }
        required = $true
        ok = $exists
        exists = $exists
    }
}

# Cursor: harness installed (utils repo may only have harness in .cursor after manual add)
$cursorHarness = Join-Path $cursor "skills\vs-harness\SKILL.md"
Add-Check (Test-PathReport "cursor harness skill" $cursorHarness)

# Self-sustained NN template
$execTpl = Join-Path $vibeKit "templates\skills\executionist-NN-SKILL.template.md"
Add-Check (Test-PathReport "executionist NN template" $execTpl)
if (Test-Path -LiteralPath $execTpl) {
    $selfSustained = Test-FileContains $execTpl "Mandatory read first"
    Add-Check @{
        label = "NN executionist template self-contained"
        path  = $execTpl
        required = $true
        ok = $selfSustained
        exists = $true
    }
}

Add-Check (Test-PathReport "ideator.html template" (Join-Path $vibeKit "templates\verification\ideator.html.template"))
Add-Check (Test-PathReport "goat-planner-tmp template" (Join-Path $vibeKit "templates\goat-planner-tmp\README.md"))
Add-Check (Test-PathReport "auto-tests kit" (Join-Path $vibeKit "executionist-auto-tests\run-tests.ps1"))

# --- Simulate Antigravity (.agents/) ---
Add-Check (Test-PathReport "Antigravity .agents/" $agents $false)
foreach ($skill in @("vs-hub", "vs-executionist", "vs-ideator", "vs-performance", "vs-harness")) {
    $p = Join-Path $agents "skills\$skill\SKILL.md"
    $r = Test-PathReport "Antigravity skill $skill" $p $false
    if (-not $r.exists -and (Test-Path -LiteralPath (Join-Path $cursor "skills\$skill\SKILL.md"))) {
        $r.ok = $false
        $r.required = $true
        $r.label = "Antigravity mirror $skill (run sync-agent-config)"
    }
    Add-Check $r
}

$agRule = Join-Path $agents "rules\executionist-governance.md"
Add-Check (Test-PathReport "Antigravity executionist-governance" $agRule $false)

# Antigravity: executionist skill body length / entrypoint keywords (simulates thin-skill failure)
$agExec = Join-Path $agents "skills\vs-executionist\SKILL.md"
if (Test-Path -LiteralPath $agExec) {
    $lines = (Get-Content -LiteralPath $agExec).Count
    $hasPaths = Test-FileContains $agExec "SYSTEM-PATHS"
    $hasProceed = Test-FileContains $agExec "proceed"
    Add-Check @{
        label = "Antigravity vs-executionist substantive (lines>=40, paths, proceed)"
        path = $agExec
        required = $true
        ok = ($lines -ge 40 -and $hasPaths -and $hasProceed)
        exists = $true
    }
}

# --- Simulate Claude Code (.claude/) ---
foreach ($skill in @("vs-hub", "vs-executionist", "vs-ideator", "vs-performance", "vs-harness")) {
    $p = Join-Path $claude "skills\$skill\SKILL.md"
    $r = Test-PathReport "Claude skill $skill" $p $false
    if (-not $r.exists -and (Test-Path -LiteralPath (Join-Path $cursor "skills\$skill\SKILL.md"))) {
        $r.required = $true
        $r.ok = $false
    }
    Add-Check $r
}

# Claude SDK note: skills are filesystem-only; mirror must exist
$claudeExec = Join-Path $claude "skills\vs-executionist\SKILL.md"
if (Test-Path -LiteralPath $claudeExec) {
    Add-Check @{
        label = "Claude mirror matches entrypoint contract (Turn A / immediate checklist)"
        path = $claudeExec
        required = $true
        ok = (Test-FileContains $claudeExec "Immediate checklist")
        exists = $true
    }
}

# --- Simulate Codex (AGENTS.md) ---
Add-Check (Test-PathReport "Codex AGENTS.md" $agentsMd)
if (Test-Path -LiteralPath $agentsMd) {
    Add-Check @{
        label = "AGENTS.md mentions Vibe Scaler"
        path = $agentsMd
        required = $true
        ok = (Test-FileContains $agentsMd "Vibe Scaler")
        exists = $true
    }
    Add-Check @{
        label = "AGENTS.md mentions vs-executionist"
        path = $agentsMd
        required = $true
        ok = (Test-FileContains $agentsMd "vs-executionist")
        exists = $true
    }
    Add-Check @{
        label = "AGENTS.md mentions planner-tree-curator"
        path = $agentsMd
        required = $true
        ok = (Test-FileContains $agentsMd "vs-planner")
        exists = $true
    }
    Add-Check @{
        label = "AGENTS.md mentions /vs-hub hub"
        path = $agentsMd
        required = $true
        ok = (Test-FileContains $agentsMd "/vs-hub")
        exists = $true
    }
    Add-Check @{
        label = "AGENTS.md mentions plain menu or mermaid fallback (VOICE)"
        path = $agentsMd
        required = $true
        ok = ((Test-FileContains $agentsMd "plain numbered menu") -or (Test-FileContains $agentsMd "do not render mermaid"))
        exists = $true
    }
}

# Verify script itself
Add-Check (Test-PathReport "verify script" (Join-Path $vibeKit "scripts\verify-vibe-scaler-setup.ps1"))

if ($FixSync) {
    $syncBat = Join-Path $ProjectRoot "bin\sync-agent-config.bat"
    if (Test-Path -LiteralPath $syncBat) {
        Write-Host "Running sync-agent-config..." -ForegroundColor Cyan
        & $syncBat
    }
}

$report = @{
    project_root = $ProjectRoot
    timestamp    = (Get-Date).ToString("o")
    passed       = ($script:failures -eq 0)
    failure_count = $script:failures
    checks       = $script:results
    references   = @{
        cursor_skills = "https://cursor.com/docs/skills"
        antigravity_skills = "https://antigravity.google/docs/skills/"
        claude_sdk_skills = "https://code.claude.com/docs/en/agent-sdk/skills"
        codex_agents_md = "https://developers.openai.com/codex/guides/agents-md"
    }
}

if ($Json) {
    $report | ConvertTo-Json -Depth 6
} else {
    Write-Host ""
    Write-Host "vibe-scaler setup verify" -ForegroundColor White
    Write-Host "=========================" -ForegroundColor White
    Write-Host "Root: $ProjectRoot"
    foreach ($c in $script:results) {
        $icon = if ($c.ok) { "[OK]" } else { "[FAIL]" }
        $color = if ($c.ok) { "Green" } else { "Red" }
        Write-Host "  $icon $($c.label)" -ForegroundColor $color
        if (-not $c.ok) { Write-Host "       $($c.path)" -ForegroundColor DarkGray }
    }
    Write-Host ""
    if ($script:failures -eq 0) {
        Write-Host "All required checks passed." -ForegroundColor Green
    } else {
        Write-Host "$($script:failures) check(s) failed. Run: bin\sync-agent-config.bat  then re-run with -FixSync" -ForegroundColor Yellow
    }
    Write-Host ""
    Write-Host "Docs: Cursor skills, Antigravity .agents/skills, Claude SDK settingSources+skills, Codex AGENTS.md" -ForegroundColor DarkGray
}

# Utils repo: vibe-scaler 00 skills live in kit until ide-init copies to .cursor
$cursorExec = Join-Path $cursor "skills\vs-executionist\SKILL.md"
if (-not (Test-Path -LiteralPath $cursorExec) -and (Test-Path -LiteralPath (Join-Path $vibeKit "skills\vs-executionist\SKILL.md"))) {
    Write-Host "  [WARN] vs-executionist not in .cursor/skills (kit only). Run ide-init vibe-scaler y for full Cursor install." -ForegroundColor Yellow
}

exit $(if ($script:failures -eq 0) { 0 } else { 1 })
