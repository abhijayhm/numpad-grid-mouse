# Ideation — three-agent extension runtime (per chat session)

Status: **ideation only** (no implementation in this repo yet). Complements [plan-recap.md](./plan-recap.md), [runtime-algorithm.md](./runtime-algorithm.md), and [ui-and-features.md](./ui-and-features.md).

## Roles (maker / supervisor / broker)

Each **Agent 3 chat session** runs three TypeScript agents (via `@cursor/sdk` or equivalent), with roles fixed at spawn time in the system prompt.

| Agent | Role | Analogy | Who talks to the user |
|-------|------|---------|------------------------|
| **Agent 3** | Master broker, bidirectional communicator | Orchestrator in the UI | **Yes** — only Agent 3’s user-visible stream |
| **Agent 1** | Executionist (maker) | `/vs-executionist` hands | No — internal |
| **Agent 2** | Coordinator, monitor, supervisor | Checker / driver / approver (loose) | No — escalates to 3 when needed |

**Spawn tree**

```text
Agent 3 (session root, one per sidebar chat)
  └── spawns Agent 1 (executionist)
        └── spawns Agent 2 (monitor + coordinator for Agent 1’s work)
```

- **Agent 3** orchestrates Agent 1 (does not implement chunks itself).
- **Agent 2** watches Agent 1’s context and chat; chooses next action after Agent 1 completes a unit of work.
- Not a rigid maker-checker gate: Agent 2 can run extra tests, drive MCP UI automation, or escalate without blocking on a formal “approve” every time.

**Governance unchanged:** playground plan, goat close, and human **proceed** / checklist rules from `vibe-scaler/WORKFLOW.md` still apply. Agents execute; humans (orchestrator + goat) decide scope.

---

## User-visible language (Agent 3)

Agent 3 is instructed at session start:

- **Hide internal jargon** in the default stream (chunk IDs, handoff JSON, MCP tool names, “Agent 1 said…”, status enums) unless the user typed **`pro`**.
- **Thinking / reasoning** stays out of the user stream (SDK-private or internal channel).
- **User-facing text** is either:
  - the only tokens streamed to the sidebar, or
  - explicitly wrapped in `<FOR_USER>…</FOR_USER>` — the extension **strips** everything else before render.

Agents 1 and 2 never write directly to the sidebar; their outputs go to Agent 3 and to **history / state** (below).

---

## Deterministic pipeline triggers (coded, not improvised)

Pipeline advancement is **extension code**, not Agent 3 “deciding” to start work.

| Step | Who | What runs (deterministic) |
|------|-----|---------------------------|
| 1 | User + Agent 3 | Conversation, routing (`/vs-hub`, ideator, checker, etc.) via Agent 3 as broker |
| 2 | Agent 3 ends turn | Extension calls internal **`startPipeline()`** (name TBD) |
| 3 | Code | Invokes **`/vs-auto-execute`** resolution (next program + chunk from playground) |
| 4 | Code | Spawns **Agent 1** + **Agent 2** (or resumes them) with pinned `PROGRAM_ROOT` + chunk id |
| 5 | Agent 1 | Implements chunk + runs **initial automated tests** (project harness / `vs-auto-tests` profile) |
| 6 | Code | On Agent 1 **finished** signal → invokes **Agent 2** `reviewAndNext()` |
| 7 | Agent 2 | Chooses: more work on same chunk, MCP/visual pass, spawn refinement, re-invoke Agent 1, or **escalate to Agent 3** |
| 8 | Agent 3 | Presents human checkpoint (summary + checks) when policy says chunk is pre-verified |

Agent 3 does **not** skip step 3–4 by narrating “I’ll start build”; the UI **Start / Run chunk** (or auto after explicit user consent) calls the same code path every time.

---

## Testing split (Agent 1 vs Agent 2)

