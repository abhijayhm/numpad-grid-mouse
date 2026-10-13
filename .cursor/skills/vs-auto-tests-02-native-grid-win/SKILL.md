---
name: vs-auto-tests-02-native-grid-win
description: >-
  Auto-tests entrypoint for 02_native-grid-win. Continues from executionist handoff;
  dotnet test + filesystem. Self-contained paths.
program_id: 02_native-grid-win
program_root: docs/idea-to-reality-playground/02_native-grid-win/
---

# Executionist auto-tests 02 — native-grid-win (entrypoint)

**PROGRAM_ROOT** = `docs/idea-to-reality-playground/02_native-grid-win/`

## Mandatory read first

1. `vibe-scaler/VOICE.md`
2. `PROGRAM_ROOT/auto-tests/last-handoff.json`
3. `vibe-scaler/skills/vs-auto-tests/SKILL.md`
4. `.cursor/custom-rules/vs-auto-tests-governance.mdc`

## Kit

`vibe-scaler/executionist-auto-tests/run-tests.ps1` — profile `backend` or `filesystem` per chunk.

Default verify: `dotnet test src/NumpadGridMouse.Win.Tests -c Release`
