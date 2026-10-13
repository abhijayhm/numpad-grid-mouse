# 02 — Native Windows Grid & Arrow Mouse

| Field | Value |
|-------|--------|
| **ID** | `02` |
| **Short** | `native-grid-win` |
| **Created** | 2026-10-13 |
| **Status** | See `status.json` — currently **ideation** (ready to execute) |
| **Skills** | `/vs-auto-execute-02-native-grid-win` · `/vs-ideator-02-native-grid-win` · `/vs-checker-02-native-grid-win` · `/vs-executionist-02-native-grid-win` · `/vs-auto-tests-02-native-grid-win` · `/vs-code-reviewer-02-native-grid-win` · `/vs-performance-02-native-grid-win` · `/vs-jr-goat-02-native-grid-win` |
| **Verification** | [ideator.html](./verification/ideator.html) · [chunks](./verification/chunks/) |
| **Autonomous runs** | [plan/autonomous-execution.md](./plan/autonomous-execution.md) |

## Summary

Native **.NET 8 WPF** tray host: `RegisterHotKey` message loop, layered full-screen grid overlay, `SendInput` pointer, optional **arrow-key mouse mode** with HUD. Python app remains reference only; new code under `src/NumpadGridMouse.Win/`.

## Links

- [Original prompts](./original-prompts.md)
- [Plan tree](./plan/index.md)
- [Architecture](./plan/architecture.md)
- [Modes & hotkeys](./plan/modes-and-hotkeys.md)
- [Literal UI copy](./plan/ui-literal-copy.md)
- [Implementation index](./implementation-index.md)
- [Checklist](./implementation-checklist.md)
- [Log](./implementation-log.md)
- [Operators](./operators/index.md)
