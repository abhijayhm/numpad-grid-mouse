---
name: vs-code-reviewer
description: >-
  Per-chunk code review for a Vibe Scaler program. Uses git + index-extractor indexing;
  one review file per plan chunk with pseudocode, mermaid, and change highlights.
  Use /vs-code-reviewer or code-reviewer-NN-short when status code_review_pending.
---

# Code reviewer 00 — base

Senior-engineer / goat-facing review artifact. **Two crisp deliverables per chunk** (see below).

## Mandatory read first

1. `vibe-scaler/SYSTEM-PATHS.md`
2. `vibe-scaler/VOICE.md`
3. `.cursor/custom-rules/code-reviewer-governance.mdc`
4. Skill **index-extractor** — refresh or read `docs/indexing/manifest.md` + shards if stale
5. `PROGRAM_ROOT/implementation-index.md`, all `plan/chunks/*.md`
6. `PROGRAM_ROOT/status.json` or `index.md` status (**`code_review_pending`**)

## Scope selection

User may pin:

- Whole **program** (`NN_<short>`)
- **Chunk id**
- **Skill context** (`vs-executionist-NN-*` or `vs-ideator-NN-*`) — map to program via skill frontmatter

## Workflow

1. **Index context** — run or read `docs/indexing/`; use `git log` / `git diff` scoped to chunk commits (messages referencing `chunk <id>` or date range from `implementation-log.md`).
2. For **each** `plan/chunks/<id>.md` (same count as plan — one review file per chunk):

   Write **`PROGRAM_ROOT/reviews/chunk-<id>.md`** using `vibe-scaler/templates/reviews/chunk-review.template.md`.

3. Set program status **`code_review_pending`** → **`code_review_iteration`** while drafting; when reports complete and no blockers, leave **`code_review_pending`** for human goat read OR advance per governance.
4. Summarize in chat: **two crisp bullets per chunk** (see template § Crisp summary).

## Per-chunk file must include

| # | Section |
|---|---------|
| **1** | **Crisp summary** — two bullets max (what changed + risk/quality) |
| **2** | **Pseudocode** — literal actions inferred from diff |
| **3** | **Mermaid** — flowchart or tree of control/data for this chunk |
| **4** | **Code highlights** — main hunks from `git diff` (paths + cited lines; not whole files) |
| **5** | **Top narrative** — how the change set reads as a whole (short paragraph) |

## Pairing

- **vs-auto-tests** runs policy review + refinements **before** user checklist; may invoke this skill when `code_review_pending`.
- **vs-goat** after senior approves digest verification → **`closed`**.

## Voice

**`vibe-scaler/VOICE.md`** — top bullets readable by a non-coder. In chat, follow any mermaid with a text outline (Codex, Claude Code, Antigravity may not draw diagrams). Diff detail stays in the review file.
