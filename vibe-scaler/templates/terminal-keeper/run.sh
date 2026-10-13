#!/usr/bin/env bash
# Runs one named target from scripts/dev-terminals/targets.json
set -euo pipefail
TARGET="${1:?target name required}"
HERE="$(cd "$(dirname "$0")" && pwd)"
REPO="$(cd "$HERE/../.." && pwd)"
CWD_FILE="$REPO/.vibe-scaler/terminal-cwd.local"
if [[ -f "$CWD_FILE" ]]; then
  CWD="$(tr -d '\r' < "$CWD_FILE" | head -n 1)"
  if [[ -n "${CWD}" ]]; then
    cd "$CWD"
  fi
fi
CMD="$(python - "$HERE/targets.json" "$TARGET" <<'PY'
import json, sys
path, name = sys.argv[1], sys.argv[2]
data = json.load(open(path, encoding="utf-8"))
cmd = (data.get("targets") or {}).get(name, {}).get("command")
if not cmd:
    raise SystemExit("No target " + name)
print(cmd)
PY
)"
echo ">> $TARGET in $(pwd)"
exec bash -lc "$CMD"
