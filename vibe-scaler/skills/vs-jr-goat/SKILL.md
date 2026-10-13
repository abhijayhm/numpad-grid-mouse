---
name: vs-jr-goat
description: >-
  After a chunk is implemented (and auto-tests when they apply), before the human
  checklist. Suggest a short list of known product issues, let the human decide,
  and turn accepted ones into sub-chunks on the same plan. Use
  /vs-jr-goat or /vs-jr-goat-NN-short.
---

# Jr goat refinements 00 — base

You are still the **executionist**. The human (orchestrator / ideator) decides product feel. You suggest; you do not invent a side quest or change scope on your own.

**When:** after Turn B (and Turn B′ when tests apply), **before** Turn C (human checklist).

## Mandatory read first

1. `vibe-scaler/VOICE.md` (plain menu/text; mermaid only with a text outline)
2. `vibe-scaler/SYSTEM-PATHS.md`
3. Active `PROGRAM_ROOT/plan/chunks/<id>.md`
4. What changed: `git diff` for this chunk (or the files the chunk names)
5. `auto-tests/<id>/results.json` if present

## Always start with a verdict

Even if this skill was not invoked yet, **executionist must print this** before Turn C. When this skill runs, print it again after you look:

```text
Jr goat refinements — verdict: recommend | optional | skip
Why: <one sentence a newcomer understands>
Next: /vs-jr-goat-NN-<short>   OR   go on to the human checklist
```

| Verdict | When |
|---------|------|
| **recommend** | You can point at something in the diff a person would feel (loader, layout, fake data, silent error, mismatched label) |
| **optional** | Polish only; parent chunk still matches the plan |
| **skip** | Change is internal (no screen, no copy, no data a human sees) or the chunk doc already covers it |

You choose the verdict. The human chooses whether to run the pass.

## Issue genres (ask these; do not invent a sixth unless the diff forces it)

For each genre that **could** apply, one sentence of what you saw, then ask: **“Is this a problem for this chunk?”**

| # | Genre | Look for |
|---|--------|----------|
| 1 | **Loaders and progress** | Spinner or wait with no words about what is happening |
| 2 | **Feel and layout** | Hierarchy, spacing, empty states — the screen feels off versus the wireframe |
| 3 | **Honest data** | Placeholder, lorem, or AI-looking sample data shown as if it were real |
| 4 | **Errors and empty** | Failure or “nothing here” is silent or a generic message |
| 5 | **Labels match actions** | Button or sentence does not do what it says |

Skip genres that do not touch this diff. **At most three** accepted issues become sub-chunks in one pass.

## If they say yes to an issue

Same idea as **`/vs-ideator`**, smaller:

1. Append a section **`## Jr goat sub-chunks`** on the parent chunk file.
2. Add **`plan/chunks/<parent-id>-jr-<n>.md`** from the chunk template: deliverable, verify command, stop condition. Each sub-chunk **returns to the parent** (what “better” means for this screen or API), not a new product.
3. List them in **`implementation-index.md`** as children of the parent. They do **not** count toward the 15-chunk program cap. They **do** block the parent checklist until done or the human explicitly drops one (log the drop).
4. Status stays **`executing`**. Next slash command is **`/vs-executionist-NN-<short>`** on the first sub-chunk, then back to the parent checklist.

Refuse filler: if a suggestion does not change what the parent chunk promised, say so and do not write a sub-chunk.

## If they say no / skip

One log line in **`implementation-log.md`**: `jr-goat skipped or declined` + verdict. Then executionist **Turn C**.

## Voice

**`vibe-scaler/VOICE.md`**. You suggest; they decide. Name the genre in plain words, then the file you would add.
