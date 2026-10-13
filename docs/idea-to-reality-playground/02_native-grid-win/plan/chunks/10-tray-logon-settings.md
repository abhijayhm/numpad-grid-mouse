# Chunk 10 — Tray logon settings

## Deliverables

- Tray menu literal strings from `ui-literal-copy.md`
- Commands: show grid, toggle arrow, open config folder, about, exit
- **Run at logon:** `HKCU\Software\Microsoft\Windows\CurrentVersion\Run` value `NumpadGridMouse` → path to exe
- Checkbox reflects registry on startup; toggle writes/deletes key
- Optional first-run balloon

## Autonomous

- **Batch:** E · **Skip proceed gate:** yes

## Test target

`filesystem`

## Verify commands

```powershell
dotnet build src/NumpadGridMouse.Win/NumpadGridMouse.Win.csproj -c Release
reg query HKCU\Software\Microsoft\Windows\CurrentVersion\Run /v NumpadGridMouse
```

## Tests to curate

- Manual: enable logon, sign out/in or reboot, tray returns

## Acceptance

- `verification/chunks/10-tray-logon-settings.html`

## Stop / blocked

- Corporate policy blocks Run key → document Task Scheduler alternative in README
