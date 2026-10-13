# VS Code / Cursor extension — technical architecture

Use **`@cursor/sdk`** as the agent backend and **TypeScript** for the VS Code extension. You do not need to build the agent loop, tool orchestration, or model integration from scratch. The SDK provides Cursor's agent runtime.

Docs: [Cursor SDK (TypeScript)](https://cursor.com/docs/sdk/typescript) · [VS Code WebviewView](https://code.visualstudio.com/api/extension-guides/webview)

## Architecture

```text
VS Code Extension (TypeScript)
 ├── Sidebar UI (Webview)
 ├── Extension Controller
 └── VS Code API (workspace, edits, diagnostics)
          │
          ▼
    @cursor/sdk (Agent Engine)
     ├── Agent Loop + LLM
     ├── Codebase Context
     ├── Tool Execution
     └── Streaming Responses
```

Optional sidecars (same product, not in the extension host):

- **Python backend** — long-running orchestration, Agent 1/2 process management, WebRTC capture, MCP process pool, cloud control plane client.
- **Process C** — IPC/WebSocket between agents, human chat, pause/stop, mobile/ngrok viewers.

The extension is the **control surface**; playground + cloud hold **workflow truth**.

## 1. Install

```bash
npm install @cursor/sdk
```

Requires **Node.js 22.13+**. Set `CURSOR_API_KEY` in the environment.

## 2. Run the agent (minimal)

```typescript
import { Agent } from "@cursor/sdk";

const agent = await Agent.create({
  apiKey: process.env.CURSOR_API_KEY!,
  model: { id: "composer-2.5" },
  local: { cwd: "/path/to/workspace" },
});

const run = await agent.send("Analyze this codebase");

for await (const event of run.stream()) {
  console.log(event);
}
```

Production wiring: map **Agent 1** and **Agent 2** to separate `Agent` sessions (or SDK runs) with distinct system prompts, tool allowlists, and playground `PROGRAM_ROOT` injected from `/vs-auto-execute` resolution.

## 3. Connect the sidebar

- Sidebar sends prompts → extension controller.
- Controller invokes `agent.send(prompt)` (or routes to Python orchestrator for multi-agent).
- Stream agent events → sidebar UI (activity, tool calls, evidence thumbnails).
- Use VS Code APIs for editor-specific interactions (open chunk doc, acceptance HTML preview, checklist webview).

**Important:** The SDK's local runtime accesses files and runs tools itself; the extension owns the VS Code UI and editor integration. Local tool calls can execute automatically — configure **approval hooks** or sandboxing before exposing to interns/operators.

## 4. Vibe Scaler–specific UI surfaces (planned)

| Surface | Purpose |
|---------|---------|
| Hub panel | Mirror `/vs-hub` menu + quick-index tree |
| Chunk rail | Active program, chunk, status from `status.json` |
| Evidence viewer | Contact sheets, WebRTC clips, test logs |
| Checklist | Human checkpoint after Agent 2 pre-verification |
| Agent controls | Pause / resume / escalate to orchestrator (Process C) |

## 5. Compatibility

- **Cursor:** primary target; SDK + skills already used in repo.
- **VS Code:** same extension manifest; document any Cursor-only SDK features and degrade gracefully or require Cursor for full agent runtime.

Implementation code for the extension will live outside this spec folder (e.g. future `packages/vibe-scaler-extension/`); this directory stays **design and algorithms only**.
