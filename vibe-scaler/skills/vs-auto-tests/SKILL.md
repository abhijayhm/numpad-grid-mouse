---
name: vs-auto-tests
description: >-
  Vibe Scaler auto-tests 00. Runs after executionist Turn B for same chunk.
  Canonical stack pytest Playwright Detox Flutter Vitest filesystem. Reads
  last-handoff.json, spec.json, profiles, run-tests, generate-checklist.
  Use /vs-auto-tests or auto-continue from executionist handoff.
---

# Executionist auto-tests 00 — base

You run the **canonical test stack** for the **active chunk** after **executionist** implemented it. Same program, same operator, no context loss.

## Mandatory read first

1. `vibe-scaler/SYSTEM-PATHS.md`
2. `vibe-scaler/VOICE.md`
3. `vibe-scaler/vs-auto-tests/README.md`
4. **`PROGRAM_ROOT/auto-tests/last-handoff.json`** (written by executionist)
5. `.cursor/custom-rules/vs-auto-tests-governance.mdc`
6. `.cursor/skills/vs-executionist/SKILL.md` § Turn C (you feed C, not replace it)

## When to run

| Trigger | Action |
|---------|--------|
| **Auto** | Executionist finished **Turn B** → handoff `status: pending` → **you take over same chat** |
| **Manual** | `/vs-auto-tests` or `/vs-auto-tests-NN-<short>` |
| **Re-run** | User says **re-run auto-tests** for chunk `<id>` |

## Handoff contract (`last-handoff.json`)

Executionist writes:

```json
{
  "from_skill": "executionist",
  "next_skill": "vs-auto-tests",
  "program_id": "02_short",
  "program_root": "docs/idea-to-reality-playground/02_short/",
  "chunk_id": "03",
  "operator": "priya",
  "spec_path": "docs/.../auto-tests/03/spec.json",
  "acceptance_html": "verification/chunks/03.html",
  "expected_user_input": ["None — agent run only", "OR: Detox device/sim ready", "OR: API keys in .env"],
  "status": "pending",
  "created_at": "ISO"
}
```

**Tell the user** at takeover:

- **Next skill:** `vs-auto-tests` (this skill)
- **Chunk:** `<id>`
- **What we need from you:** list `expected_user_input` (may be empty)
- **What the agent does:** detect target → profile → tests → `results.json` → `generated-checklist.md`

Set handoff `status` → `running` when you start.

## Canonical stack

| Target | Framework |
|--------|-----------|
| backend | **pytest** |
| web | **Playwright** (default UI E2E) |
| mobile-react-native | **Jest + RNTL + Detox** |
| mobile-flutter | **flutter_test + integration_test** |
| desktop-electron | **Vitest/Jest + Playwright Electron** |
| filesystem | **Node/Python assertions** |

Mapping: Django/Python → pytest; Node backend → Vitest; React web → Playwright; etc. (see kit README).

## Steps (per handoff)

1. Load **`spec.json`** at `spec_path` (schema: `vibe-scaler/vs-auto-tests/spec.schema.json`). If missing, create from chunk doc + `verification/chunks/<id>.html`.
2. **Detect target** if needed: match repo to `vibe-scaler/vs-auto-tests/profiles/*.json` `detect_globs`; set `spec.target`.
3. **Generate/update tests** minimal for this chunk only (right adapter; do not boil the ocean).
4. Run:

   ```powershell
   powershell -NoProfile -File vibe-scaler/vs-auto-tests/run-tests.ps1 -SpecPath "<spec_path>"
   ```

5. Run:

   ```powershell
   powershell -NoProfile -File vibe-scaler/vs-auto-tests/generate-checklist.ps1 -ResultsPath "<dir>/results.json"
   ```

6. **Code review + refine loop (policy)** — before user checklist:

   - Read chunk diff; apply **code-reviewer-governance** standards (index context from `docs/indexing/` if present; else note “index stale”).
   - If tests pass but quality/policy fails → **refine code** → re-run step 4–5 (**repeat** until pass + acceptable or **blocked**).
   - Write **`PROGRAM_ROOT/reviews/chunk-<id>-auto.md`** — **two crisp bullets** + short verdict.

7. Chat: **`### Agent test run (auto-tests)`** + **`### Policy code review`** — link `generated-checklist.md` and review file.

8. Update **`implementation-log.md`**: `## ISO — auto-tests (chunk id) passed|failed`.

9. **Status:**

   - Needs senior goat read → **`code_review_pending`** + run or schedule **`/vs-code-reviewer-NN-<short>`** (full `reviews/chunk-<id>.md` set).
   - Else → **`human_checklist_pending`**; handoff `passed` → executionist **Turn C**.

10. On **blocked** (device/secrets): list `expected_user_input`; wait; re-run from step 4.

## Per-program skill

`vs-auto-tests-NN-<short>` — same behavior, fixed `program_root` (entrypoint for Antigravity).

## Do not

- Skip to user checklist (Turn C) without running this skill after Turn B
- Mark chunk complete without `results.json` when auto-tests apply

## Voice

**`vibe-scaler/VOICE.md`** — say what the tests prove in plain English, then name the framework. Announce **`expected_user_input`** from handoff in words, not JSON keys only.
