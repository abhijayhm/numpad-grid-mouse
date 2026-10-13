---
name: vs-auto-execute
description: >-
  Traverse Vibe Scaler playground — find last handoff, status, half-done programs;
  ask name/role; suggest exact next slash command. Use /vs-auto-execute or
  auto-execute-NN-short per plan.
---

# Auto-execute 00 — traverser

**Context search + next-step router.** No implementation — only read state and recommend.

**Prefer `/vs-hub`** when the user wants a **numbered menu**, onboarding, or the full playground tree. This skill is the lightweight “what next?” shortcut (menu option **7**).

## Mandatory read first

1. `vibe-scaler/SYSTEM-PATHS.md`
2. `vibe-scaler/VOICE.md`
3. `docs/idea-to-reality-playground/index.md`
4. Optional pin: `PROGRAM_ROOT` from message or `vs-auto-execute-NN-<short>` skill

## Ask once per session

- **Name** (operator)
- **Role** (ideator | executionist driver | intern | goat/senior) — infer from message if clear

## Traverse algorithm

```
FOR each program in playground index (newest first):
  READ index.md status, status.json if present
  READ auto-tests/last-handoff.json
  READ implementation-checklist.md active chunk
  READ implementation-log.md last entry
  SCORE incomplete / halfway signals
PICK highest-priority program + next action
```

## Status → suggested command

| Status / signal | Suggest |
|-----------------|--------|
| missing program / ideation | `/vs-ideator` |
| `ideation`, no checker HTML | `/vs-checker-NN-<short>` generate |
| wireframe not approved | `/vs-checker-NN-<short>` refine |
| `executing`, cold / no proceed | `/vs-executionist-NN-<short>` (briefing) |
| handoff `pending` | `/vs-auto-tests-NN-<short>` |
| tests failed | re-run auto-tests; fix chunk |
| `code_review_pending` | `/vs-code-reviewer-NN-<short>` |
| `human_checklist_pending` | open chunk HTML; then checker ingest |
| `goat_verification_pending` | `/vs-goat` |
| `archived` | none — folder under `archives/` |
| `cancelled` / `discarded` | none unless reopen |

## Output format (chat)

```markdown
## Auto-execute
- **You:** <name> (<role>)
- **Program:** NN_short — <status>
- **Last activity:** <from log/handoff>
- **Do next:** `/skill-name` — one line why
- **Optional:** second program if halfway elsewhere
```

## Per-program skill

`vs-auto-execute-NN-<short>` — fixed `PROGRAM_ROOT`; same algorithm scoped to one plan.

## Voice

**`vibe-scaler/VOICE.md`** — one next step, one plain sentence why; translate status/handoff into what the human should do.
