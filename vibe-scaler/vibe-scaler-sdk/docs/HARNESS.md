# Vibe Scaler Python SDK Harness Guide

Welcome to the **Vibe Scaler Python SDK Harness** (`vibe-scaler-sdk`), the reference three-agent execution runtime powered by Cursor Python SDK.

This guide is designed for operators, interns, and junior goats to run, verify, and interact with the harness.

---

## 1. Quick Setup & Environment

### Prerequisites
- Python 3.11 or newer
- Virtual environment activated

### Install in Editable Mode
From the repository root:
```bash
cd vibe-scaler/vibe-scaler-sdk
pip install -e .
```

### Environment Configuration (`.env`)
The SDK harness requires your Cursor API key configured in `.env` at the repository root:
```dotenv
# Repository root .env (gitignored)
CURSOR_API_KEY=crsr_your_api_key_here
```
> [!NOTE]
> Never commit `.env` or your raw API key. The harness automatically masks keys in logs and outputs.

---

## 2. Running the Interactive REPL (Agent 3 Broker)

To launch the user-facing interactive session:
```bash
python -m vibe_scaler_sdk
```

### Available REPL Commands

| Command | Action | Description |
|---------|--------|-------------|
| `help` | Show commands | Prints friendly usage instructions |
| `doctor` | Health check | Verifies `.env`, API key, and prompts |
| `run` | Execute pipeline | Resolves active chunk and starts lifecycle run |
| `quit` / `exit` | Terminate session | Exits the interactive loop cleanly |

---

## 3. Workflow Cycle: REPL → `run` → History Folder Updated

The core execution cycle for operators is:

1. **Launch REPL:** Start with `python -m vibe_scaler_sdk`.
2. **Execute:** Type `run` to trigger the pipeline controller.
   - Deterministic resolver identifies the open chunk from `implementation-checklist.md`.
   - Dispatches Agent 1 (Executionist) to execute deliverables pinned to repository root.
   - Dispatches Agent 2 (Supervisor) to review deliverables against acceptance criteria.
3. **Inspect History:** Check the updated session history folder under:
   ```text
   docs/vibe-scaler/history/<session_id>/
   ├── state.json     # Atomic state and lifecycle events
   └── summary.md     # Append-only human-readable event journal
   ```

---

## 4. Autonomous & CLI Execution

You can run individual pipeline phases directly from the command line:

### Check Environment Health
```bash
python -m vibe_scaler_sdk doctor
```

### Resolve Next Open Chunk Deterministically
```bash
python -m vibe_scaler_sdk.resolve --program 01_vs-sdk-harness
```

### Execute Pipeline for a Specific Chunk
```bash
# Dry-run execution
python -m vibe_scaler_sdk.pipeline --chunk c01-package --agent 1 --dry-run

# Run Agent 2 supervisor pass
python -m vibe_scaler_sdk.pipeline --chunk c01-package --agent 2 --dry-run
```

### Run End-to-End Smoke Test
- **macOS / Linux:**
  ```bash
  ./vibe-scaler/vibe-scaler-sdk/scripts/smoke.sh
  ```
- **Windows (PowerShell):**
  ```powershell
  powershell -File vibe-scaler/vibe-scaler-sdk/scripts/smoke.ps1
  ```

---

## 5. Pointer to `/vs-executionist-01-vs-sdk-harness`

For implementing product chunks and progressing through playground programs:

- Use the dedicated slash command **`/vs-executionist-01-vs-sdk-harness`** (or menu via `/vs-hub`).
- Follow the standard loop discipline:
  - **Turn A:** Read briefing and answer alignment prompts.
  - **Turn B:** Implement deliverables and run auto-tests.
  - **Turn C:** Confirm immediate verification checklist in chat before commit and push.
