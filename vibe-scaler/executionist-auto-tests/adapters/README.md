# Adapters

Each adapter runs the profile `default_run` from repo root (or `cwd` in spec) and appends to `results.json`.

| Folder | Profile |
|--------|---------|
| `pytest/` | backend |
| `playwright/` | web, desktop-electron |
| `detox/` | mobile-react-native |
| `flutter/` | mobile-flutter |
| `filesystem/` | filesystem |

Agents may add project-specific wrapper scripts under `adapters/<name>/run.ps1`.
