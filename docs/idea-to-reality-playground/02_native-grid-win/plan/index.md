# Plan tree — Native Windows Grid & Arrow Mouse

## Outline

```text
1. Architecture & product contract
   → architecture.md, modes-and-hotkeys.md, ui-literal-copy.md
2. Foundation (batch A)
   01 solution scaffold · 02 global hotkeys · 03 config
3. Core pointer & grid logic (batch B)
   04 grid model · 05 SendInput pointer
4. Grid UX (batch C)
   06 overlay · 07 grid input FSM
5. Arrow mode (batch D)
   08 arrow pointer · 09 listening HUD
6. Shell & ship (batch E–F)
   10 tray & logon · 11 build · 12 parity & acceptance
```

## Sections

| # | Doc | Summary |
|---|-----|---------|
| — | [architecture.md](./architecture.md) | Processes, HWND, hooks, project layout |
| — | [modes-and-hotkeys.md](./modes-and-hotkeys.md) | Grid vs arrow mode; chords |
| — | [ui-literal-copy.md](./ui-literal-copy.md) | Strings for tray, HUD, overlay |
| — | [autonomous-execution.md](./autonomous-execution.md) | Multi-chunk batches; minimal human gates |

## Chunks

| ID | Doc |
|----|-----|
| 01 | [01-solution-scaffold.md](./chunks/01-solution-scaffold.md) |
| 02 | [02-global-hotkeys.md](./chunks/02-global-hotkeys.md) |
| 03 | [03-config-appdata.md](./chunks/03-config-appdata.md) |
| 04 | [04-grid-model.md](./chunks/04-grid-model.md) |
| 05 | [05-pointer-sendinput.md](./chunks/05-pointer-sendinput.md) |
| 06 | [06-grid-overlay.md](./chunks/06-grid-overlay.md) |
| 07 | [07-grid-input-fsm.md](./chunks/07-grid-input-fsm.md) |
| 08 | [08-arrow-pointer-mode.md](./chunks/08-arrow-pointer-mode.md) |
| 09 | [09-listening-hud.md](./chunks/09-listening-hud.md) |
| 10 | [10-tray-logon-settings.md](./chunks/10-tray-logon-settings.md) |
| 11 | [11-build-publish.md](./chunks/11-build-publish.md) |
| 12 | [12-parity-acceptance.md](./chunks/12-parity-acceptance.md) |
