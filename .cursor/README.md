# Shared Cursor configuration

Generic agent rules, skills, and scripts for any repository. Copy or install this `.cursor/` tree into a target repo root.

## Layout

| Path | Type | Purpose |
|------|------|---------|
| `rules/rule-router.mdc` | always-on (pointer) | Lightweight router — loads custom rules on demand |
| `custom-rules/rule-router-ingest.mdc` | on-demand | One-pass ingest table (when to Read which rule) |
| `custom-rules/graphify-rule.mdc` | on-demand | Graphify setup, navigation, block vs background |
| `custom-rules/graphify-auto.mdc` | on-demand | Post-edit sync, `.graphifyignore`, incremental updates |
| `custom-rules/project-index.mdc` | on-demand | Cold-start: read `docs/indexing/` |
| `custom-rules/verify-code-update.mdc` | on-demand | Pre-edit flowchart + pseudocode per subsystem |
| `skills/index-extractor/SKILL.md` | skill | Generate `docs/indexing/full-indexing-*.md` shards |
| `scripts/graphify-update-changed.py` | script | Incremental Graphify rebuild for touched files only |
| `scripts/graphify-update-changed.ps1` | script | Windows wrapper for incremental Graphify rebuild |

**Only `rule-router.mdc` lives under `rules/`.** All other rule bodies are in `custom-rules/` and Read per phase.

`graphify.mdc` is installed per repo by `graphify cursor install` → move to `custom-rules/`.

## Per-repo artifacts (not in this bundle)

| Path | Owner |
|------|-------|
| `.graphifyignore` | each repo (baseline in `graphify-auto.mdc`) |
| `graphify-out/graph.json` | generated per repo |
| `docs/indexing/*` | generated per repo via index-extractor skill |
| `custom-rules/graphify.mdc` | installed per repo by Graphify CLI |

## Agent flow

```
rule-router.mdc (always injected)
  → Read rule-router-ingest.mdc (on repo work / cold start)
    → Read matched custom-rules/*.mdc for current phase
```

## Cross-IDE sync

Edit rules/skills here, then run `bin/sync-agent-config.bat` to mirror into:

- `.claude/rules/*.md` — Claude Code
- `AGENTS.md` + `.codex/` — Codex (coding rules in AGENTS.md; `.codex/rules` is exec-policy only)
- `.agents/rules/*.md` — Antigravity (`.agent/` is legacy)

Install all bundles into another repo: `bin/ide-init.bat` (from target project cwd).

See `custom-rules/agent-sync.mdc` and skill `skills/agent-sync/SKILL.md`.
