---
name: vs-harness
description: >-
  Test Vibe Scaler and multi-IDE agent setup in this repo — simulates Antigravity (.agents),
  Claude Code (.claude), and Codex (AGENTS.md) without external SDKs. Run verify script;
  fix with sync-agent-config. Use /vs-harness or "test vibe scaler setup".
---

# Vibe Scaler harness 00 — setup test

You verify that **Vibe Scaler** and **IDE mirrors** match what **Antigravity**, **Claude Code**, and **Codex** load from disk (skills are filesystem-discovered; Codex uses root `AGENTS.md` per OpenAI docs).

## When to use

- After `ide-init`, vibe-scaler install, or editing `.cursor/skills` / rules
- Before trusting **`/vs-executionist-NN-<short>`** in Antigravity-only sessions
- User says **test vibe scaler**, **harness**, or **`/vs-harness`**

## Mandatory read

`vibe-scaler/SYSTEM-PATHS.md` and **`vibe-scaler/VOICE.md`**

## Run verification (always execute)

From **repository root**:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File vibe-scaler/scripts/verify-vibe-scaler-setup.ps1
```

Optional JSON + auto-sync:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File vibe-scaler/scripts/verify-vibe-scaler-setup.ps1 -Json
powershell -NoProfile -ExecutionPolicy Bypass -File vibe-scaler/scripts/verify-vibe-scaler-setup.ps1 -FixSync
```

Report **every `[FAIL]`** line; suggest `bin\sync-agent-config.bat` when Antigravity/Claude mirrors are missing.

## What each “IDE simulation” means

| Profile | Paths checked | Real-world equivalent |
|---------|----------------|------------------------|
| **Cursor** | `.cursor/skills/*-00-base/`, `.cursor/custom-rules/*vibe*` | Native skill + rule-router ingest |
| **Antigravity** | `.agents/skills/`, `.agents/rules/` | [Antigravity skills](https://antigravity.google/docs/skills/) — workspace `.agents/skills/<name>/SKILL.md` |
| **Claude Code** | `.claude/skills/`, `.claude/rules/` | CLI/SDK `settingSources` + `skills` on `query()` — [Agent SDK skills](https://code.claude.com/docs/en/agent-sdk/skills) |
| **Codex** | `AGENTS.md` at repo root | [agents.md guide](https://developers.openai.com/codex/guides/agents-md) — no separate skills folder |

This harness does **not** call Claude/Codex APIs; it asserts **on-disk parity** with `sync-agent-config`.

## Antigravity-only drill (manual)

1. Imagine **only** `.agents/skills/vs-executionist/SKILL.md` is attached (no router).
2. Open that file — confirm **Mandatory read**, **SYSTEM-PATHS**, **proceed gate**, **immediate checklist**, **git after user OK**.
3. If mirror is stale or thin, re-sync and regenerate **NN** skills from `vibe-scaler/templates/skills/executionist-NN-SKILL.template.md`.

## Claude SDK drill (manual, optional)

From a project with `@anthropic-ai/claude-agent-sdk` installed:

- `cwd` = repo root; `settingSources: ['project']`; `skills: 'all'`.
- First `init` system message should list skills whose `SKILL.md` exists under `.claude/skills/` (see SDK docs).

## Pass criteria

- Verify script exit code **0**
- Kit skills + templates present under `vibe-scaler/`
- Mirrors contain **vs-executionist** with full playbook keywords

## Fix loop

1. `bin\sync-agent-config.bat`
2. Re-run verify script
3. If consumer project: `ide-init` vibe-scaler **y** or option **6** add-missing-only

## Voice

**`vibe-scaler/VOICE.md`** — report verify results as pass/fail humans understand; mention script paths second.
