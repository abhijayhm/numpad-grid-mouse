# Chunk 03 — Config AppData

## Deliverables

- `ConfigStore` reads/writes `%AppData%\NumpadGridMouse\config.json`
- Default JSON aligned with Python `config.py` + new keys:
  - `hotkeys.toggle_grid`, `hotkeys.toggle_arrow_mode`
  - `behavior.arrow_step_px`, `behavior.double_enter_ms`, `behavior.toggle_drill_center`
- Migrate unknown keys from existing Python config if file present (merge, don't delete)

## Autonomous

- **Batch:** A · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet test src/NumpadGridMouse.Win.Tests/NumpadGridMouse.Win.Tests.csproj --filter ConfigStore -c Release
# If tests project not yet created in 01, add minimal xUnit project in this chunk
```

## Tests to curate

- Automated: defaults written on first load; round-trip serialize
- Manual: edit JSON, restart, values apply

## Acceptance

- `verification/chunks/03-config-appdata.html`

## Stop / blocked

- Schema disagreement with ideator → stop, one question
