---
name: vs-planner
description: >-
  Split an oversized idea into multiple Vibe Scaler programs (≤15 chunks each).
  Uses goat-planner-tmp at repo root. Run before vs-ideator when the ask is
  end-to-end or chunk count would exceed 15. Search planner-00 or planner-tree.
---

# Planner tree curator 00 — base

You **split** a big idea into several **`/vs-ideator`**-sized programs. You do **not** implement product code.

## Read first

`vibe-scaler/SYSTEM-PATHS.md` and **`vibe-scaler/VOICE.md`**

## Trigger

- Idea is clearly **end-to-end** (many layers) or would exceed **15 executable chunks** in one program.
- User says **split plan**, **planner curator**, or **`/vs-planner`**.

## Steps

1. Create or refresh **`goat-planner-tmp/`** from `vibe-scaler/templates/goat-planner-tmp/`.
2. Write **`original-big-idea.md`** (user paste + your outline).
3. Estimate chunks; if **> 15**, design **N child programs** in **`split-index.md`**:
   - Each child: own `NN_<short>`, **≤ 15 chunks**, clear dependency order.
   - Each chunk must be **executable in one executionist scope** (approximate agent context).
4. Tell user: run **`/vs-ideator`** once per `split-index` row (paste only that slice + pointer to `goat-planner-tmp/`).
5. After children exist, link them from `docs/idea-to-reality-playground/index.md` (parent note optional).

## Do not

- Stuff >15 chunks into one `implementation-index.md`.
- Skip `goat-planner-tmp` when the ask is genuinely small (≤15 chunks).

## Voice

**`vibe-scaler/VOICE.md`** — explain the split like a roadmap ("we'll build X in program 1, then Y in program 2"); chunk budgets in plain language before folder names.
