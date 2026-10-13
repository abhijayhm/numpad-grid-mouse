# Literal UI copy (checker wireframes)

## Tray menu

| Item | Text |
|------|------|
| Open grid | `Show grid (Ctrl+Shift+G)` |
| Arrow mode | `Arrow pointer mode (Ctrl+Shift+A)` |
| Settings | `Settings…` |
| Run at logon | `Run at Windows logon` (checkbox) |
| About | `About Numpad Grid Mouse` |
| Exit | `Exit` |

## Tray tooltip

`Numpad Grid Mouse — Ctrl+Shift+G grid · Ctrl+Shift+A arrows`

## Listening HUD (arrow mode)

- Visual only: **green dot** (no text required).
- Tooltip on hover: `Arrow pointer active — Enter click · Enter×2 double-click · Shift+F10 right-click · Esc exit`

## Grid overlay HUD (optional corner chip — top-left)

`Depth {n} · Region {key}` — same semantics as Python HUD; toggle `hud.enabled` in config.

## Grid cell labels

Digits **1** through **9** centered in each cell (Segoe UI Semibold).

## Settings window (v1 minimal)

Title: `Numpad Grid Mouse Settings`  
Sections: `Hotkeys` (read-only display + “Edit config.json” button), `Behavior`, `Appearance`  
Button: `Open config folder`  
Button: `Save and restart` (if in-app edits later; v1 may open folder only)

## About dialog

Title: `About`  
Body: `Numpad Grid Mouse` + version + `Keyboard grid and arrow pointer for Windows.`

## First-run balloon (optional)

`Numpad Grid Mouse is running in the tray. Press Ctrl+Shift+G for the grid.`
