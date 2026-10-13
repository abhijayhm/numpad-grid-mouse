> Keep Cursor rules/skills synced to Claude Code, Codex, and Antigravity bundles

# Agent config sync

**Source of truth:** `.cursor/` (rules, custom-rules, skills).

**Verified IDE layouts (do not guess — use these paths):**

| IDE | Rules | Skills | Notes |
|-----|-------|--------|-------|
| **Cursor** | `.cursor/custom-rules/*.mdc` + `.cursor/rules/rule-router.mdc` | `.cursor/skills/<name>/SKILL.md` | Native format |
| **Claude Code** | `.claude/rules/*.md` | `.claude/skills/<name>/SKILL.md` | `.md` only; recursive discovery |
| **Codex** | **`AGENTS.md` at repo root** | Inline in AGENTS.md / docs | `.codex/rules/*.rules` = **shell exec policy only** |
| **Antigravity** | **`.agents/rules/*.md`** | `.agents/skills/` (convention) | `.agent/` is legacy fallback |

## When to sync

Run **`bin/sync-agent-config.bat`** after you:

- Create, edit, move, or delete anything under `.cursor/custom-rules/`
- Create, edit, move, or delete anything under `.cursor/skills/`
- Change `.cursor/rules/rule-router.mdc` or ingest table behavior

Do **not** hand-edit `.claude/`, `.agents/`, `AGENTS.md`, or `.codex/README.md` — edit `.cursor/` and re-sync.

## Install into another repo

```bat
cd C:\your\project
C:\path\to\utils\bin\ide-init.bat
```

Installs all four IDE bundles (Cursor + Claude + Codex + Antigravity).

Config: `config/utils.example.json` + gitignored `config/utils.local.json`.

