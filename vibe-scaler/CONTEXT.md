# Vibe Scaler — copyable context

Paste this block into any LLM, onboarding doc, or Antigravity/Cursor session when you need the full picture.

---

## What it is

**Vibe Scaler** is a portable workflow kit (folder `vibe-scaler/` + Cursor/Claude/Codex/Antigravity skills and rules) for turning ideas into **shipped software** with governed agents and clear human checkpoints.

It separates:

1. **Hub** — **`/vs-hub`** menu, maintained **quick-index**, user onboarding + **3 MCQ profile** (how agents explain things)  
2. **Planning** (ideator copilot) — plan tree, ≤15 executable chunks per program, literal UI copy  
3. **Acceptance** (checker) — literal wireframe HTML per chunk + ideator hub; JSON for verification ingest  
4. **Execution** (executionist) — implement chunks, **B′ auto-tests**, **jr goat verdict** (optional sub-chunks), human checklist, optional **performance review**, optional git  
5. **Context on disk** — hub → program summary → **operator folders** (takeover-friendly; git-reviewable)  
6. **Proof** — playground files are the source of truth, not chat memory  

Install into a host repo via **Windows Vibecoder Utils** `ide-init` (vibe-scaler **y**) or copy kit + rules per `INSTALL.md`. Sync mirrors: `bin/sync-agent-config.bat`.

---

## One-line pipeline

**`/vs-hub` → Idea → (split if huge) → plan + NN skills → checker HTML → execute (A→B→B′→ jr goat? →C→D?) → ingest → goat close.**

---

## Context hub (layers)

| Layer | Path |
|-------|------|
| Playground hub | `docs/idea-to-reality-playground/CONTEXT.md` |
| Quick tree | `quick-index.md` — programs, chunks, active operator, avg score |
| User (repo-wide) | `users/<kebab>/onboarding.json`, `profile.json` |
| Program rollup | `NN_<short>/context/summary.md` |
| Person (per program) | `operators/<kebab>/context.md`, `handoffs.md` |
| Performance | `performance/scores.json` — sessions + **average** |

**Takeover:** `CONTEXT.md` → program `context/summary.md` → `operators/index.md` → active person’s folder (+ git log + scores).

---

## Skill numbering

| Prefix | Meaning |
|--------|---------|
| **—** | **`vs-hub`** → `/vs-hub` (hub; not tied to one program) |
| **00** | Base skills include `vs-help-teach`, `vs-help-llm`, **`vs-terminal-keeper`**, `vs-ideator`, `vs-executionist`, `vs-jr-goat`, `vs-performance`, `vs-auto-execute`, `vs-goat` |
| **01…99** | Per program — folder `01_<short>/`; e.g. `/vs-executionist-02-foo`, `/vs-jr-goat-02-foo`, `/vs-performance-02-foo` |

**NN program skills are self-contained entrypoints** (mandatory path list + full execution contract) so Antigravity/Cursor without rule-router still works.

---

## Roles

| Role | Who | What |
|------|-----|------|
| **Hub** | Anyone | **`/vs-hub`** — numbered menu, tree, onboarding, MCQs, routes (ideator options before executionist) |
| **Ideator / owner** | Human with the vision | Decides; `/vs-ideator`, `/vs-ideator-NN-*` |
| **Planner curator** | Agent | Splits ideas **>15 chunks** via `goat-planner-tmp/` |
| **Checker** | Agent | Wireframes + `verification/chunks/*.html` + `ideator.html`; ingest JSON |
| **Executionist** | Agent | Executes the chunk for the orchestrator; attends the goat; no freelance scope |
| **Auto-tests** | Agent | **B′** — pytest/Playwright/Detox/… after Turn B |
| **Jr goat** | Agent suggests; human decides | After tests, before checklist. Known issue genres → sub-chunks on the same plan |
| **Performance** | Agent | **`/vs-performance-NN-*`** — score chunk session + running average |
| **Code reviewer** | Agent / goat | `/vs-code-reviewer-NN-*` when `code_review_pending` |
| **Operator / tester** | Human | Name, **proceed**, checklist, acceptance HTML, commit OK |
| **Goat / senior** | Human | `/vs-goat` — **closed** (MD5) |
| **Router** | Anyone lost | **`/vs-hub`** or `/vs-auto-execute` |
| **Harness** | Dev | `verify-vibe-scaler-setup.ps1` — IDE mirrors |
| **Terminals** | Agent | **`/vs-terminal-keeper`** — index → Terminal Keeper sessions + gitignored cwd |

---

## Where state lives

**Playground root:** `docs/idea-to-reality-playground/`

| Artifact | Purpose |
|----------|---------|
| `CONTEXT.md`, `quick-index.md` | Hub + maintained tree |
| `users/<kebab>/` | Onboarding flag + MCQ profile |
| `index.md` | Chronological registry |

**Per program:** `NN_<short>/`

