# Chunk 09 — Listening HUD

## Deliverables

- `ListeningHudWindow` — 24×24 green dot, bottom-left 16px margin on active monitor
- Visible only in arrow mode; `Topmost`, no taskbar, click-through except tooltip
- Tooltip literal copy from `ui-literal-copy.md`

## Autonomous

- **Batch:** D · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet build src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj -c Release
# Manual: dot appears with arrow mode, vanishes on exit
```

## Tests to curate

- Manual: dot on correct monitor when moving between monitors

## Acceptance

- `verification/chunks/09-listening-hud.html`

## Stop / blocked

- Dot blocks clicks → enable WS_EX_TRANSPARENT hit test
