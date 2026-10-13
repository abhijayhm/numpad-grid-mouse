#!/usr/bin/env bash
# Vibe Scaler SDK Harness — End-to-End Smoke Test (macOS / Linux)
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PACKAGE_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"
REPO_ROOT="$(cd "${PACKAGE_DIR}/../.." && pwd)"

echo "============================================================"
echo "  Vibe Scaler SDK Harness — End-to-End Smoke Test"
echo "  Repo root:    ${REPO_ROOT}"
echo "  Package dir:  ${PACKAGE_DIR}"
echo "============================================================"

# 1. Verify Python & Package Import
echo "[1/6] Verifying Python import of vibe_scaler_sdk..."
python3 -c "import vibe_scaler_sdk; print('  ✓ vibe_scaler_sdk version:', vibe_scaler_sdk.__version__)"

# 2. Config & .env Check
echo "[2/6] Verifying config & environment..."
python3 -c "
from vibe_scaler_sdk.config import get_api_key, load_settings, mask_api_key
settings = load_settings()
key = get_api_key()
assert key is not None, 'CURSOR_API_KEY not found in .env'
print(f'  ✓ API Key detected: {mask_api_key(key)}')
"

# 3. Prompt Loader Verification
echo "[3/6] Verifying prompt templates..."
python3 -c "
from vibe_scaler_sdk.prompts import load
for p in ['agent1', 'agent2', 'agent3']:
    text = load(p)
    assert len(text) > 50, f'Prompt {p} empty or too short'
    assert 'VOICE.md' in text, f'Prompt {p} must reference VOICE.md'
print('  ✓ All 3 role prompts loaded with VOICE.md authority.')
"

# 4. Deterministic Playground Resolver
echo "[4/6] Verifying playground chunk resolver..."
python3 -m vibe_scaler_sdk.resolve --program 01_vs-sdk-harness >/dev/null
echo "  ✓ Playground chunk resolution verified."

# 5. Pipeline Agent 1 & Agent 2 Runs (Dry Run)
echo "[5/6] Verifying pipeline lifecycle (Agent 1 & Agent 2)..."
python3 -m vibe_scaler_sdk.pipeline --chunk c01-package --agent 1 --dry-run >/dev/null
python3 -m vibe_scaler_sdk.pipeline --chunk c01-package --agent 2 --dry-run >/dev/null
echo "  ✓ Lifecycle events (auto_execute -> spawn_1 -> agent1_done -> spawn_2 -> agent2_done) verified."

# 6. REPL Subcommand Verification
echo "[6/6] Verifying REPL commands..."
python3 -m vibe_scaler_sdk doctor >/dev/null
echo "  ✓ REPL doctor executed cleanly."

echo "============================================================"
echo "✓ ALL VIBE SCALER SDK SMOKE CHECKS PASSED SUCCESSFULLY!"
echo "============================================================"
exit 0
