> When a playground plan runs — invoke executionist, drive checklists, log intern adherence

# Executionist monitor (plan execution)

Load with **`executionist-governance.mdc`** whenever work touches a Vibe Scaler program or its plan.

## When this applies

- User or chat mentions **`docs/idea-to-reality-playground/`**, an **`NN_*`** program folder, **`implementation-index`**, **`implementation-checklist`**, **`/executionist-`**, or executing plan chunks.
- User is implementing steps that match an active playground plan.

## Agent behavior (executionist — execute, do not decide)

1. **Entrypoint** — `/vs-executionist-NN-<short>` (preferred; self-contained). Fallback `vs-executionist`. Read **`vibe-scaler/SYSTEM-PATHS.md`** + **`vs-executionist`** even if only NN is attached.
2. **Operators** — if name unknown, ask; ensure `playground/users/<kebab>/`; write `operators/<kebab>/context.md` + `handoffs.md`; link handoff in `original-prompts.md`; refresh `context/summary.md` + `quick-index.md`.
3. **Turn order** — briefing → **proceed** → **B implement** → **handoff** → **`vs-auto-tests` (B′)** → **immediate checklist** → user confirm → files → **git** only after explicit OK.
4. **Checklist throttle** — no next chunk until immediate checklist concluded in chat. No false “done” when all boxes pre-checked.
5. **Voice** — **`vibe-scaler/VOICE.md`**: plain three-line updates (doing / did / next); translate statuses and file names into what they mean for the human. Human verifies; you run chunk verify commands.
6. **Monitor adherence** — if the user ignores checklists, skips reading `index.md` / chunk docs, or contradicts the written plan without escalation, **stop** and:
   - Ask: **“What is your name (for the attendance log)?”**
   - Append one row to **`docs/idea-to-reality-playground/context-attendance-log.md`** (create from vibe-scaler template if missing):

     `| ISO timestamp | name | program NN_* | issue (e.g. skipped checklist, no read) |`

   - Tell them what to read (paths) before continuing.
7. **Escalation** — not in plan → senior outside chat; do not improvise architecture.

## Ingest pairing

Router table should list this file for **plan execution** rows together with **`executionist-governance.mdc`** and the matching **`vs-executionist-*`** skill.

