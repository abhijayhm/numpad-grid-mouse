# UI and features — Vibe Scaler sidebar extension

Status: **ideation / product spec**. Implementation: TypeScript VS Code extension + webview sidebar; agent backend `@cursor/sdk` ([vscode-sdk-architecture.md](./vscode-sdk-architecture.md)).

Design goal: feel like **Cursor’s agent chat** (modes, attachments, streaming) while enforcing **Vibe Scaler governance** (playground on disk, deterministic pipeline starts, three internal agents per session).

---

## Layout (Activity Bar → WebviewView)

```text
┌─────────────────────────────────────┐
│ Vibe Scaler                    [≡]  │  ← session title = Agent 3 chat id (short)
├─────────────────────────────────────┤
│ Quick tree (from quick-index)       │  ← read-only; click opens playground files
│  · NN_foo | executing | chunk-02    │
├─────────────────────────────────────┤
│                                     │
│  Chat stream (FOR_USER only)        │  ← Agent 3 messages; user bubbles
│                                     │
├─────────────────────────────────────┤
│ [Agent mode ▼] [Model ▼] [📎]       │  ← mirror Cursor: mode, model, attachments
│ ┌─────────────────────────────────┐ │
│ │ Message input                   │ │
│ └─────────────────────────────────┘ │
│ [New chat] [Run chunk] [Pause]      │
├─────────────────────────────────────┤
│ MCP ●  Cursor usage ···        [?]  │  ← green/yellow/red dot (Agent 2 health)
└─────────────────────────────────────┘
```

| Control | Behavior |
|---------|----------|
| **New chat** | New Agent 3 session → new `docs/vibe-scaler/history/<chat-id>/` + new SQLite session row |
| **Run chunk** | Calls coded `startPipeline()` → `/vs-auto-execute` → spawn Agents 1 & 2 |
| **Pause** | Stops Agents 1 & 2; Agent 3 remains interactive |
| **Quick tree** | Loaded from playground `quick-index.md` (refresh on hub rules / manual refresh) |
| **Attachments** | Same UX as Cursor chat: files/images passed into Agent 3’s SDK turn (and forwarded in metadata to 1/2 when relevant) |
| **Agent mode** | Same option set as Cursor agent modes where SDK exposes them (e.g. agent vs ask) — Agent 3 only in sidebar |

---

## Theming

- Webview CSS uses **VS Code theme variables** only (`--vscode-editor-background`, `--vscode-foreground`, `--vscode-button-background`, etc.).
- Icons: Codicons where possible for consistency with VS Code/Cursor chrome.
- No separate light/dark assets — inherit workbench theme automatically.

---

## Sessions and chats

| Concept | Storage | Notes |
|---------|---------|--------|
| **Sidebar chat** | One **Agent 3** session | User-visible thread |
| **Agent 1 / 2** | Sub-sessions linked to same `chat_id` | Not separate sidebar tabs; optional “Activity” debug panel later |
| **New chat window** | New `chat_id` | Independent pipeline state; user can run multiple programs in parallel only if policy allows (default: one active pipeline per workspace) |
| **Resume chat** | SQLite + `history/<chat-id>/` | Reload sidebar → restore input draft + last FOR_USER messages + pipeline state |

---

## State database (SQLite)

**Location (proposed):** workspace-local, gitignored — e.g. `.vibe-scaler/state.db` (or global per user in extension storage for multi-root).

**Why:** full chat transcripts, tool calls, and Agent 1/2 logs are too heavy for `history/`; SQLite keeps the sidebar fast and queryable.

### Suggested tables (ideation)

| Table | Purpose |
|-------|---------|
| `sessions` | `chat_id`, `workspace_root`, `created_at`, `title`, `active_program`, `active_chunk`, `pipeline_status` |
| `messages` | `id`, `chat_id`, `role` (user \| agent3_user \| agent1 \| agent2 \| system), `content`, `for_user` bool, `created_at` |
| `attachments` | `message_id`, `path` or `blob_ref`, `mime` |
| `agent_runs` | SDK run ids, agent slot (1/2/3), status, token usage snapshot |
| `pipeline_events` | deterministic events: `auto_execute`, `spawn_1`, `spawn_2`, `pause`, `resume`, `escalate` |

**Rendering rule:** sidebar lists only rows where `for_user = true` OR `role = user` OR content parsed from `<FOR_USER>` blocks.

**Export:** optional “copy session for goat” → writes sanitized markdown into `history/<chat-id>/summary.md`.

---

## On-disk history (simplified context)

Path: **`docs/vibe-scaler/history/<agent-3-chat-id>/`**

| File | Writer | Readers |
|------|--------|---------|
| `summary.md` | Agent 3 (prompted) + extension on milestone | Agents 1, 2, 3 on resync |
| `state.json` | Extension controller | Pipeline resume |
| `chunks/*.md` | Extension on segment boundaries | Agents when context truncated |

When SDK context is **out of sync**, all three agents get a **system prepend**: read `summary.md` + `state.json` before continuing.

Commit policy: `history/` may be committed for team visibility (unlike `.vibe-scaler/state.db`). Product decision: default **gitignore** `state.db`, **track** `docs/vibe-scaler/history/` optionally per team.

---

## Streaming and `<FOR_USER>`

1. Agent 3 stream arrives from SDK.
2. Extension parser:
   - If tags present: buffer outside `<FOR_USER>`, append inside to webview.
   - If no tags: treat entire stream as internal unless `channel=user` metadata set by controller.
3. Thinking blocks never appended to webview (match Cursor behavior).

---

## MCP status (bottom-right)

| Dot | Meaning |
|-----|---------|
| **Green** | OS MCP configured + health check passed (Agent 2) |
| **Yellow** | Configured but last action failed / retry |
| **Red** | Not configured or user declined |
| **Gray** | Web-only project; MCP not required |

Click → panel: which server, last error, “Re-run setup”, link to [ui-automation-mcp.md](./ui-automation-mcp.md).

Setup flow: consent dialog → extension writes MCP config for SDK → test action (e.g. screenshot or noop) → set green.

---

## Feature phases (implementation order)

| Phase | Deliverable |
|-------|-------------|
| **P0** | Sidebar webview, SQLite messages, Agent 3 only, manual `/vs-hub` paste routing |
| **P1** | New chat sessions, `history/<chat-id>/`, FOR_USER filtering |
| **P2** | Coded `startPipeline` + `/vs-auto-execute`, spawn Agent 1 |
| **P3** | Agent 2 spawn, review hook, pause on user message |
| **P4** | MCP onboarding + status dot, Agent 2 desktop tests |
| **P5** | Attachments + agent mode parity with Cursor SDK APIs |
| **P6** | Evidence viewer (contact sheets), checklist webview, cloud control plane |

---

## Out of scope for first build

- Mobile APK / ngrok (see [runtime-algorithm.md](./runtime-algorithm.md))
- Replacing playground files with chat-only state
- Letting Agent 1 or 2 post directly to the user stream

---

## Related kit paths

| Today | Extension adds |
|-------|----------------|
| `docs/idea-to-reality-playground/` | Unchanged — plans, acceptance, status |
| `vibe-scaler/prompts/pgoat-init.txt` | Link from welcome screen |
| `/vs-hub` skill | Agent 3 invokes same menu semantics in sidebar |