| Artifact | Purpose |
|----------|---------|
| `context/summary.md` | Program rollup (maintained) |
| `original-prompts.md` | Canonical idea + amendments |
| `plan/`, `implementation-index.md` | Tree + ordered chunks |
| `implementation-checklist.md`, `implementation-log.md` | Throttle + history |
| `operators/index.md` | Who is active on which chunk |
| `operators/<kebab>/context.md`, `handoffs.md` | Per-person context (takeover) |
| `performance/scores.json` | Session scores + average |
| `verification/ideator.html`, `verification/chunks/<id>.html` | Acceptance wireframes |
| `verification/acceptance-state.json` | Human ticks → ingest |
| `auto-tests/last-handoff.json`, `auto-tests/<id>/*` | Test handoff + results |
| `reviews/chunk-<id>.md` | Code review |
| `status.json`, `goat-verification-complete.md` | Status + goat digest |

**Big idea staging:** `goat-planner-tmp/` at repo root.

Full path table: `vibe-scaler/SYSTEM-PATHS.md`.

---

## How execution works (per chunk)

1. **Identity** — If chat has no name, executionist asks; ensure `users/<kebab>/`; use **profile.json** with **VOICE.md**.  
2. **A — Before code:** Briefing, external teach prompt (chunk tech only), ≤5 questions → **`operators/<kebab>/context.md`**, wait for **proceed**.  
3. **B — Implement:** Active chunk doc only.  
4. **Handoff:** `auto-tests/last-handoff.json` + `spec.json`.  
5. **B′ — Auto-tests:** `/vs-auto-tests-*` → `run-tests.ps1` → checklist.  
6. **Jr goat verdict (always):** recommend / optional / skip. If they run `/vs-jr-goat-NN-*`, accepted issues become sub-chunks, then execution returns to this chunk.  
7. **C — User checklist:** Agent-done vs human-verify; user confirms.  
8. **C.2 — Performance (optional):** `/vs-performance-NN-*` (menu **7**).  
9. **D — Git (optional):** Commit + push only after explicit yes.  
10. **Human acceptance:** Real app → chunk HTML → checker **ingest**.  

Do **not** mark program **done** without checker ingest, checklist discipline, and goat close where required.

---

## How ideation works

1. Start **`/vs-hub`** or if **>15 chunks** → `/vs-planner`.  
2. `/vs-ideator` — hub files if missing, program folder with **`context/`**, **`performance/`**, operator templates, plan, **NN skills** (incl. **vs-jr-goat** and **vs-performance**).  
3. Stack hints only when relevant (see ideator-governance).  
4. `/vs-checker-NN-<short>` — wireframes until ideator approves.  
5. `/vs-executionist-NN-<short>` on any IDE.

---

## Slash commands (typical order)

```text
/vs-hub                           # menu: New idea, Mockups, Build…  invoke with /vs-ideator, /vs-checker, /vs-executionist
/vs-planner          # only if huge
/vs-ideator
/vs-checker-NN-<short>            # generate → refine → later ingest
/vs-executionist-NN-<short>
/vs-auto-tests-NN-<short>     # after Turn B
/vs-jr-goat-NN-<short>        # after tests, before checklist (menu 6)
/vs-performance-NN-<short>         # after chunk session (menu 7)
/vs-code-reviewer-NN-<short>              # when code_review_pending
/vs-checker-NN-<short> ingest
/vs-goat
/vs-auto-execute                  # What's next (menu 8)
/vs-help-teach                            # Teach me, including Terminal Keeper (menu t)
/vs-help-llm                              # Ask another AI — paste prompt (menu g)
/vs-terminal-keeper                 # backend/frontend/app/electron + combos like web (menu k)
/vs-harness           # Check setup (menu h)
```

---

## How agents talk

**`vibe-scaler/VOICE.md`** — plain language first, then the precise term. Crisp on build turns: **doing / did / next**. Respect **`users/<kebab>/profile.json`**. User writes **`pro`** for denser detail.

**Authority:** the **orchestrator** (human in chat) decides and says **proceed**. The **goat** closes. The **executionist** only executes the active chunk. Plan gap → stop and ask. No extra features.

## What “governed” means

- Orchestrator decides in chat; goat closes; executionist does not freelance
- Plan gaps → stop and ask; no invented scope
- One chunk; cold start is briefing only until **proceed**
- **Identity + operator folders** — no anonymous writes to checklist/log
- **App works** ≠ **process done** — tests, ingest, goat close  

---

## Verify install (utils or host repo)

```powershell
powershell -NoProfile -File vibe-scaler/scripts/verify-vibe-scaler-setup.ps1
```

---

## Source in this monorepo

- Kit: `vibe-scaler/`  
- Install: `bin/lib/VibeScalerInstall.ps1`, `vibe-scaler/INSTALL.md`  
- Full workflow (phases + flowchart): `vibe-scaler/WORKFLOW.md`  

---

_End of copyable context._
