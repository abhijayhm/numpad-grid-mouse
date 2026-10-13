# Architecture — .NET 8 native host

## Goals

- One **tray-resident** process; **no Python**, **no admin** for normal use.
- **Low latency** grid + pointer; **Windows 11–like** visuals (Segoe UI, acrylic-friendly dark grid, subtle glow on active cell).
- **Two input modes** that do not fight: at most one of {grid active, arrow mode active}.

## Project layout

```text
v2/src/NumpadGridMouse.Win/
  NumpadGridMouse.Win.csproj          # WPF, win-x64, single-file publish
  App.xaml / App.xaml.cs              # DI-lite composition root
  Tray/
    TrayApplicationContext.cs         # NotifyIcon + menu
  Hotkeys/
    HotkeyService.cs                  # RegisterHotKey + WM_HOTKEY
    HotkeyIds.cs
  Config/
    AppConfig.cs, ConfigStore.cs      # %AppData%\NumpadGridMouse\config.json
  Grid/
    GridModel.cs                      # Port of grid_model.py
    GridOverlayWindow.xaml            # Full-screen per-monitor overlay
    GridInputController.cs            # WH_KEYBOARD_LL while grid visible
  Pointer/
    PointerService.cs                 # SetCursorPos + SendInput clicks/scroll
  ArrowMode/
    ArrowPointerController.cs         # LL hook + double-Enter detector
  Hud/
    ListeningHudWindow.xaml           # Bottom-left green dot + optional tooltip
  Monitors/
    MonitorService.cs                 # MonitorFromPoint, bounds
  Native/
    NativeMethods.cs, InputStructs.cs
```

## Runtime diagram

```text
[Tray icon] ──► App message loop (hidden HWND)
                    │
     RegisterHotKey ◄┘
                    ├─► Ctrl+Shift+G → GridOrchestrator.Toggle()
                    └─► Ctrl+Shift+A → ArrowPointerController.Toggle()

Grid visible:
  GridOverlayWindow (topmost, layered)
  WH_KEYBOARD_LL (suppress 1–9, arrows for scroll, etc.)
  GridModel stack + PointerService

Arrow mode:
  ListeningHudWindow (small, bottom-left, click-through except tooltip)
  WH_KEYBOARD_LL (arrows, Enter, Shift+F10)
  PointerService
```

## HWND & layering

- **Hotkey sink:** `HwndSource` or hidden `Window` with `WS_EX_NOACTIVATE`.
- **Grid overlay:** per active monitor `Window` `AllowsTransparency`, `WindowStyle=None`, `Topmost`, `ShowInTaskbar=false`, `Background=Transparent`; draw grid in `OnRender` or `Canvas`.
- **HUD:** 24×24 px circle, `#22C55E`, 2px white ring; position: **16px from bottom-left** of monitor containing cursor (or primary if unknown).

## Hooks policy

| State | Hook | Suppress keys? |
|-------|------|----------------|
| Idle | None | — |
| Grid | `WH_KEYBOARD_LL` | Yes for handled keys (1–9, Enter, Esc, arrows when scrolling) |
| Arrow mode | `WH_KEYBOARD_LL` | Yes for arrows, Enter, Shift+F10 while active |

Always unregister hook on mode exit / app exit.

## Python reference

Port behavior from repo root: `grid_model.py`, `input_handler.py` (state names), `virtual_pointer.py`, `config.py` keys — document deltas in chunk 12.

## Out of scope (v1)

- MSI installer, Store package
- Custom grid sizes (stay 3×3)
- macOS/Linux
