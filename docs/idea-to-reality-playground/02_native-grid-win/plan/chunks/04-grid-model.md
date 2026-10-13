# Chunk 04 — Grid model

## Deliverables

- `Grid/GridModel.cs` — port `grid_model.py`:
  - 3×3 regions, numpad mapping 1–9
  - Recursion stack, `select_region`, `go_up`, `current_region`, max depth from config
- `NumpadGridMouse.Win.Tests` with tests ported from `test_grid_model.py`

## Autonomous

- **Batch:** B · **Skip proceed gate:** yes

## Test target

`backend`

## Verify commands

```powershell
dotnet test src/NumpadGridMouse.Win.Tests/NumpadGridMouse.Win.Tests.csproj --filter GridModel -c Release
```

## Tests to curate

- Automated: center selection, depth limit, backstack
- Manual: none

## Acceptance

- `verification/chunks/04-grid-model.html`

## Stop / blocked

- Python test failure when ported → fix model, don't change spec
