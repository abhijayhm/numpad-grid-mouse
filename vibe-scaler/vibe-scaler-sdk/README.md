# vibe-scaler-sdk

Python harness for Vibe Scaler **Agents 1 / 2 / 3** (Cursor SDK). Plan 1 program: `docs/idea-to-reality-playground/01_vs-sdk-harness/`.

## Setup

```bash
cd vibe-scaler/vibe-scaler-sdk
pip install -e .
```

Repo root **`.env`** must define `CURSOR_API_KEY` (see `.env.example`). Never commit `.env`.

### Configuration (`vibe_scaler_sdk.config`)

The configuration module automatically discovers the repository root by walking up directories and loads `CURSOR_API_KEY` via `python-dotenv`:

```python
from vibe_scaler_sdk.config import get_api_key, load_settings, mask_api_key

api_key = get_api_key()  # Raises RuntimeError if missing
settings = load_settings()  # Returns dict with repo_root, model, cursor_api_key
print(mask_api_key())  # Safe masked string: crsr_***...
```

## Run (after chunks c01–c05)

```bash
python -m vibe_scaler_sdk
```

## Portable

See [PORTABLE.md](./PORTABLE.md) — copy `vibe-scaler/` to a new repo.

## Spec

`vibe-scaler/extension/ideation-three-agent-runtime.md`