| | Agent 1 | Agent 2 |
|---|---------|---------|
| **Default** | Runs automated tests required by chunk + `last-handoff.json` / `vs-auto-tests` | Optional second pass on same harness |
| **Choice** | Must run initial suite after implement | May skip, re-run, or extend (e.g. Playwright + desktop MCP) |
| **Override** | — | Agent 3 can instruct Agent 2 to test (user or policy) |

Desktop UI automation (Windows / macOS / Linux MCP) is **Agent 2’s** tool surface when the chunk or handoff requests it — see [ui-automation-mcp.md](./ui-automation-mcp.md).

---

## User interrupts Agent 3

When the user sends a message to **Agent 3** while 1 or 2 are running:

1. Extension sends **pause / cancel** to Agent 1 and Agent 2 (cooperative stop at next tool boundary).
2. Agent 3 handles the user turn (broker: ideator, checker, explain, etc.).
3. On user **Run** / **Resume pipeline**, code **re-spawns or resumes** 1 and 2 from saved workflow state + history snapshot.

All three agents can read the same **on-disk history** if context drifts (see below).

---

## History on disk (simplified context)

**Canonical slim history** (for all three agents when resyncing):

```text
docs/vibe-scaler/history/<agent-3-chat-id>/
  summary.md          # rolling human-readable recap (Agent 3 maintains)
  chunks/             # optional segment files per pipeline segment
  state.json          # pointers: program, chunk, last agent statuses
```

- **`<agent-3-chat-id>`** = one sidebar chat session (new chat → new folder).
- Full message bodies and tool traces live in **SQLite** (see [ui-and-features.md](./ui-and-features.md)); history folder is the **compressed** trail agents re-read when “too much out of sync.”
- Playground (`docs/idea-to-reality-playground/`) remains workflow truth; history is **session orchestration**, not a replacement for plans or acceptance JSON.

---

## Agent 3 as skill router

Agent 3 is the **only** agent that maps user intent to Vibe Scaler entrypoints:

- Menu and tree: **`/vs-hub`**
- Lost / next step: **`/vs-auto-execute`**
- New idea, mockups, build, test, polish, score, review, sign-off: corresponding **`/vs-*`** skills (extension invokes the same contracts the kit defines today)

Agent 1 and Agent 2 stay on **active chunk execution** unless escalated.

---

## Agent 2 — MCP onboarding (per machine)

On extension **first load** (or workspace trust):

1. Detect OS → wire [Windows](https://github.com/cursortouch/windows-mcp) / [macOS](https://github.com/CursorTouch/MacOS-MCP) / [Linux](https://github.com/agent-sh/computer-use-linux) MCP server config for the SDK worker.
2. Prompt once: **“Can I set up desktop automation for this machine?”** — no silent install.
3. Health check → status in UI: **green dot** bottom-right = MCP ready; yellow = degraded; red = not configured.

Agent 1 does not own MCP; Agent 2 does.

---

## VS Code / Cursor parity

- **Theme:** use VS Code theme tokens (`var(--vscode-*)`) in the webview — same look as built-in panels; no custom design system unless branding requires it.
- **Cursor API usage:** follow the same patterns as the current Cursor chat surface where documented (agent mode selector, model/options, attachments) — extension hosts a **parallel** sidebar, not a fork of Cursor internals.
- **Usage / billing display:** surface Cursor usage the same way the product does when APIs expose it (placeholder in UI spec until wired).

---

## Mapping to earlier blueprint

| Earlier name | This ideation |
|--------------|----------------|
| Process A — Agent 1 | Agent 1 (executionist) |
| Process B — Agent 2 | Agent 2 (supervisor + MCP + visual loop) |
| Process C — Chat / IPC | Agent 3 + extension controller + SQLite + history folder |

Cloud SaaS, WebRTC contact sheets, and mobile remote control remain in [runtime-algorithm.md](./runtime-algorithm.md); this doc is the **per-IDE chat session** shape.
