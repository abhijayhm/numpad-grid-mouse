> Vibe Scaler — ideation playground + executionist chunks; load when using ideator/executionist skills

# Vibe Scaler (portable)

## When

- `/vs-hub` (hub menu), `/vs-ideator`, `/vs-ideator-NN-*`, `/vs-checker-*`, `/vs-executionist-*`, `/vs-auto-tests-*`, `/vs-jr-goat-*`, `/performance-review-*`, `/vs-code-reviewer-*`, `/vs-auto-execute-*`, `/vs-goat` (`00` = base; `NN` = program)
- `docs/idea-to-reality-playground/**`

## Playground root

`docs/idea-to-reality-playground/` — numbered folders `NN_short-title`. Each idea is self-contained.

## Mandatory files per idea

| File | Owner |
|------|--------|
| `index.md` | Ideator + executionist |
| `original-prompts.md` (per idea folder only) | Ideator appends every user input for **that** program |
| `plan/` | Ideator — tree + `plan/index.md` |
| `implementation-index.md` | Ideator chunks; executionist follows |
| `implementation-checklist.md` | Executionist updates |
| `implementation-log.md` | Executionist append-only |
| `verification/ideator.html` | Ideator hub (iframes all chunk pages) |
| `verification/chunks/<chunk-id>.html` | Per-chunk acceptance (executionist) |
| `verification/acceptance.html` | Optional aggregate / legacy link |
| `context/summary.md` | Program rollup (maintained) |
| `operators/<kebab>/context.md`, `handoffs.md` | Per-person per program (takeover-friendly) |
| `performance/scores.json` | Chunk session scores + average |
| `users/<kebab>/onboarding.json`, `profile.json` | Playground-wide onboarding + MCQ profile |
| `status.json` | Machine status (see `templates/program-status.schema.json`) |
| `reviews/chunk-*.md` | Code review artifacts |
| `goat-verification-complete.md` | MD5 digest only — goat close |
| `auto-tests/last-handoff.json` | Execution chain handoff |
| `verification/acceptance-criteria.json` | Expected checks (canonical) |
| `verification/acceptance-state.json` | Human ticks + notes; ingest updates progress |

## Program status (standard)

`ideation` · `executing` · `iteration` · `auto_tests_running` · `code_review_pending` · `human_checklist_pending` · `verification_pending` · `goat_verification_pending` · `blocked` · **`closed`** (goat verified) · `cancelled` · `discarded` · **`archived`** (move to `playground/archives/NN_short/`)

Custom values allowed in notes. Executionist may set **`code_review_pending`**; goat sets **`closed`** via **`vs-goat`** (secret hashed MD5, never stored plaintext).

## Root registry

Update `docs/idea-to-reality-playground/index.md` on every new idea (chronology table). Maintain **`CONTEXT.md`**, **`quick-index.md`**, and each program **`context/summary.md`** when status/chunks/operators change.

## Voice

Read **`vibe-scaler/VOICE.md`** every Vibe Scaler turn. **Plain English first** (what / why / what they do next), then precise terms and paths for pros. Default rhythm: **doing · did · next**. User wrote **`pro`** → staff-engineer density. Not in plan → stop; ask **senior human outside chat** in clear language.

Also read: `vibe-scaler-hub-governance.mdc`, `ideator-governance.mdc`, `executionist-governance.mdc`, `executionist-monitor.mdc`.

Attendance log (repo-wide for playground): `docs/idea-to-reality-playground/context-attendance-log.md`.

