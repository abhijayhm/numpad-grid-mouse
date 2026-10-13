# UI automation — Playwright and desktop MCP

When a **plan chunk** or acceptance item calls for testing a **real UI** (not only unit/API tests), the runtime picks a driver:

| Target | Default tool | When |
|--------|--------------|------|
| Web app in browser | **Playwright** | Chunk or `vs-auto-tests` profile says web/E2E; URLs in handoff |
| Windows desktop / native | **Windows MCP** | Chunk requires Windows UI, shell, or apps that are not browser-only |
| macOS desktop | **macOS MCP** | Same on Mac hosts or Mac workers |
| Linux desktop | **Linux computer-use MCP** | Same on Linux hosts or workers |

Agents do **not** swap tools without a plan update or an explicit escalation. Chunk doc and `auto-tests/last-handoff.json` should name the profile (`web` | `desktop-windows` | `desktop-macos` | `desktop-linux`).

## Reference implementations (integrate as MCP servers in the worker / local bridge)

| OS | Repository |
|----|------------|
| Windows | [cursortouch/windows-mcp](https://github.com/cursortouch/windows-mcp) |
| macOS | [CursorTouch/MacOS-MCP](https://github.com/CursorTouch/MacOS-MCP) |
| Linux | [agent-sh/computer-use-linux](https://github.com/agent-sh/computer-use-linux) |

## Agent 2 (Driver) usage

1. Read chunk acceptance + wireframe HTML for literal copy and regions.
2. After Agent 1 implementation and initial `/vs-auto-tests`, if the chunk flags **desktop UI** or **visual verification**:
   - Start the appropriate MCP session on the execution worker (cloud) or **local bridge** (when the app cannot run in cloud).
   - Perform user-like actions (launch app, click, type, navigate).
   - Run WebRTC capture in parallel (see [runtime-algorithm.md](./runtime-algorithm.md) § C).
3. Compare contact sheets and frames to wireframes; file refinements as in-scope sub-tasks or `/vs-jr-goat` sub-chunks per governance.
4. On ambiguity or missing scope → pause Process C → human orchestrator (not Agent 2 deciding product).

## Security and approvals

- MCP tools can drive the real desktop. Require **session approval** hooks in the extension (align with `@cursor/sdk` local tool policy).
- Local bridge must be authenticated; cloud workers must be isolated per tenant/session.
- Never store secrets in playground artifacts; use existing handoff `expected_user_input` patterns from `vs-auto-tests`.

## Kit today vs extension tomorrow

**Today:** skills + Playwright/pytest via `vibe-scaler/executionist-auto-tests/`.  
**Extension:** same governance files on disk; extension + backend add Agent 2, MCP routing, WebRTC evidence, and Process C UI.
