---
name: vs-ideator
description: >-
  Vibe Scaler ideator 00 — entrypoint for new programs. Copilot for owner; ≤15 executable
  chunks or planner split; stack hints from project-utils; generates self-sustained NN skills.
  Read vibe-scaler/SYSTEM-PATHS.md. Use /vs-ideator.
---

# Ideator 00 — base (entrypoint)

You **assist** the ideator (owner). **Warn, suggest, tell** — keep flow small. They drive decisions. Append every user message to **`PROGRAM_ROOT/original-prompts.md`**.

## Mandatory read first

1. `vibe-scaler/SYSTEM-PATHS.md`
2. `vibe-scaler/VOICE.md`
3. `.cursor/custom-rules/ideator-governance.mdc`
4. `.cursor/custom-rules/vs-hub-rule.mdc`
5. `docs/idea-to-reality-playground/index.md`

## Skill numbering

| **00** | This skill |
| **01+** | `vs-ideator-NN-<short>` — refine one program (`NN` = folder prefix) |

## Trigger

`/vs-ideator` + user idea (any length).

## Before creating a program

| Condition | Action |
|-----------|--------|
| Idea needs **> 15 executable chunks** or is huge end-to-end | Run **`/vs-planner`** first → `goat-planner-tmp/` → multiple ideator runs from `split-index.md` |
| ≤ 15 chunks | Continue below |

Each chunk = **one executionist scope** (fits agent context + verify commands).

## Steps (autonomous)

1. Read playground `index.md` — next `NN` (zero-pad 2 digits).
2. `<short-title>` kebab-case (≤24 chars).
3. Ensure playground hub exists (once per repo): copy `vibe-scaler/templates/playground-CONTEXT.md` → `docs/idea-to-reality-playground/CONTEXT.md`, `playground-quick-index.md` → `quick-index.md`, create **`users/`** with `_template/` if missing.
4. Create **`docs/idea-to-reality-playground/NN_<short>/`** from `vibe-scaler/templates/idea-folder/` including **`context/summary.md`**, **`performance/scores.json`**, **`operators/`** (`index.md` + `_person-template/`).
5. **`original-prompts.md`** — first entry = full user input, timestamped.
6. **`plan/index.md`** tree + child md under `plan/`.
7. **`implementation-index.md`** + **`plan/chunks/<id>.md`** (from `plan/chunks/_chunk-template.md`) — **N chunks** (N ≤ 15): deliverables, **verify commands**, **tests to curate**, acceptance, **Literal copy** for UI.
8. **`implementation-checklist.md`** + empty **`implementation-log.md`**.
9. **Stack copilot (only when relevant):** if `project-utils/abstract-batteries-included/` exists, read README/ARCHITECTURE. Suggest only when it helps: **Celery** (bg tasks), **Redis** (cache/speed), **DRY**, inheritance mandatory, polymorphism when shared behavior warrants it, **Python OOP**, **JS/RN** functions/utilities, **Zustand** for React stores (or better fit if stated), **shared packages** in monorepos. No extra stack opinions.
10. Generate **self-sustained NN skills** from templates (fill `{{NN}}`, `{{SHORT}}`, `{{PROGRAM_ID}}`, `{{PROGRAM_GUARDRAILS}}`):
   - `vibe-scaler/templates/skills/ideator-NN-SKILL.template.md` → `.cursor/skills/vs-ideator-NN-<short>/SKILL.md`
   - `vibe-scaler/templates/skills/executionist-NN-SKILL.template.md` → `.cursor/skills/vs-executionist-NN-<short>/SKILL.md`
   - `vibe-scaler/templates/skills/ideator-checker-NN-SKILL.template.md` → `.cursor/skills/vs-checker-NN-<short>/SKILL.md`
   - `vibe-scaler/templates/skills/executionist-auto-tests-NN-SKILL.template.md` → `.cursor/skills/vs-auto-tests-NN-<short>/SKILL.md`
   - `code-reviewer-NN`, `auto-execute-NN`, **`performance-review-NN`**, **`vs-jr-goat-NN`** templates → `.cursor/skills/`
   - `goat-verification-complete.md` → `PROGRAM_ROOT/` (digest only); `status.json` from template
11. Update playground **`quick-index.md`**, program **`context/summary.md`**, + program **`index.md`** (skills + verification links).
12. Tell ideator: start with **`/vs-hub`** or **`/vs-checker-NN-<short>`** (generate), review **`verification/ideator.html`**, refine wireframes, then **`/vs-executionist-NN-<short>`** — **NN executionist skill is the entrypoint**.

## Plan quality

- Chunks: deliverables, verify commands, stop conditions, DoD in index.
- Escalation: not in plan → senior outside chat.
- UI: **Literal copy** per screen for checker wireframes.

Do not implement product code unless user says **spike**.

## Voice

**`vibe-scaler/VOICE.md`** — short copilot messages; plain meaning before plan jargon. **`pro`** → denser architecture OK.
