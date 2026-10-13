# Chunk 11 — Build publish

## Deliverables

- `scripts/publish-win.ps1` or `build.ps1` at repo root
- `dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true`
- Output `artifacts/NumpadGridMouse.exe`
- App icon `.ico` embedded

## Autonomous

- **Batch:** F · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
pwsh -File scripts/publish-win.ps1
.\artifacts\NumpadGridMouse.exe
```

## Tests to curate

- Manual: run published exe without SDK installed (same machine smoke)

## Acceptance

- `verification/chunks/11-build-publish.html`

## Stop / blocked

- Single-file trim breaks WPF → adjust `TrimMode` / disable trim
