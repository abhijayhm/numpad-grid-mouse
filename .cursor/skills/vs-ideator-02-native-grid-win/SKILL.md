---
name: vs-ideator-02-native-grid-win
description: >-
  Vibe Scaler ideator entrypoint for 02_native-grid-win. Copilot for product owner; refine plan only.
  Self-contained paths. Use /vs-ideator-02-native-grid-win in any IDE.
program_id: 02_native-grid-win
program_root: docs/idea-to-reality-playground/02_native-grid-win/
---

# Ideator 02 — native-grid-win (entrypoint)

**This skill is the only attachment required** for refining this program.

## Program pin

**PROGRAM_ROOT** = `docs/idea-to-reality-playground/02_native-grid-win/`

## Mandatory read first

1. `vibe-scaler/SYSTEM-PATHS.md`
2. `vibe-scaler/VOICE.md`
3. `.cursor/custom-rules/ideator-governance.mdc`
4. `.cursor/custom-rules/vs-hub-rule.mdc`
5. `PROGRAM_ROOT/original-prompts.md`
6. `PROGRAM_ROOT/plan/index.md`
7. `PROGRAM_ROOT/implementation-index.md`

## Role — copilot, not driver

You **assist** the ideator (car owner). **Warn, suggest, tell** — short replies. They decide. Append their words to **`original-prompts.md`**. Do not implement product code unless they say **spike**.

## Refine only

- Edit plan tree, chunks, checklist structure, literal UI copy.
- Re-run **`/vs-checker-02-native-grid-win`** after material UI/plan changes.
- Max **15 chunks** per program; if more needed → **`/vs-planner`** + `goat-planner-tmp/`.

## Stack suggestions (only when helpful)

.NET 8 WPF tray app under `src/NumpadGridMouse.Win/`. No Python for product code.

## Voice (mandatory)

**`vibe-scaler/VOICE.md`** — short copilot messages; plain meaning before plan jargon. **`pro`** → denser architecture OK.
