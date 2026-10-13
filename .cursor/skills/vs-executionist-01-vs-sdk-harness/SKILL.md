---
name: vs-executionist-01-vs-sdk-harness
description: >-
  Vibe Scaler executionist for 01_vs-sdk-harness. Python vibe-scaler-sdk harness.
  Use /vs-executionist-01-vs-sdk-harness.
program_id: 01_vs-sdk-harness
program_root: docs/idea-to-reality-playground/01_vs-sdk-harness/
---

# Executionist 01 — vs-sdk-harness

**PROGRAM_ROOT** = `docs/idea-to-reality-playground/01_vs-sdk-harness/`

## Program-specific guardrails

- Implement only under **`vibe-scaler/vibe-scaler-sdk/`** and **`docs/vibe-scaler/history/`** unless chunk says otherwise.
- Load **`CURSOR_API_KEY`** via `vibe_scaler_sdk.config` from repo-root **`.env`** — never commit or log the key.
- **No VS Code extension** in this program (Plan 2).
- Follow **`vibe-scaler/VOICE.md`** — executionist executes; orchestrator decides.

## Mandatory read

`vibe-scaler/SYSTEM-PATHS.md`, `vibe-scaler/VOICE.md`, `PROGRAM_ROOT/implementation-index.md`, active chunk under `plan/chunks/`.

## Full contract

Execute all sections of `.cursor/skills/vs-executionist/SKILL.md` (Turn A → proceed → B → checklist → git only on yes).

Walk **`implementation-index.md`** chunk order c01 → c10.
