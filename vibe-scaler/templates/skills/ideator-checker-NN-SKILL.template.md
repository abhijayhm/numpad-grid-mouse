---
name: vs-checker-{{NN}}-{{SHORT}}
description: >-
  Checker entrypoint for {{PROGRAM_ID}}. ideator.html hub + per-chunk HTML; ingest JSON.
program_id: {{PROGRAM_ID}}
program_root: docs/idea-to-reality-playground/{{PROGRAM_ID}}/
---

# Ideator checker {{NN}} — {{SHORT}} (entrypoint)

**PROGRAM_ROOT** = `docs/idea-to-reality-playground/{{PROGRAM_ID}}/`

## Mandatory read first

1. `vibe-scaler/SYSTEM-PATHS.md`
2. `vibe-scaler/VOICE.md`
3. `.cursor/custom-rules/ideator-checker-governance.mdc`
4. `.cursor/skills/vs-checker/SKILL.md` — follow **all** phases (generate, refine, ingest)

## Outputs (this program)

| File | Role |
|------|------|
| `verification/chunks/<chunk-id>.html` | One page per chunk — executionist verifies here |
| `verification/ideator.html` | Single hub — iframe each chunk page for ideator review |
| `verification/acceptance-criteria.json` | Aggregate criteria |
| `verification/acceptance-state.json` | Intern ticks |

Generate **per-chunk HTML first**, then **`ideator.html`** from `vibe-scaler/templates/verification/ideator.html.template`.

## Voice (mandatory)

**`vibe-scaler/VOICE.md`** — describe what each screen should do in everyday language; wireframe/HTML paths after.
