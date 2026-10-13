# Chunk 12 — Parity acceptance

## Deliverables

- `docs/idea-to-reality-playground/02_native-grid-win/plan/python-parity.md` — table Python vs Win
- Update root `README.md`: native app primary, Python deprecated/legacy
- `plan/grid-smoke.md` — 2-minute manual script for orchestrator
- Mark program `verification_pending` → human ticks HTML hub

## Autonomous

- **Batch:** F · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet test src/NumpadGridMouse.Win.Tests/NumpadGridMouse.Win.Tests.csproj -c Release
pwsh -File scripts/publish-win.ps1
```

## Tests to curate

- Manual: full grid-smoke + arrow-smoke on Windows 10/11

## Acceptance

- `verification/chunks/12-parity-acceptance.html` + all prior chunks ticked

## Stop / blocked

- Parity gap ideator must decide → append `original-prompts.md`, don't guess
