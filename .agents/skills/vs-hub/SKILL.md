---
name: vs-hub
description: >-
  Vibe Scaler hub — numbered terminal-style menu, playground quick-index tree,
  role help, user onboarding + 3 MCQ profile, routes to ideator/executionist skills.
  Use /vs-hub anytime.
---

# Vibe Scaler — hub menu

**Entry point for humans.** Read **`vibe-scaler/VOICE.md`** — adapt depth using the user’s **`users/<name>/profile.json`** when present.

## Mandatory read first

1. `vibe-scaler/SYSTEM-PATHS.md`
2. `vibe-scaler/VOICE.md`
3. `docs/idea-to-reality-playground/CONTEXT.md`
4. `docs/idea-to-reality-playground/quick-index.md`
5. `.cursor/custom-rules/vs-hub-hub-governance.mdc`

## Session start (every time)

### 1 — Identify the human

If the message does **not** clearly state who they are:

- Ask: **“What name should we use for this session?”** (kebab-case for folders, e.g. `alex-k`)
- Create if missing: `docs/idea-to-reality-playground/users/<kebab>/onboarding.json` + `profile.json` from `vibe-scaler/templates/users/_template/`

Load **`profile.json`** and **`onboarding.json`** for that user.

### 2 — Onboarding gate

If `onboarding.json` → `vibe_scaler_flow_complete` is **false**:

1. Ask: **“Have you already done the Vibe Scaler walkthrough?”**  
   - **Yes** → set `vibe_scaler_flow_complete: true`, `source: user_declared`, `completed_at` ISO.  
   - **No** → offer **menu option 0** (tutorial); after they finish, set `source: completed_tutorial`.

Until onboarded, show a **short** “what this is” blurb before the full menu.

### 2b — Terminal Keeper (once per user)

If `onboarding.json` has no `terminal_keeper_installed: true`:

1. Install the Cursor/VS Code extension **Terminal Keeper** (`nguyenngoclong.terminal-keeper`):
   - Try `cursor --install-extension nguyenngoclong.terminal-keeper`
   - If that fails, try `code --install-extension nguyenngoclong.terminal-keeper`
2. Tell them in one sentence what it does: named terminals you can restore after a restart.
3. Print the paste-ready prompt from **`vibe-scaler/prompts/terminal-keeper-kt.md`** so they can learn it in ChatGPT.
4. Set `terminal_keeper_installed: true` and `terminal_keeper_installed_at` (ISO) even if the CLI failed — also set `terminal_keeper_note` to the exact error and the command they can run. Do not block the rest of onboarding on a failed install.

### 3 — Profile MCQs (if `profile.json` → `profiled_at` is null)

Ask **3 multiple-choice questions** (one message, numbered options):

**Q1 — Building software with AI agents**

1. New — I’m learning  
2. Comfortable — I’ve shipped a few things  
3. Daily — I work like a staff engineer  

**Q2 — How you like explanations**

1. Step-by-step, one thing at a time  
2. Short overview, then details when I ask  
3. Technical first — I’ll ask if I’m lost  

**Q3 — Vibe Scaler familiarity**

1. First time  
2. Used it once or twice  
3. I use this playground regularly  

Map answers → `mcq` fields + `labels`:

| Q1 | `experience_tier` |
|----|-------------------|
| 1 | beginner |
| 2 | comfortable |
| 3 | pro |

| Q2 | `explanation_style` |
|----|---------------------|
| 1 | steps |
| 2 | overview |
| 3 | technical |

| Q3 | `familiarity` |
|----|---------------|
| 1 | new |
| 2 | returning |
| 3 | regular |

Set `profiled_at` ISO. Add one line to `notes_for_agents` (how to talk to this person).

### 4 — Role

Ask: **“What’s your role today?”** (or infer)

| # | Role | Typical next |
|---|------|----------------|
| 1 | **Ideator** (product owner) | Menu **1** or **2** |
| 2 | **Executionist driver** (build with agent) | Menu **4** |
| 3 | **Tester / intern** | Menu **3** ingest path + chunk HTML |
| 4 | **Goat / senior** | Menu **10** Sign off |

Default assumption if they skip: **executionist driver** — briefly teach chunk flow (A → proceed → build → tests → checklist → git?).

---

## Rebuild quick index (before showing menu)

1. Read `docs/idea-to-reality-playground/index.md` and each `NN_*/` program: `status.json`, `implementation-checklist.md`, `operators/index.md`, `performance/scores.json`, `context/summary.md`.
2. Update **`quick-index.md`** (template: `vibe-scaler/templates/playground-quick-index.md`).
3. Touch **`CONTEXT.md`** snapshot lines (counts, in-flight).
4. Per program, refresh **`context/summary.md`** snapshot table.

---

## Main menu (print every `/vs-hub` unless user sent a number)

**Required format (Cursor, Claude Code, Codex, Antigravity):** plain numbered list below. See **`vibe-scaler/VOICE.md`** — box-drawing is optional and only if the client renders Unicode boxes; never use boxes as the only menu.

```text
VIBE SCALER — reply with a number

Hello, <name> — <role> — <experience_tier>

Where things are:
- NN_short | status | chunk | person

Menu (plain name on the left, type the /vs- name to run it):
0  Learn            /vs-learn            (this menu, option 0)
t  Teach me         /vs-help-teach
g  Ask another AI   /vs-help-llm
1  New idea         /vs-ideator
2  Edit plan        /vs-ideator-nn
3  Mockups          /vs-checker
4  Build            /vs-executionist
5  Test             /vs-auto-tests
6  Polish           /vs-jr-goat
7  Score            /vs-performance
8  What's next      /vs-auto-execute
9  Review           /vs-code-reviewer
10 Sign off         /vs-goat
p  Split            /vs-planner
k  Terminals        /vs-terminal-keeper
h  Check setup      /vs-harness

Order: New idea → Mockups → Build → Test → Polish → checklist → Sign off.
```

### Option 0 — Learn (sets onboarding when done)

Same lesson as **/vs-help-teach**. Use the plain menu names (New idea, Build). The searchable slash is the `/vs-` name. One step at a time.

Point to: `CONTEXT.md` → each program `context/summary.md` → `operators/<you>/context.md`.

On completion: `onboarding.json` → `completed_tutorial`.

### User picks a number

- Resolve **NN** from reply or ask which program from the tree.
- Tell them the **exact slash command** to run next and what they will need to type (e.g. **proceed**, name).
- Do **not** implement product code in this skill — route only.

### Help (`?`)

Re-show roles + link `vibe-scaler/WORKFLOW.md` + offer option **0**.

---

## Using profile when talking

| Label | Agent behavior |
|-------|------------------|
| `experience_tier: beginner` | More analogies; fewer acronyms without translation |
| `comfortable` | Default **VOICE.md** two-layer |
| `pro` | Denser paths/status; user may also type **`pro`** in chat |
| `explanation_style: steps` | Numbered micro-steps |
| `overview` | 2-sentence summary before detail |
| `technical` | Lead with precise terms after one-line outcome |

---

## Do not

- Skip identity when writing playground files
- Show menu without refreshing **quick-index** when stale (>1 program or user asks “where are we?”)
- Replace executionist/ideator skills — **route** to them
