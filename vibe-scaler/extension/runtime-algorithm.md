# Vibe Scaler Autonomous Engineering Runtime — Algorithmic Blueprint

## A. Core architecture

1. **Human / Senior Engineer:** Defines product vision, architecture, constraints, plans and acceptance criteria.
2. **Vibe Scaler:** Stores project knowledge, plans, chunks, dependencies, workflow status and acceptance rules.
3. **Process A — Agent 1:** Executes the initial implementation and automated tests for the active chunk.
4. **Process B — Agent 2:** Drives subsequent execution, desktop interaction, visual inspection, refinement and verification.
5. **Process C — Chat / IPC:** Connects both agents, displays activity, routes messages and enables intervention, pausing and stopping.
6. **VS Code Extension:** Provides the custom chat UI, project view, evidence viewer, checklists and agent controls.
7. **Python Backend + Cursor SDK:** Manages agent sessions, orchestration, tool execution, persistence and usage tracking.
8. **Testing Layer:** Integrates Playwright, Windows MCP / macOS MCP / Linux MCP and existing project test frameworks.
9. **Visual Evidence Layer:** Uses WebRTC recording, FPS-controlled frame extraction, timestamped images and stitched contact sheets.
10. **Cloud Control Plane:** Manages organizations, projects, workflow state, agent sessions, artifacts, permissions, budgets and audit records.
11. **Execution Layer:** Uses isolated cloud workers by default, with an optional authenticated local bridge when local applications or devices are required.
12. **Remote Interface:** Provides authenticated ngrok/WebSocket access and a proposed mobile APK for monitoring and intervention.

---

## B. End-to-end execution algorithm

1. Receive `/vs-auto-execute` or the appropriate pipeline trigger.
2. Identify the next eligible Vibe Scaler program and chunk.
3. Load the active plan, project context, acceptance criteria, wireframe and prior handoffs.
4. Validate prerequisites, dependencies, permissions and workflow state.
5. Start Agent 1 using the Cursor Python SDK (or `@cursor/sdk` from the extension host as applicable).
6. Agent 1 implements **only** its assigned chunk.
7. Agent 1 runs the initial automated tests.
8. Record implementation changes, test results, errors and handoff state.
9. Transfer execution to Agent 2 — Driver.
10. Agent 2 identifies the next permitted task from the plan and current state.
11. Select Playwright, Windows MCP, macOS MCP, or Linux MCP according to the application and testing requirement.
12. Launch or interact with the real application.
13. Execute the required user-like actions.
14. Record execution through the WebRTC-based capture service.
15. Extract frames according to the configured sampling rate.
16. Generate chronological contact sheets while preserving original timestamped images.
17. Attach relevant test results, logs, actions and application context.
18. Send selected visual evidence to Agent 2 for inspection.
19. Compare actual behavior against approved wireframes, literal UI copy, requirements and acceptance criteria.
20. Classify findings as defects, in-scope refinements, optional improvements or plan ambiguities.
21. If no unresolved issues remain, proceed to verification.
22. If an in-scope fix is required, create a small refinement task.
23. Implement the fix.
24. Rerun relevant automated tests.
25. Capture fresh visual evidence.
26. Reinspect the result and check for regressions.
27. Repeat steps 18–26 within configured retry, time and cost limits.
28. If scope is missing, interpretation conflicts, retries fail or a consequential decision is needed, pause and escalate.
29. Process C relays the human's decision to the appropriate agent.
30. Resume from the saved workflow state.
31. When verification passes, generate the chunk summary, evidence and human checklist.
32. Present the human with a simple explanation and specific application checks.
33. Record the human's checklist response.
34. Perform required acceptance HTML verification and checker ingestion.
35. Update Vibe Scaler status, logs, handoffs and performance records.
36. Request explicit authorization for Git commit/push wherever required.
37. Determine whether more eligible chunks remain.
38. If yes, return to step 2.
39. If no, verify all required deliverables, acceptance records and closure conditions.
40. Invoke the existing final-close procedure; **never** mark the program closed solely because an agent reports completion.

The playground remains the authoritative workflow record; agent chat history is not a substitute for its plans, status, handoffs and acceptance artifacts.

---

## C. Visual recording algorithm

1. Start recording the relevant application or desktop through WebRTC.
2. Associate the recording with the active task and execution session.
3. Select the required sampling mode:
   - **3 FPS:** 9 frames across 3 seconds.
   - **1 FPS:** 10 frames across 10 seconds.
4. Extract frames at the configured rate.
5. Preserve timestamps and original images.
6. Arrange frames chronologically into a contact sheet.
7. Associate frames with actions, test steps, logs and failures.
8. Send selected images and contextual metadata to Agent 2.
9. Increase capture density around failures or complex transitions where needed.
10. Use the visual findings to drive refinement and verification.

---

## D. Vibe Scaler governance algorithm

1. Use `/vs-hub` or `/vs-auto-execute` to identify the next action.
2. Use `/vs-planner` for ideas requiring more than 15 chunks per program.
3. Use `/vs-ideator` to create or refine the plan.
4. Use `/vs-checker-NN-*` to generate and approve acceptance wireframes.
5. Execute through `/vs-executionist-NN-*`.
6. Run `/vs-auto-tests-NN-*` after implementation.
7. Run the policy review and required retest loop.
8. Evaluate refinements through `/vs-jr-goat-NN-*`.
9. Invoke `/vs-code-reviewer-NN-*` when required.
10. Complete the human checklist and acceptance ingestion.
11. Record performance through `/vs-performance-NN-*` when applicable.
12. Preserve explicit Git authorization.
13. Close the program through `/vs-goat` only after the required conditions are satisfied.

**Authority (kit):** orchestrator decides in chat; goat closes; executionist does not freelance scope (`vibe-scaler/VOICE.md`).

---

## E. SaaS and team-scaling algorithm

1. Create an organization and its users, roles and permissions.
2. Register projects and connect their repositories.
3. Store Vibe Scaler knowledge and canonical orchestration state in the cloud.
4. Keep source code in the customer's repository.
5. Queue eligible chunks according to dependencies and project priority.
6. Allocate isolated execution workers.
7. Launch the required agent processes.
8. Track progress, failures, evidence, usage and cost.
9. Prevent conflicting tasks from modifying the same resources.
10. Apply project budgets, retry limits and concurrency controls.
11. Provide remote monitoring and human intervention.
12. Recover interrupted tasks from durable state.
13. Scale worker capacity according to workload and budget.
14. Measure accepted chunks, delivery time, defects, intervention hours and cost per completed project.
15. Increase project volume only as measured throughput and reliability support it.

---

## F. Final operating objective

1. You remain the core engineering ideator.
2. One or two experienced people manage engineering direction, exceptions and consequential decisions.
3. Agent 1 performs initial implementation and testing.
4. Agent 2 drives continued execution, desktop testing, visual refinement and verification.
5. Process C manages communication, visibility and intervention.
6. Interns and freshers handle prescribed operational work rather than carrying the primary engineering judgment burden.
7. Cloud orchestration coordinates projects, workers, usage and evidence.
8. Human effort concentrates on decisions and acceptance that cannot safely be automated.
9. Delivery capacity grows without requiring proportional growth in senior engineering headcount.
10. Success is measured by reliably accepted engineering output per experienced engineer at a lower total delivery cost.
