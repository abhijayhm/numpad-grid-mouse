# Modes & hotkeys

## Global (RegisterHotKey — no admin)

| Chord | Action |
|-------|--------|
| **Ctrl+Shift+G** | Toggle **grid overlay** (show / hide). If grid already visible, optional **drill center (5)** — match Python “repeat toggle drills” OR reset per config flag `behavior.toggle_drill_center` default `true`. |
| **Ctrl+Shift+A** | Toggle **arrow pointer mode** (listening). Exits grid if open. |

Config keys (defaults):

```json
"hotkeys": {
  "toggle_grid": "Ctrl+Shift+G",
  "toggle_arrow_mode": "Ctrl+Shift+A"
}
```

Registration uses parsed modifiers + VK; user rebinding in v1 = edit JSON + restart.

## Grid mode (overlay visible)

| Key | Action |
|-----|--------|
| **1–9** | Select numpad region (7-8-9 top row) |
| **Enter** | Left click at cell center |
| **Ctrl+Enter** | Right click at center |
| **Space** | Move only |
| **Backspace** | Up one grid level |
| **↑↓←→** | Scroll at current region |
| **Esc** | Hide grid |

## Arrow pointer mode (listening HUD on)

| Key | Action |
|-----|--------|
| **↑↓←→** | Move cursor by `behavior.arrow_step_px` (default 8); hold = repeat via system key repeat |
| **Enter** | Left click |
| **Enter** (second within 450 ms) | Double click |
| **Shift+F10** | Right click (context menu key) |
| **Esc** | Exit arrow mode |
| **Ctrl+Shift+A** | Exit arrow mode (toggle off) |

Grid hotkey **cannot** open grid while arrow mode active without first closing arrow mode (or auto-close arrow — **auto-close arrow when grid opens**).

## Mutual exclusion

Opening grid → close arrow mode + hide HUD.  
Opening arrow mode → hide grid + pop grid stack.
