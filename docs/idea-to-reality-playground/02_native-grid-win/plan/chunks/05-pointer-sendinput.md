# Chunk 05 — Pointer SendInput

## Deliverables

- `Pointer/PointerService.cs`:
  - `MoveTo(int x, int y)`
  - `LeftClick`, `RightClick`, `DoubleClick`
  - `Scroll(dx, dy)` for grid arrows
- Use `SendInput` + `SetCursorPos`; DPI-aware (PerMonitorV2)

## Autonomous

- **Batch:** B · **Skip proceed gate:** yes

## Test target

`backend`

## Verify commands

```powershell
dotnet test src/NumpadGridMouse.Win.Tests/NumpadGridMouse.Win.Tests.csproj --filter PointerService -c Release
```

## Tests to curate

- Automated: struct sizes / mockable wrapper tests
- Manual: move to screen corner without throwing

## Acceptance

- `verification/chunks/05-pointer-sendinput.html`

## Stop / blocked

- UAC elevated apps won't receive clicks — document limitation in chunk 12
