# Vibe Scaler

**`/vs-hub` hub → ideation → acceptance (HTML+JSON) → chunked execution → performance scores → verification → goat close.**

Portable subproject: copy `vibe-scaler/` into any repo (or submodule). Wire skills/rules into `.cursor/` per `INSTALL.md`.

## Skill numbering (search by `00`, `01`, …)

| # | Folder / slash command | Role |
|---|------------------------|------|
| — | `vibe-scaler` → **`/vs-hub`** | Hub menu, quick-index, onboarding, MCQ profile |
| **00** | `vs-performance` | Chunk session score + average |
| **00** | `vs-ideator` → `/vs-ideator` | New program + plan |
| **00** | `vs-checker` → `/vs-checker` | Acceptance HTML/JSON (any program) |
| **00** | `vs-executionist` → `/vs-executionist` | Execute chunks (resolves program from index) |
| **01…** | `ideator-01-<short>`, `executionist-01-<short>`, `ideator-checker-01-<short>` | **Per program** — `01` matches folder `01_<short>/` |

Search in Cursor skills: type `ideator-01`, `executionist-02`, `checker-03`, etc.

## Roles

| Role | Who | Skill |
|------|-----|--------|
| **Ideator** | Product / lead — brings vision | `/vs-ideator` → `/vs-ideator-NN-<short>` |
| **Checker** | Agent — acceptance curator | `/vs-checker-NN-<short>` (or `-00-base`) |
| **Senior engineer** | Agent — executes plan | `/vs-executionist-NN-<short>` (or `-00-base`) |
| **Intern** | Human — tests HTML checkboxes, pastes JSON | Drives `acceptance-state.json` → checker ingest |

## Where data lives (in host repo)

```text
docs/idea-to-reality-playground/
  CONTEXT.md                        # hub summary → sub-contexts
  quick-index.md                    # maintained program/chunk tree
  users/<name>/                     # onboarding.json + profile.json (MCQs)
  index.md                          # chronology + links
  NN_short-title/                   # one folder per ideated program
    context/summary.md            # program rollup
    operators/<name>/context.md     # per-person takeover context
    performance/scores.json         # session scores + average
    index.md
    original-prompts.md             # this idea only — verbatim inputs + amendments (not shared across NN_* folders)
    plan/                           # tree of chunked plans
    implementation-index.md
    implementation-checklist.md
    implementation-log.md
    verification/
      acceptance.html
      acceptance-criteria.json
      acceptance-state.json
  context-attendance-log.md           # playground-wide (plan read / adherence)
```

Generated per program (host `.cursor/skills/`):

- `vs-ideator-NN-<short>/SKILL.md` — refine plan only; human-first voice ([VOICE.md](./VOICE.md))
- `vs-checker-NN-<short>/SKILL.md` — generate/ingest acceptance for that folder only
- `vs-executionist-NN-<short>/SKILL.md` — run chunks; senior agent; plain language + technical detail ([VOICE.md](./VOICE.md)); **`pro`** for density

## Entrypoints (any IDE)

| Who | Skill |
|-----|--------|
| Big idea (>15 chunks) | `/vs-planner` → `goat-planner-tmp/` |
| New program | `/vs-ideator` |
| Refine plan | `/vs-ideator-NN-<short>` |
| Acceptance | `/vs-checker-NN-<short>` → `verification/ideator.html` + `chunks/*.html` |
| Execute | `/vs-executionist-NN-<short>` (**self-contained** — paths in skill + `SYSTEM-PATHS.md`) |

## Quick start

1. `ide-init` or copy `vibe-scaler/` + install skills/rules per [INSTALL.md](./INSTALL.md).
2. **`/vs-ideator`** → NN skills from templates (not thin stubs).
3. Checker → **`ideator.html`** (ideator) + **per-chunk HTML** (executionist).
4. **`/vs-executionist-NN-<short>`** → operators, learning prompt, strict checklist, git after user OK.

See [SYSTEM-PATHS.md](./SYSTEM-PATHS.md) and [WORKFLOW.md](./WORKFLOW.md).

## Test setup (harness)

```powershell
powershell -NoProfile -File vibe-scaler/scripts/verify-vibe-scaler-setup.ps1
```

Skill: **`/vs-harness`** — simulates Antigravity (`.agents/`), Claude (`.claude/`), Codex (`AGENTS.md`) mirror checks per vendor docs.
