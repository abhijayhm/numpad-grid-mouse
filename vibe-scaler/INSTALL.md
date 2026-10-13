# Install Vibe Scaler in a host repo

1. Copy folder `vibe-scaler/` to repo root (or git submodule).
2. Copy skills:
   - `vibe-scaler/skills/vs-ideator` → `.cursor/skills/vs-ideator`
   - `vibe-scaler/skills/vs-checker` → `.cursor/skills/vs-checker`
   - `vibe-scaler/skills/vs-executionist` → `.cursor/skills/vs-executionist`
   - `vibe-scaler/skills/vs-planner` → `.cursor/skills/vs-planner`
   - `vibe-scaler/skills/vs-harness` → `.cursor/skills/vs-harness`
   - `vibe-scaler/skills/vs-auto-tests` → `.cursor/skills/vs-auto-tests`
   - Rule: `vs-auto-tests-governance.mdc`

## Verify setup (utils repo or any host)

```powershell
powershell -NoProfile -File vibe-scaler/scripts/verify-vibe-scaler-setup.ps1
```

Or **`/vs-harness`** in Cursor / Antigravity after sync.
3. Copy rules → `.cursor/custom-rules/`:
   - `vibe-scaler-rule.mdc`
   - `ideator-governance.mdc`
   - `ideator-checker-governance.mdc`
   - `executionist-governance.mdc`
   - `executionist-monitor.mdc`
4. Add row to host `rule-router-ingest.mdc` (see `vibe-scaler/rules/rule-router-ingest-snippet.mdc`).
5. Create `docs/idea-to-reality-playground/index.md` from `vibe-scaler/templates/playground-index.md`.
6. Run host sync script if Antigravity (e.g. `sync-agent-config.py`).

Per-program skills are **generated** by `vs-ideator` into `.cursor/skills/vs-ideator-NN-<short>/`, `vs-executionist-NN-<short>/`, and `vs-checker-NN-<short>/` (`NN` matches playground folder `NN_<short>/`; **00** = base skills only).
