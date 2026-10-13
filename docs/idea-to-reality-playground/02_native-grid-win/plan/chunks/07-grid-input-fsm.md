# Chunk 07 — Grid input FSM

## Deliverables

- `GridInputController` + state machine: Hidden ↔ Visible
- `WH_KEYBOARD_LL` while visible; map keys per `modes-and-hotkeys.md`
- Wire to `GridModel` + `PointerService` + overlay refresh
- Toggle drill-center behavior when grid visible and `Ctrl+Shift+G` again (config)
- Esc hides; mutual exclusion closes arrow mode if open

## Autonomous

- **Batch:** C · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet build src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj -c Release
# Manual parity script in docs/idea-to-reality-playground/02_native-grid-win/plan/grid-smoke.md (create in chunk 12 or here)
```

## Tests to curate

- Manual: 5→5→Enter clicks; Backspace up; scroll arrows

## Acceptance

- `verification/chunks/07-grid-input-fsm.html`

## Stop / blocked

- Keys leak to foreground app → increase suppress list
