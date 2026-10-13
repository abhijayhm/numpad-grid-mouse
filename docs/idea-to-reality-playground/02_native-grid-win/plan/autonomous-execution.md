# Autonomous execution (orchestrator custom)

**Owner pre-authorized:** agent may implement **multiple chunks per session** without Turn A “proceed” per chunk, unless a stop condition fires.

## Human gates (minimal)

| After batch | Human smoke (≤5 min) |
|-------------|----------------------|
| A | App builds; tray appears; hotkeys log in debug or show message box once |
| B | Unit tests green for GridModel + pointer mocks |
| C | Grid shows on monitor; 1→5→Enter clicks Notepad test area |
| D | Arrow mode: dot visible; arrows move; double-Enter opens file |
| E | Logon checkbox creates HKCU Run value; survives restart |
| F | `dotnet publish` exe runs on clean machine path |

Reply **`proceed`** only at batch boundary if something failed; otherwise agent continues.

## Batches

| Batch | Chunks | Parallel OK |
|-------|--------|-------------|
| **A** | 01, 02, 03 | Yes — single PR/commit group |
| **B** | 04, 05 | Yes |
| **C** | 06, 07 | Prefer sequential 06→07 |
| **D** | 08, 09 | Prefer sequential 08→09 |
| **E** | 10 | Solo |
| **F** | 11, 12 | Yes |

## Agent rules

1. Read **active chunk docs** + this file before coding.
2. **Do not** skip verify commands in chunk docs.
3. Append each completed chunk to `implementation-log.md` + checklist.
4. Set `status.json` → `executing` when batch A starts.
5. Turn C checklist: **post in chat once per batch**, not per chunk.
6. Git: **one commit per batch** unless user asks otherwise; push only when user says.

## Entry command

`/vs-executionist-02-native-grid-win` with message: `Run batch A autonomous`
