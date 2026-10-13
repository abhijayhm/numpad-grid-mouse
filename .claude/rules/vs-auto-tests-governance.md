> Auto-tests stack after executionist Turn B — handoff, profiles, results, checklist

# Executionist auto-tests governance

## Chain position

```text
executionist Turn B (implement) → write handoff + spec.json → vs-auto-tests → executionist Turn C
```

## Kit

`vibe-scaler/vs-auto-tests/` — profiles, `run-tests`, `generate-checklist`, `spec.schema.json`.

## Handoff file

`PROGRAM_ROOT/auto-tests/last-handoff.json` — executionist **must** write before auto-tests. Auto-tests **must** read first.

## Expected user input (announce every takeover)

Copy from handoff `expected_user_input`. Examples:

- **None** — pytest/Playwright in CI locally
- **Simulator/device** — Detox / Flutter integration_test
- **Secrets** — `.env`, API keys (never commit)
- **Proceed** — after blocked item resolved

## Framework policy

Use profile for `target`; default UI E2E = **Playwright**. Do not swap frameworks without ideator plan update.

Pair with skill **`vs-auto-tests`** or **`vs-auto-tests-NN-<short>`**.

## Voice

**`vibe-scaler/VOICE.md`** — explain what each test run checks in plain language; name pytest/Playwright/etc. right after. Always state **`expected_user_input`** from handoff in words humans understand.

