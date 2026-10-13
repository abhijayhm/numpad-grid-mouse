# Chunk 06 — Grid overlay

## Deliverables

- `GridOverlayWindow.xaml` — full monitor bounds, transparent, topmost
- Fluent styling per `ui-literal-copy.md`: dark `#1E1E1E` lines `#3B82F6`, active cell fill `#2563EB33`, labels Segoe UI 28px white
- HUD chip top-left when `hud.enabled`
- `MonitorService` picks monitor under cursor on show

## Autonomous

- **Batch:** C · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet build src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj -c Release
# Manual: Ctrl+Shift+G shows grid on correct monitor
```

## Tests to curate

- Manual / visual: 3×3 lines, digits 1–9 readable on light and dark wallpaper

## Acceptance

- `verification/chunks/06-grid-overlay.html`

## Stop / blocked

- Multi-monitor misalignment → fix bounds before chunk 07
