# Chunk 08 — Arrow pointer mode

## Deliverables

- `ArrowPointerController`:
  - Toggle via hotkey + tray
  - LL hook: arrows move by step; Enter click; double-Enter within `double_enter_ms`; Shift+F10 right click; Esc exit
- Closes grid when entering; does not register second global hotkey beyond chunk 02

## Autonomous

- **Batch:** D · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet build src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj -c Release
# Manual: Ctrl+Shift+A, move with arrows, double-Enter on desktop icon
```

## Tests to curate

- Automated: `DoubleEnterDetector` unit tests (timestamps)
- Manual: Shift+F10 opens context menu on desktop

## Acceptance

- `verification/chunks/08-arrow-pointer-mode.html`

## Stop / blocked

- Sticky keys interference → document Esc always exits
