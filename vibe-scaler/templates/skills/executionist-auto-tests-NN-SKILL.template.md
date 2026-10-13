---
name: vs-auto-tests-{{NN}}-{{SHORT}}
description: >-
  Auto-tests entrypoint for {{PROGRAM_ID}}. Continues from executionist handoff;
  pytest Playwright Detox Flutter Vitest filesystem. Self-contained paths.
program_id: {{PROGRAM_ID}}
program_root: docs/idea-to-reality-playground/{{PROGRAM_ID}}/
---

# Executionist auto-tests {{NN}} — {{SHORT}} (entrypoint)

**PROGRAM_ROOT** = `docs/idea-to-reality-playground/{{PROGRAM_ID}}/`

## Mandatory read first

1. `vibe-scaler/VOICE.md`
2. `PROGRAM_ROOT/auto-tests/last-handoff.json`
3. `vibe-scaler/skills/vs-auto-tests/SKILL.md` — execute **all** steps
4. `.cursor/custom-rules/vs-auto-tests-governance.mdc`

## Handoff

If `last-handoff.json` missing → tell user to run **`/executionist-{{NN}}-{{SHORT}}`** Turn B first or paste chunk id.

## Kit

`vibe-scaler/vs-auto-tests/run-tests.ps1` + `generate-checklist.ps1`
