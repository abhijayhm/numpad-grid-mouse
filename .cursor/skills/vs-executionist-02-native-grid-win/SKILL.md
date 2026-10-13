---
name: vs-executionist-02-native-grid-win
description: >-
  Vibe Scaler executionist entrypoint for 02_native-grid-win. Self-contained program pin +
  full execution contract (paths, operators, learning tour, strict checklist, git).
  Use /vs-executionist-02-native-grid-win in any IDE (Antigravity, Cursor, etc.).
program_id: 02_native-grid-win
program_root: docs/idea-to-reality-playground/02_native-grid-win/
---

# Executionist 02 — native-grid-win (entrypoint)

**This skill is the only attachment required.** Follow everything below even if no other rules load.

## Program pin

| Field | Value |
|-------|--------|
| **PROGRAM_ID** | `02_native-grid-win` |
| **PROGRAM_ROOT** | `docs/idea-to-reality-playground/02_native-grid-win/` |

## Mandatory read first (paths)

| # | Path |
|---|------|
| 1 | `vibe-scaler/SYSTEM-PATHS.md` |
| 2 | `vibe-scaler/VOICE.md` |
| 3 | `.cursor/custom-rules/executionist-governance.mdc` |
| 4 | `.cursor/custom-rules/executionist-monitor.mdc` |
| 5 | `.cursor/custom-rules/vs-hub-rule.mdc` |
| 6 | `PROGRAM_ROOT/index.md` |
| 7 | `PROGRAM_ROOT/implementation-index.md` |
| 8 | `PROGRAM_ROOT/plan/autonomous-execution.md` |
| 9 | `PROGRAM_ROOT/implementation-checklist.md` |
| 10 | `PROGRAM_ROOT/implementation-log.md` |
| 11 | `PROGRAM_ROOT/original-prompts.md` |
| 12 | Active chunk: `PROGRAM_ROOT/plan/chunks/<id>.md` |
| 13 | `PROGRAM_ROOT/verification/chunks/<chunk-id>.html` |

## Program-specific guardrails

- **Product code only** under `v2/src/NumpadGridMouse.Win/` (+ `v2/src/NumpadGridMouse.Win.Tests/`).
- **WPF** for overlay, HUD, settings; **NotifyIcon** tray; **RegisterHotKey** (no admin).
- Hotkeys: **`Ctrl+Shift+G`** grid, **`Ctrl+Shift+A`** arrow mode — see `plan/modes-and-hotkeys.md`.
- **Autonomous batches:** when user says `Run batch X autonomous` or references `autonomous-execution.md`, skip per-chunk Turn A **proceed**; one checklist per batch.
- Port grid logic from `grid_model.py`; do not delete Python until chunk 12 marks legacy in README.
- Mutual exclusion: grid vs arrow mode per plan.
- Literal UI strings from `plan/ui-literal-copy.md`.

## Full execution contract

Execute **every section** of `.cursor/skills/vs-executionist/SKILL.md` as if invoked here.

## Traverse tree

Walk **`implementation-index.md`** → chunk files under **`plan/chunks/`**.

## Voice (mandatory)

Read **`vibe-scaler/VOICE.md`**. Plain language first; paths and statuses second. **Doing · did · next** in chat. User wrote **`pro`** → denser detail.
