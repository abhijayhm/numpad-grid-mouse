# Numpad Grid Mouse v2

## Run

```powershell
.\artifacts\NumpadGridMouse.exe
```

## Cheatsheet

**[CHEATSHEET.md](./CHEATSHEET.md)** · in app: **Ctrl+Shift+?** or **?** (during grid/arrow)

| | |
|--|--|
| **Ctrl+Shift+G** | Grid |
| **Ctrl+Shift+A** | Arrow mode + green dot |
| **Ctrl+Shift+?** | Help menu |

## Build / reinstall (one step)

```powershell
powershell -ExecutionPolicy Bypass -File scripts\install.ps1
```

Kills old instance, runs tests, publishes to `artifacts\`, starts the app.

Or publish only:

```powershell
powershell -File scripts\publish-win.ps1
```

Config: `%AppData%\NumpadGridMouse\config.json`
