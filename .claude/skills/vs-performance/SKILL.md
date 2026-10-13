---
name: vs-performance
description: >-
  Score a completed chunk session (1-5 dimensions), append to performance/scores.json,
  update average and context/summary.md. Use /vs-performance or
  performance-review-NN-short after Turn C or from /vs-hub menu 6.
---

# Performance review 00 — base

**Ideator-side quality metric** for each **chunk completion session** (after executionist Turn C confirmed, or end of D). Others can read scores in git for takeover context.

## Mandatory read first

1. `vibe-scaler/SYSTEM-PATHS.md`
2. `vibe-scaler/VOICE.md`
3. `vibe-scaler/templates/schemas/performance-session.schema.json`
4. `PROGRAM_ROOT/implementation-log.md` (last entries for chunk)
5. `PROGRAM_ROOT/operators/<name>/` (active operator context)
6. Optional: `git log` / `git diff` for chunk scope

## Program resolution

Same as **vs-executionist** (message `NN_*`, or playground index, or **vs-performance-NN-<short>** pin).

## When to run

| Trigger | Action |
|---------|--------|
| User `/vs-performance` or menu **7** | Score **last completed** chunk session |
| Pin `chunk_id` in message | Score that chunk’s latest session |
| Executionist Turn C done | Offer: “Run performance review for this chunk?” |

## Scoring (1 = poor, 5 = excellent)

| Dimension | What you judge |
|-----------|----------------|
| **plan_adherence** | Deliverables match `plan/chunks/<id>.md` |
| **verification** | Tests/checklist/auto-tests evidence |
| **handoff_clarity** | `last-handoff.json`, operator `context.md`, log readable for takeover |
| **communication** | Briefing + checklist matched **VOICE** + operator profile |

**Overall** = rounded mean of the four (one decimal). Add 1–2 sentence **notes** for humans (plain language).

## Write

1. Ensure `PROGRAM_ROOT/performance/scores.json` exists (template in `vibe-scaler/templates/idea-folder/performance/scores.json`).
2. Append session object:

```json
{
  "chunk_id": "...",
  "operator": "<kebab-name>",
  "scored_at": "<ISO>",
  "git_ref": "<optional HEAD or commit>",
  "dimensions": { "plan_adherence": 4, "verification": 5, "handoff_clarity": 4, "communication": 4 },
  "overall": 4.3,
  "notes": "..."
}
```

3. Recompute **`average`** and **`session_count`** on all `sessions[].overall`.
4. Update **`context/summary.md`** snapshot (`Last session score`, `Average score`, `Last updated`).
5. Rebuild **`docs/idea-to-reality-playground/quick-index.md`** (or tell user to run `/vs-hub`).
6. Append one line to **`implementation-log.md`**: `## ISO — performance review chunk <id> overall <score>`.

## Voice

Explain scores so a newcomer understands; put dimension keys in parentheses for pros.

## Per-program skill

`vs-performance-NN-<short>` — same behavior, fixed `PROGRAM_ROOT`.
