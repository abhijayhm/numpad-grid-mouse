# Implementation index — Native Windows Grid & Arrow Mouse

## Definition of done

- `src/NumpadGridMouse.Win` builds and publishes a **single-file win-x64** exe.
- **Ctrl+Shift+G** grid and **Ctrl+Shift+A** arrow mode work without admin.
- Grid behavior matches Python parity doc (chunk 12).
- Tray + optional **Run at logon** (HKCU).
- All chunk acceptance HTML items satisfied or noted in log.

## Chunks

| ID | Name | Doc | Depends |
|----|------|-----|---------|
| 01 | Solution scaffold | `plan/chunks/01-solution-scaffold.md` | — |
| 02 | Global hotkeys | `plan/chunks/02-global-hotkeys.md` | 01 |
| 03 | Config AppData | `plan/chunks/03-config-appdata.md` | 01 |
| 04 | Grid model | `plan/chunks/04-grid-model.md` | 01 |
| 05 | Pointer SendInput | `plan/chunks/05-pointer-sendinput.md` | 01 |
| 06 | Grid overlay | `plan/chunks/06-grid-overlay.md` | 04, 05 |
| 07 | Grid input FSM | `plan/chunks/07-grid-input-fsm.md` | 02, 06 |
| 08 | Arrow pointer mode | `plan/chunks/08-arrow-pointer-mode.md` | 02, 05 |
| 09 | Listening HUD | `plan/chunks/09-listening-hud.md` | 08 |
| 10 | Tray logon settings | `plan/chunks/10-tray-logon-settings.md` | 02, 03, 07, 08, 09 |
| 11 | Build publish | `plan/chunks/11-build-publish.md` | 10 |
| 12 | Parity acceptance | `plan/chunks/12-parity-acceptance.md` | 11 |

## Dependency graph

```text
01 ─┬─► 02 ─┬─► 07 ─┐
    ├─► 03 ─────────┼─► 10 ─► 11 ─► 12
    ├─► 04 ─► 06 ───┘
    └─► 05 ─┬─► 06
            └─► 08 ─► 09 ─► 10
```

## Rules / skills for executors

- **Entry:** `/vs-executionist-02-native-grid-win`
- **Autonomous:** `plan/autonomous-execution.md`
- **Guardrails:** in executionist skill `PROGRAM_GUARDRAILS`
- **Voice:** `vibe-scaler/VOICE.md`
