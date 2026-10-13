---
name: vs-executionist-{{NN}}-{{SHORT}}
description: >-
  Vibe Scaler executionist entrypoint for {{PROGRAM_ID}}. Self-contained program pin +
  full execution contract (paths, operators, learning tour, strict checklist, git).
  Use /executionist-{{NN}}-{{SHORT}} in any IDE (Antigravity, Cursor, etc.).
program_id: {{PROGRAM_ID}}
program_root: docs/idea-to-reality-playground/{{PROGRAM_ID}}/
---

# Executionist {{NN}} — {{SHORT}} (entrypoint)

**This skill is the only attachment required.** Follow everything below even if no other rules load.

## Program pin

| Field | Value |
|-------|--------|
| **PROGRAM_ID** | `{{PROGRAM_ID}}` |
| **PROGRAM_ROOT** | `docs/idea-to-reality-playground/{{PROGRAM_ID}}/` |

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
| 8 | `PROGRAM_ROOT/implementation-checklist.md` |
| 9 | `PROGRAM_ROOT/implementation-log.md` |
| 10 | `PROGRAM_ROOT/original-prompts.md` |
| 11 | `PROGRAM_ROOT/operators/index.md` |
| 12 | Active chunk: `PROGRAM_ROOT/plan/chunks/<id>.md` (from index) |
| 13 | `PROGRAM_ROOT/verification/chunks/<chunk-id>.html` |
| 14 | `PROGRAM_ROOT/verification/acceptance-state.json` (if exists) |

## Program-specific guardrails

{{PROGRAM_GUARDRAILS}}

## Full execution contract

Execute **every section** of `.cursor/skills/vs-executionist/SKILL.md` as if invoked here:

- **Operators** — name, `operators/<name>.md`, handoff in `original-prompts.md`
- **Turn A** — simple briefing + external LLM teach prompt (chunk scope only, no pipeline) + up to 5 questions + **proceed** gate
- **Turn B** — implement → write `auto-tests/last-handoff.json` + `spec.json` → announce next skill
- **Turn B′** — **`vs-auto-tests-NN-{{SHORT}}`** (canonical stack; `run-tests.ps1` → `generated-checklist.md`)
- **Jr goat** — always print verdict; optional **`vs-jr-goat-NN-{{SHORT}}`** before checklist (sub-chunks circle back)
- **Turn C** — **immediate checklist in chat** (agent-verified vs human-verify); user must confirm before next chunk
- **Turn D** — **git commit + push origin** only after user explicitly OK post-checklist
- **Turn E** — optional `docs/learning/<name>/report.md`
- **No false done** when checklist empty — checker + `index.md` status required

## Traverse tree

Walk **`implementation-index.md`** → chunk files under **`plan/chunks/`** (not this skill file).

## Voice (mandatory)

Read **`vibe-scaler/VOICE.md`**. Plain language first; paths and statuses second. **Doing · did · next** in chat. User wrote **`pro`** → denser detail.
