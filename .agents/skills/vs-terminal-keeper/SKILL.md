---
name: vs-terminal-keeper
description: >-
  Build Terminal Keeper sessions from the project index: one terminal per app
  (backend, frontend, app, electron) plus combos (web = backend+frontend).
  OS picks ps1, bat, or sh. Local cwd is gitignored. Use /vs-terminal-keeper.
---

# Setup terminals

Creates or **updates** Terminal Keeper so each app has its own terminal, and combos open several at once. Commands run from a **local cwd file** (gitignored), so the same scripts work on any machine.

## Read first

1. `vibe-scaler/VOICE.md`
2. `docs/indexing/manifest.md` and the `full-indexing-*.md` parts it lists  
   If missing or stale → run skill **index-extractor**, then re-read.
3. Confirm stacks against the tree (do not invent apps the index does not support).

## What to detect

| Target | When the index or tree shows |
|--------|------------------------------|
| backend | API / server / Django / FastAPI / Go / `server/` |
| frontend | web UI / Next / Vite / `web/` or `frontend/` |
| app | React Native, Flutter, or a mobile `app/` |
| electron | Electron desktop |

Skip a target that is not in this repo.

## Combos (only if every member exists)

| Session | Opens |
|---------|--------|
| web | backend + frontend |
| desktop | electron, plus frontend if that is how the UI is served |
| mobile | app |
| all | every target you created |

Each **single** target is also its own session, so someone can start just backend.

## Files to write (host repo)

| Path | Git | Role |
|------|-----|------|
| `.vibe-scaler/terminal-cwd.local` | **ignored** | One line: absolute repo root on this machine |
| `scripts/dev-terminals/run.ps1` | yes | Copy from `vibe-scaler/templates/terminal-keeper/run.ps1` |
| `scripts/dev-terminals/run.sh` | yes | Copy `run.sh` |
| `scripts/dev-terminals/run.bat` | yes | Copy `run.bat` |
| `scripts/dev-terminals/targets.json` | yes | `{ "targets": { "backend": { "command": "..." } }, "combos": { "web": ["backend","frontend"] } } ` |
| `.vscode/terminal-keeper.json` | yes | Sessions for Terminal Keeper |

Append to `.gitignore` if missing:

```text
.vibe-scaler/terminal-cwd.local
```

## OS → command in terminal-keeper.json

Detect the OS you are on. Point each terminal's `command` at the matching runner (the other runners stay in the repo for teammates):

| OS | command |
|----|---------|
| Windows | `powershell -NoProfile -ExecutionPolicy Bypass -File scripts/dev-terminals/run.ps1 <target>` |
| macOS / Linux | `bash scripts/dev-terminals/run.sh <target>` |

A combo session lists one terminal object per member, each calling the runner with that member's name.

Shape:

```json
{
  "active": "web",
  "activateOnStartup": false,
  "sessions": [
    { "name": "backend", "terminals": [{ "name": "backend", "command": "<runner> backend" }] },
    { "name": "web", "terminals": [
      { "name": "backend", "command": "<runner> backend" },
      { "name": "frontend", "command": "<runner> frontend" }
    ]}
  ]
}
```

Commands inside `targets.json` are the real start commands from the index (`npm run dev`, `python manage.py runserver`, etc.), run **after** the runner `cd`s to `terminal-cwd.local`.

## If `.vscode/terminal-keeper.json` already exists

- Replace sessions whose **name** matches a target or combo you manage.
- Keep any other sessions the human added.
- Refresh `targets.json` commands from the latest index.
- Rewrite `terminal-cwd.local` to this machine's repo root.

## Tell the human

In plain language: extension **Terminal Keeper**, command **Terminal Keeper: Active Session** (or the status bar), pick `backend` or `web`. Point them at `vibe-scaler/prompts/terminal-keeper-kt.md` if they want the ChatGPT lesson. Menu letter **k**.
