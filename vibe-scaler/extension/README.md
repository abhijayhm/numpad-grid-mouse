# Vibe Scaler — VS Code / Cursor extension (spec)

This folder holds the **product and runtime spec** for Vibe Scaler as a full IDE extension on top of **VS Code** and **Cursor**, plus cloud orchestration. It extends the portable kit (`vibe-scaler/` skills, rules, playground) — it does not replace it.

**Source of truth for workflow today:** `vibe-scaler/WORKFLOW.md`, `vibe-scaler/CONTEXT.md`, playground under `docs/idea-to-reality-playground/`.

**Onboarding (skills-only today):** `vibe-scaler/prompts/pgoat-init.txt`.

## Documents in this folder

| File | Contents |
|------|----------|
| [plan-recap.md](./plan-recap.md) | End-to-end pipeline: agents, testing, visual evidence, human checkpoint, SaaS, team scaling |
| [runtime-algorithm.md](./runtime-algorithm.md) | Algorithmic blueprint: architecture, execution loop, visual recording, governance, SaaS scaling |
| [ui-automation-mcp.md](./ui-automation-mcp.md) | Playwright vs desktop MCP (Windows / macOS / Linux); when chunks require UI automation |
| [vscode-sdk-architecture.md](./vscode-sdk-architecture.md) | Extension shape: TypeScript UI, `@cursor/sdk` agent backend |
| [ideation-three-agent-runtime.md](./ideation-three-agent-runtime.md) | **Current ideation:** Agents 1/2/3 roles, deterministic pipeline, MCP, history folder |
| [ui-and-features.md](./ui-and-features.md) | Sidebar UI, SQLite state, sessions, FOR_USER streaming, MCP status dot |

## Build order

1. **Plan 1 (COMPLETED)** — playground program [`01_vs-sdk-harness`](../../docs/idea-to-reality-playground/01_vs-sdk-harness/) + [`vibe-scaler-sdk`](../vibe-scaler-sdk/) (10 chunks, Python harness verified end-to-end with tests and smoke script).
2. **Plan 2+** — VS Code extension UI reuses the same SDK (`goat-planner-tmp/split-index.md`). Ready for extension ideator.

## Direction (one sentence)

Reliably deliver work across many projects without proportional senior headcount, while **humans** (orchestrator + goat) keep authority over scope, consequential decisions, and final acceptance.

## Relationship to existing `/vs-*` skills

The extension **implements and surfaces** the same gates the kit already defines: `/vs-hub`, `/vs-auto-execute`, `/vs-executionist`, `/vs-auto-tests`, `/vs-jr-goat`, human checklist, checker ingest, `/vs-goat`. Playground files remain authoritative; chat and agent sessions are not a substitute for plans, status, handoffs, or acceptance artifacts.
