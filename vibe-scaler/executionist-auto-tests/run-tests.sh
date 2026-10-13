#!/usr/bin/env bash
set -euo pipefail
SPEC="${1:?spec.json path required}"
ROOT="$(cd "$(dirname "$0")" && pwd)"
if command -v pwsh >/dev/null 2>&1; then
  exec pwsh -NoProfile -File "$ROOT/run-tests.ps1" -SpecPath "$SPEC"
fi
powershell -NoProfile -File "$ROOT/run-tests.ps1" -SpecPath "$SPEC"
