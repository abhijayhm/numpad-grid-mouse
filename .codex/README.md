# Codex project layer

OpenAI Codex loads **project instructions from `AGENTS.md` at the repository root** (this repo: ../AGENTS.md).

| Path | Purpose |
|------|---------|
| `../AGENTS.md` | Coding rules & vibecoding context (generated from `.cursor/`) |
| `config.toml` | Optional Codex settings (copy from `config.toml.example`) |
| `rules/*.rules` | **Shell execution policy only** — not general coding rules |

Docs: https://developers.openai.com/codex/guides/agents-md
