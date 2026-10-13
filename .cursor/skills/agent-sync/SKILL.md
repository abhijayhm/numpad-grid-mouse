---
name: agent-sync
description: Sync Cursor rules and skills to Claude Code, Codex, and Antigravity using verified IDE folder layouts. Use after editing .cursor/custom-rules, .cursor/skills, or .cursor/rules, or when the user asks to sync agent config across IDEs.
---

# Agent config sync

## Source of truth

| Path | Content |
|------|---------|
| `.cursor/custom-rules/*.mdc` | On-demand rule bodies |
| `.cursor/rules/rule-router.mdc` | Always-on router pointer |
| `.cursor/skills/**/SKILL.md` | Agent skills |

## Verified mirrors (generated — do not edit directly)

| IDE | Rules location | Skills | Format |
|-----|----------------|--------|--------|
| Claude Code | `.claude/rules/*.md` | `.claude/skills/` | `.md` with optional `paths:` frontmatter |
| Codex | **`AGENTS.md`** (repo root) | Referenced in AGENTS.md | Markdown; `.codex/rules/*.rules` is exec-policy only |
| Antigravity | **`.agents/rules/*.md`** | `.agents/skills/` | `.md`; `.agent/` is legacy |

## Commands

```bat
bin\sync-agent-config.bat          REM refresh mirrors in this repo
bin\ide-init.bat                   REM install all IDE bundles into another project (run from target cwd)
```

## When to run

After any change under `.cursor/custom-rules/`, `.cursor/skills/`, or `.cursor/rules/`.

Read `custom-rules/agent-sync.mdc` for full policy.
