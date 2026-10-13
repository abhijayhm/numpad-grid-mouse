> One-pass ingest table — which custom-rules to Read and when (loaded on demand)

# Rule router — ingest table

**How to use:** Scan the **When** column for the current task phase. **Read** only the listed `.cursor/custom-rules/<file>` entries (plus `graphify.mdc` when present). Do not Read the whole folder.

Skills (`.cursor/skills/`) and scripts (`.cursor/scripts/`) are referenced here but loaded by path when the row says so.

## Ingest table

| When (task phase) | Read |
|-------------------|------|
| Cold start, new session, unfamiliar repo, “how does this project work?” | `project-index.mdc` |
| Explore / edit / run / deploy / refactor in repo (any substantive repo work) | `graphify-rule.mdc` |
| Architectural, dependency, cross-file, or navigation questions | `graphify-rule.mdc`, `graphify.mdc` (if present) |
| After creating, modifying, deleting, moving, or renaming repo files | `graphify-auto.mdc` |
| Before modifying code (non-trivial edits) | `verify-code-update.mdc` |
| User says plan first / points planner / large prompt → numbered steps before code | `points-planner.mdc` + skill `.cursor/skills/points-planner/SKILL.md` |
| Vibe Scaler — hub menu / onboarding / quick-index | skill **`vibe-scaler`** + `vibe-scaler-hub-governance.mdc` + `docs/idea-to-reality-playground/CONTEXT.md` |
| Vibe Scaler — chunk session scoring / average | skill **`vs-performance`** or **`vs-performance-NN-<short>`** |
| Vibe Scaler — oversized idea / split programs (>15 chunks) | skill `vs-planner` + `goat-planner-tmp/` |
| Vibe Scaler — new idea / ideation | `ideator-governance.mdc` + skill `vs-ideator` (if installed) |
| Vibe Scaler — acceptance HTML/JSON / ingest verification | `ideator-checker-governance.mdc` + skill `vs-checker` or `vs-checker-NN-<short>` |
| Vibe Scaler — **execute** program / plan / checklist | `executionist-monitor.mdc` + `executionist-governance.mdc` + skill `vs-executionist` or `vs-executionist-NN-<short>` |
| Vibe Scaler — auto-tests after chunk implement (handoff) | `vs-auto-tests-governance.mdc` + skill `vs-auto-tests` or `vs-auto-tests-NN-<short>` + read `auto-tests/last-handoff.json` |
| Vibe Scaler — jr goat refinements (after tests, before checklist) | `vs-jr-goat-governance.mdc` + skill **`vs-jr-goat`** or **`vs-jr-goat-NN-<short>`** |
| Vibe Scaler — what to run next / traverse playground | skill `vs-auto-execute` or `vs-auto-execute-NN-<short>` |
| Vibe Scaler — code review (per chunk, git + index) | `code-reviewer-governance.mdc` + skill `vs-code-reviewer` or `vs-code-reviewer-NN-<short>` |
| Vibe Scaler — goat final close (MD5 secret) | skill `vs-goat` |
| Vibe Scaler — test harness / verify IDE mirrors / setup | skill `vs-harness` → run `vibe-scaler/scripts/verify-vibe-scaler-setup.ps1` |
| Vibe Scaler — playground `docs/idea-to-reality-playground/**` | `vibe-scaler-rule.mdc` + playground `index.md` |
| User asks to index / re-index, or `docs/indexing/` missing or stale | `project-index.mdc` + skill `.cursor/skills/index-extractor/SKILL.md` |
| Create/edit/delete/move/rename `.cursor/custom-rules`, `.cursor/skills`, or `.cursor/rules` | `agent-sync.mdc` + skill `.cursor/skills/agent-sync/SKILL.md` → run `bin/sync-agent-config.bat` |
| User asks to sync rules/skills across Cursor, Claude Code, Codex, Antigravity | `agent-sync.mdc` + skill `.cursor/skills/agent-sync/SKILL.md` |
| Spawning a subagent for repo work | This table → pass matched **filenames only** in the Task prompt |

## File index

| File | Scope |
|------|-------|
| `project-index.mdc` | Onboarding via `docs/indexing/` |
| `graphify-rule.mdc` | Graphify setup, navigation, block vs background |
| `graphify-auto.mdc` | Post-edit sync, `.graphifyignore`, incremental updates |
| `verify-code-update.mdc` | Pre-edit flowchart + pseudocode per subsystem |
| `agent-sync.mdc` | Mirror `.cursor/` rules/skills to Claude, Codex, Antigravity bundles |
| `points-planner.mdc` | Numbered implementation plans before multi-file work |
| `executionist-monitor.mdc` | Vibe Scaler plan execution + checklist adherence (with vibe-scaler install) |
| `graphify.mdc` | Installed by `graphify cursor install` — **not** in the bundle; lives here after install |

## graphify.mdc placement

1. `graphify cursor install` writes `.cursor/rules/graphify.mdc` by default.
2. **Move** it to `.cursor/custom-rules/graphify.mdc`.
3. Keep **only** `rule-router.mdc` under `.cursor/rules/`.
4. Do not symlink; do not hand-copy or recreate `graphify.mdc`.

## Typical phase order

```
Cold start     → project-index.mdc
Explore/nav    → graphify-rule.mdc [+ graphify.mdc]
Before edit    → verify-code-update.mdc
After edit     → graphify-auto.mdc
```

Re-read ingest only when the task phase changes (e.g. exploration → implementation → post-edit).

