# Chunk 02 — Global hotkeys

## Deliverables

- Hidden HWND + `HotkeyService` registering:
  - `Ctrl+Shift+G` → event `ToggleGrid`
  - `Ctrl+Shift+A` → event `ToggleArrowMode`
- `WM_HOTKEY` handled on UI thread; unregister on exit
- Debug log or trace when hotkey fires (remove noisy logs before ship)

## Autonomous

- **Batch:** A · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet build src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj -c Release
# Manual: run app, press chords, observe trace / temporary MessageBox in DEBUG only
```

## Tests to curate

- Automated: unit test `HotkeyParser` modifier/VK mapping if extracted
- Manual: both hotkeys work when another app focused

## Acceptance

- `verification/chunks/02-global-hotkeys.html`

## Stop / blocked

- `RegisterHotKey` returns false → surface HRESULT in log; try alternate MOD_NOREPEAT
