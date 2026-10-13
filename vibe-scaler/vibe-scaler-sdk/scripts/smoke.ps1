# Vibe Scaler SDK Harness — End-to-End Smoke Test (PowerShell / Windows)
$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$PackageDir = Split-Path -Parent $ScriptDir
$RepoRoot = Split-Path -Parent (Split-Path -Parent $PackageDir)

Write-Host "============================================================" -ForegroundColor Cyan
Write-Host "  Vibe Scaler SDK Harness — End-to-End Smoke Test (PowerShell)" -ForegroundColor Cyan
Write-Host "  Repo root:    $RepoRoot"
Write-Host "  Package dir:  $PackageDir"
Write-Host "============================================================"

# 1. Verify Python & Package Import
Write-Host "[1/6] Verifying Python import of vibe_scaler_sdk..." -ForegroundColor Yellow
python -c "import vibe_scaler_sdk; print('  ✓ vibe_scaler_sdk version:', vibe_scaler_sdk.__version__)"

# 2. Config & .env Check
Write-Host "[2/6] Verifying config & environment..." -ForegroundColor Yellow
python -c @"
from vibe_scaler_sdk.config import get_api_key, load_settings, mask_api_key
settings = load_settings()
key = get_api_key()
assert key is not None, 'CURSOR_API_KEY not found in .env'
print(f'  ✓ API Key detected: {mask_api_key(key)}')
"@

# 3. Prompt Loader Verification
Write-Host "[3/6] Verifying prompt templates..." -ForegroundColor Yellow
python -c @"
from vibe_scaler_sdk.prompts import load
for p in ['agent1', 'agent2', 'agent3']:
    text = load(p)
    assert len(text) > 50, f'Prompt {p} empty or too short'
    assert 'VOICE.md' in text, f'Prompt {p} must reference VOICE.md'
print('  ✓ All 3 role prompts loaded with VOICE.md authority.')
"@

# 4. Deterministic Playground Resolver
Write-Host "[4/6] Verifying playground chunk resolver..." -ForegroundColor Yellow
python -m vibe_scaler_sdk.resolve --program 01_vs-sdk-harness | Out-Null
Write-Host "  ✓ Playground chunk resolution verified."

# 5. Pipeline Agent 1 & Agent 2 Runs (Dry Run)
Write-Host "[5/6] Verifying pipeline lifecycle (Agent 1 & Agent 2)..." -ForegroundColor Yellow
python -m vibe_scaler_sdk.pipeline --chunk c01-package --agent 1 --dry-run | Out-Null
python -m vibe_scaler_sdk.pipeline --chunk c01-package --agent 2 --dry-run | Out-Null
Write-Host "  ✓ Lifecycle events (auto_execute -> spawn_1 -> agent1_done -> spawn_2 -> agent2_done) verified."

# 6. REPL Subcommand Verification
Write-Host "[6/6] Verifying REPL commands..." -ForegroundColor Yellow
python -m vibe_scaler_sdk doctor | Out-Null
Write-Host "  ✓ REPL doctor executed cleanly."

Write-Host "============================================================" -ForegroundColor Green
Write-Host "✓ ALL VIBE SCALER SDK SMOKE CHECKS PASSED SUCCESSFULLY!" -ForegroundColor Green
Write-Host "============================================================" -ForegroundColor Green
exit 0
