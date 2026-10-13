> After ideation — HTML acceptance doc + JSON criteria; ingest intern verification into progress

# Ideator checker governance

Runs **after** `vs-ideator` for the same `NN_<short-title>/` program.

## Outputs (per program)

| Path | Role |
|------|------|
| `verification/acceptance-criteria.json` | Expected checks (agent-authored; intern copies to state when testing) |
| `verification/acceptance-state.json` | Live ticks + notes (intern or pasted JSON) |
| `verification/ideator.html` | **Ideator hub** — iframes each chunk page |
| `verification/chunks/<chunk-id>.html` | **One per chunk** — wireframes + checkboxes (executionist target) |
| `verification/acceptance.html` | Optional rollup / link to ideator hub |

## Generate (`vs-checker` or `vs-checker-NN-<short>`)

- Derive items from `implementation-index.md`, plan chunks, and any UI/screen work.
- **UI work (mandatory visual layer):** For each screen, render a **literal, realistic** div-wise wireframe in `screen-mock` — copy from plan / `original-prompts` **verbatim** where specified; no generic placeholders. Mocks should read like the intended shipped UI (labels, data, states). See `vs-checker` § Literal wireframes.
- **Ideator refine loop:** Product reviews `acceptance.html`; checker **Refine** updates mocks + JSON + plan copy until ideator approves, then executionist builds to that spec.
- Group checklist items by screen; optional `region` in JSON links items to `data-region` on the mock.
- Non-UI: **General** / **API** sections — verify list only, no mock.
- Do not implement product code (wireframes are static HTML/CSS in the acceptance file only).

## Ingest (intern returns JSON)

When the user pastes JSON or updates `acceptance-state.json`:

1. Validate shape (see `vibe-scaler/templates/verification/acceptance-criteria.schema.json`).
2. Write `verification/acceptance-state.json`.
3. Update **`implementation-checklist.md`** (intern ☐ columns / notes per chunk).
4. Append **`implementation-log.md`** with summary + who verified.
5. Update folder **`index.md`** verification line if present.

If required items are unchecked, tell the human plainly what remains (see **`vibe-scaler/VOICE.md`**); do not mark program done.

## Voice

**`vibe-scaler/VOICE.md`** — describe screens and checks in everyday language; use JSON/path names after the human-readable summary.

## Progress tracking (existing)

| File | Purpose |
|------|---------|
| `implementation-checklist.md` | Chunk + iteration progress (agent + intern boxes) |
| `implementation-log.md` | Append-only execution history |
| `context-attendance-log.md` | Playground-wide attendance / plan-read issues |
| `verification/acceptance-state.json` | **Test/verify contract** for interns |

Pair with **`vs-executionist`** / **`vs-executionist-NN-<short>`** — executionist implements; checker JSON confirms intern verification before closing chunks.

