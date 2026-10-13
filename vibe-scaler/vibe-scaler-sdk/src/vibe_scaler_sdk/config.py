from __future__ import annotations

import os
from pathlib import Path
from dotenv import load_dotenv


def find_repo_root(start: Path | None = None) -> Path:
    """Walk up from start path until .env, .git, or personal-utils root is found."""
    current = (start or Path.cwd()).resolve()
    for directory in [current, *current.parents]:
        if (directory / ".env").is_file() or (directory / ".git").is_dir():
            return directory
    return current


def get_api_key(repo_root: Path | None = None) -> str:
    """Load CURSOR_API_KEY from environment or repo root .env.

    Raises:
        RuntimeError: If CURSOR_API_KEY is not set or empty. Never logs or echoes the key.
    """
    key = os.getenv("CURSOR_API_KEY")
    if not key:
        root = repo_root or find_repo_root()
        env_file = root / ".env"
        if env_file.is_file():
            load_dotenv(dotenv_path=env_file)
            key = os.getenv("CURSOR_API_KEY")

    if not key or not key.strip():
        raise RuntimeError(
            "CURSOR_API_KEY is missing or empty. Set it in repo root .env (see .env.example) or shell environment."
        )
    return key.strip()


def mask_api_key(key: str | None = None) -> str:
    """Safely mask key for display without exposing secrets."""
    if not key:
        try:
            key = get_api_key()
        except RuntimeError:
            return "(missing)"
    if key.startswith("crsr_") and len(key) > 8:
        return f"{key[:5]}***{key[-4:]}"
    return "*** (set, hidden)"


def load_settings(repo_root: Path | None = None) -> dict[str, str]:
    """Return dictionary of essential runtime settings."""
    root = repo_root or find_repo_root()
    key = get_api_key(root)
    model = os.getenv("CURSOR_MODEL", "claude-3-5-sonnet-20241022").strip()
    return {
        "repo_root": str(root),
        "cursor_api_key": key,
        "model": model,
    }


if __name__ == "__main__":
    test_root = find_repo_root()
    assert test_root.is_dir(), "find_repo_root must return an existing directory"
    test_key = get_api_key(test_root)
    assert test_key and len(test_key) > 10, "get_api_key must return a valid non-empty key"
    assert mask_api_key(test_key) != test_key, "mask_api_key must never expose the raw key"
    settings = load_settings(test_root)
    assert "cursor_api_key" in settings and settings["cursor_api_key"] == test_key
    print("All config assertions passed.")
