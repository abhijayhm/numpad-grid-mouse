# Chunk 01 — Solution scaffold

## Deliverables

- `src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj` — .NET 8, `UseWPF`, `win-x64`
- `App.xaml` — single-instance mutex optional
- `Tray/TrayApplicationContext.cs` — placeholder tray icon (stock icon OK)
- `README` section in program folder or root pointer: how to `dotnet run`
- Solution file `NumpadGridMouse.sln` at repo root (optional) or project-only

## Autonomous

- **Batch:** A · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet build src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj -c Release
dotnet run --project src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj
```

## Tests to curate

- Automated: build succeeds
- Manual: tray icon appears; exit cleans up

## Acceptance

- Maps to `verification/chunks/01-solution-scaffold.html`

## Stop / blocked

- WPF not available on build agent → document SDK requirement
