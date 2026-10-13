# vs-auto-tests — canonical stack

Invoked **immediately after** executionist **Turn B** (implement) for the same chunk. Continuation via `PROGRAM_ROOT/auto-tests/last-handoff.json`.

## Target → framework

| Target | Primary framework | Purpose |
|--------|-------------------|---------|
| **Backend / API** | **pytest** | Unit, integration, API, DB |
| **Web** | **Playwright** | UI + E2E + browser |
| **React Native** | **Jest + RNTL + Detox** | Component + device E2E |
| **Flutter** | **flutter_test + integration_test** | Widget + device E2E |
| **Electron** | **Vitest/Jest + Playwright Electron** | Logic + desktop UI/E2E |
| **Filesystem / repo** | **Node/Python assertions** | Required files, structure, artifacts |

**Default UI E2E:** Playwright wherever applicable.

## Mapping

```text
Django/Python       → pytest
Node backend        → Vitest
React web           → Playwright
React Native        → Jest + RNTL + Detox
Flutter             → flutter_test + integration_test
Electron            → Vitest + Playwright
```

## Layout

```text
vs-auto-tests/
├── spec.schema.json
├── profiles/
├── adapters/
├── run-tests.ps1
├── run-tests.sh
├── collect-results.ps1
└── generate-checklist.ps1
```

Per program chunk run: `PROGRAM_ROOT/auto-tests/<chunk-id>/spec.json` + `results.json` + `generated-checklist.md`.

## Flow

```text
spec.json → detect target → profile → generate/update tests → run framework
         → filesystem assertions → results.json → generated human checklist
```

Skill: **`/vs-auto-tests`** (reads handoff + runs this kit).
