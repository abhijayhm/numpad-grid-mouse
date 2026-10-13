# Summary — Native Windows Grid & Arrow Mouse

Replace the Python/PyInstaller utility with a **single .NET 8 WPF tray app** (no admin): global **`Ctrl+Shift+G`** opens a **Fluent-style 3×3 recursive grid** (parity with `grid_model.py`); **`Ctrl+Shift+A`** toggles **arrow pointer mode** (move with arrows, Enter click, double-Enter double-click, Shift+F10 right-click) with a **bottom-left green listening dot**. Config in `%APPDATA%\NumpadGridMouse\config.json` (migrate keys from Python). Run at logon via tray or HKCU Run.

**Execution:** Orchestrator pre-approved **autonomous batches** in `plan/autonomous-execution.md` — agent may implement multiple chunks per session with lightweight human smoke only at batch boundaries.
