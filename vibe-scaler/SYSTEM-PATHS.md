# Vibe Scaler — system paths (read with any NN skill)

Host repo paths are **relative to repository root** unless noted.

**Voice (how agents explain things):** `vibe-scaler/VOICE.md` — read with every skill.

## Playground

| Path | Purpose |
|------|---------|
| `docs/idea-to-reality-playground/CONTEXT.md` | **Hub** — short summary; points to sub-contexts |
| `docs/idea-to-reality-playground/quick-index.md` | **Maintained tree** — programs, chunks, operators, scores (`/vs-hub` refreshes) |
| `docs/idea-to-reality-playground/index.md` | Registry of all programs |
| `docs/idea-to-reality-playground/users/<kebab>/onboarding.json` | User completed flow / declared done |
| `docs/idea-to-reality-playground/users/<kebab>/profile.json` | 3 MCQs → how agents explain things |
| `docs/idea-to-reality-playground/archives/NN_<short>/` | **archived** programs (moved) |
| `docs/idea-to-reality-playground/context-attendance-log.md` | Plan-read / adherence (playground-wide) |
| `docs/idea-to-reality-playground/NN_<short>/` | One program (replace `NN`, `<short>`) |

## Per program `PROGRAM_ROOT = docs/idea-to-reality-playground/NN_<short>/`

| Path | Purpose |
|------|---------|
| `PROGRAM_ROOT/index.md` | Status: ideation \| executing \| done \| blocked |
| `PROGRAM_ROOT/original-prompts.md` | Canonical idea text; link operator takeovers |
| `PROGRAM_ROOT/plan/index.md` | Plan tree |
| `PROGRAM_ROOT/plan/chunks/*.md` | Chunk specs (or `subagents/`) |
| `PROGRAM_ROOT/implementation-index.md` | Chunk order + dependencies |
| `PROGRAM_ROOT/implementation-checklist.md` | Throttle; agent + human boxes |
| `PROGRAM_ROOT/implementation-log.md` | Append-only execution history |
| `PROGRAM_ROOT/context/summary.md` | Program rollup (always maintained) |
| `PROGRAM_ROOT/operators/<kebab>/context.md` | Per-person briefing + session notes |
| `PROGRAM_ROOT/operators/<kebab>/handoffs.md` | Takeovers (git-reviewable) |
| `PROGRAM_ROOT/operators/index.md` | Who is active on what |
| `PROGRAM_ROOT/performance/scores.json` | Per-chunk session scores + **average** |
| `PROGRAM_ROOT/verification/ideator.html` | Ideator hub — iframes all chunk acceptance pages |
| `PROGRAM_ROOT/verification/chunks/<chunk-id>.html` | One acceptance page per chunk (executionist works here) |
| `PROGRAM_ROOT/verification/acceptance-criteria.json` | Full criteria (optional aggregate) |
| `PROGRAM_ROOT/verification/acceptance-state.json` | Intern ticks (aggregate or per-chunk merge) |
| `PROGRAM_ROOT/auto-tests/last-handoff.json` | Executionist → auto-tests handoff (read first) |
| `PROGRAM_ROOT/auto-tests/<chunk-id>/spec.json` | Test target + verify commands |
| `PROGRAM_ROOT/auto-tests/<chunk-id>/results.json` | Agent test run output |
| `PROGRAM_ROOT/auto-tests/<chunk-id>/generated-checklist.md` | Pre-filled checklist for Turn C |
| `PROGRAM_ROOT/status.json` | Program status enum |
| `PROGRAM_ROOT/reviews/chunk-<id>.md` | Goat code review (index-extractor + git) |
| `PROGRAM_ROOT/goat-verification-complete.md` | MD5 digest only for close |
| `vibe-scaler/vs-auto-tests/` | Profiles, run-tests, generate-checklist |

## Big-idea split (before multi-program ideation)

| Path | Purpose |
|------|---------|
| `goat-planner-tmp/original-big-idea.md` | Full vision before split |
| `goat-planner-tmp/split-index.md` | Planned child programs + chunk budgets |
| `goat-planner-tmp/README.md` | How split maps to multiple `/vs-ideator` runs |

## Skills (Cursor / Antigravity)

| Path | Purpose |
|------|---------|
| `.cursor/skills/vs-hub/SKILL.md` | **Hub menu** — `/vs-hub` |
| `.cursor/skills/vs-jr-goat/SKILL.md` | After tests, before checklist — suggestions → sub-chunks |
| `.cursor/skills/vs-performance/SKILL.md` | Score chunk sessions + average |
| `.cursor/skills/vs-ideator/SKILL.md` | New program ideation |
| `.cursor/skills/vs-planner/SKILL.md` | Split oversized ideas |
| `.cursor/skills/vs-checker/SKILL.md` | Acceptance HTML + ingest |
| `.cursor/skills/vs-executionist/SKILL.md` | Execution fallback |
| `.cursor/skills/vs-auto-tests/SKILL.md` | After Turn B — canonical test stack |
| `.cursor/skills/vs-auto-tests-NN-<short>/SKILL.md` | Program-pinned auto-tests |
| `.cursor/skills/vs-harness/SKILL.md` | Multi-IDE setup test |
| `vibe-scaler/scripts/verify-vibe-scaler-setup.ps1` | Automated verify (Antigravity / Claude / Codex mirrors) |
| `.cursor/skills/vs-ideator-NN-<short>/SKILL.md` | Program pin — **entrypoint for ideation refine** |
| `.cursor/skills/vs-executionist-NN-<short>/SKILL.md` | Program pin — **entrypoint for execution** |
| `.cursor/skills/vs-checker-NN-<short>/SKILL.md` | Program pin — checker |

## Rules (read every execution / ideation turn)

| Path | Purpose |
|------|---------|
| `.cursor/custom-rules/vs-hub-rule.mdc` | Playground contract |
| `.cursor/custom-rules/vs-hub-hub-governance.mdc` | Hub, quick-index, users, onboarding |
| `.cursor/custom-rules/ideator-governance.mdc` | Ideator copilot |
| `.cursor/custom-rules/ideator-checker-governance.mdc` | Acceptance + ingest |
| `.cursor/custom-rules/executionist-governance.mdc` | Execution loop |
| `.cursor/custom-rules/executionist-monitor.mdc` | Checklist + operators |

## Stack hints (ideator only, when repo has kit)

| Path | Purpose |
|------|---------|
| `project-utils/abstract-batteries-included/README.md` | Batteries-included patterns |
| `project-utils/abstract-batteries-included/ARCHITECTURE.md` | Monorepo, Django, deploy |

## Personal learning (optional, not required ingest)

| Path | Purpose |
|------|---------|
| `docs/learning/<operator-name>/report.md` | User-maintained learning log (any IDE) |
