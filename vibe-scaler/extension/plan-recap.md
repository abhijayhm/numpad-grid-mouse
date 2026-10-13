# Vibe Scaler app — plan recap

High-level pipeline for the **autonomous engineering runtime** (extension + backend). Aligns with `vibe-scaler/WORKFLOW.md`; adds multi-agent execution, desktop UI automation, visual evidence, and cloud control plane.

1. **Trigger:** Vibe Scaler pipeline detects the next eligible chunk via `/vs-auto-execute`.
2. **Agent 1 — Executor:** Reads the plan and context, implements the chunk, and runs initial automated tests.
3. **Agent 2 — Driver:** Continues execution, coordinates testing, identifies defects, and creates small refinement sub-plans.
4. **Testing:** Use **Playwright** for browser apps and **Windows MCP / macOS MCP / Linux MCP** for real desktop interaction when the chunk requires it (see [ui-automation-mcp.md](./ui-automation-mcp.md)).
5. **Visual capture:** Record execution through WebRTC; sample at 3 FPS or 1 FPS; group frames into chronological 3-second or 10-second contact sheets.
6. **Visual refinement:** Agent 2 ingests screenshots, compares actual UI against approved wireframes and requirements, fixes issues, and retests.
7. **Repeat:** Continue until the entire chunk passes the agents' verification process.
8. **Human checkpoint:** Present a simple summary and specific application checks only after the chunk is complete and pre-verified.
9. **Agent 3 — Orchestrator / IPC:** Separate process for chat, communication between agents, monitoring, pausing, stopping, and human intervention.
10. **Remote access:** Authenticated ngrok/WebSocket connection; inspect agent activity and intervene through the app's mobile APK (proposed).
11. **Vibe Scaler governance:** Preserve plans, chunk status, handoffs, test results, reviews, acceptance records, Git approval and final closure rules.
12. **SaaS architecture:** Cloud control plane stores Vibe Scaler knowledge, project state, agent sessions, artifacts, permissions and usage data outside the code repository.
13. **Execution environments:** Managed cloud workers by default; optional secure local bridge for native apps or environments that require local access.
14. **Team scaling:** Centralize orchestration across projects, with you as engineering ideator, 1–2 experienced managers, and interns/operators handling prescribed execution and verification tasks.

## End goal

Reliably deliver work across **10–12 projects** without proportionally increasing senior engineering headcount, while retaining human authority over consequential decisions and final acceptance.
