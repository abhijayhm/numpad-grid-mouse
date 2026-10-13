from __future__ import annotations

import json
import os
import tempfile
import uuid
from datetime import datetime, timezone
from pathlib import Path

from vibe_scaler_sdk.config import find_repo_root


def new_chat_id() -> str:
    """Generate a unique session identifier."""
    return str(uuid.uuid4())


def get_history_dir(repo_root: Path | str | None = None) -> Path:
    """Locate the root history directory under docs/vibe-scaler/history/."""
    root = Path(repo_root).resolve() if repo_root else find_repo_root()
    history_dir = root / "docs" / "vibe-scaler" / "history"
    history_dir.mkdir(parents=True, exist_ok=True)
    return history_dir


def get_session_dir(repo_root: Path | str | None, session_id: str) -> Path:
    """Return path to a specific session directory."""
    return get_history_dir(repo_root) / session_id


def ensure_session(repo_root: Path | str | None, session_id: str) -> Path:
    """Ensure directory docs/vibe-scaler/history/<session_id>/ exists with initial state and summary files."""
    session_dir = get_session_dir(repo_root, session_id)
    session_dir.mkdir(parents=True, exist_ok=True)

    state_file = session_dir / "state.json"
    if not state_file.is_file():
        initial_state = {
            "session_id": session_id,
            "created_at": datetime.now(timezone.utc).isoformat(),
            "pipeline_status": "initialized",
        }
        state_file.write_text(json.dumps(initial_state, indent=2), encoding="utf-8")

    summary_file = session_dir / "summary.md"
    if not summary_file.is_file():
        summary_file.write_text(
            f"# Session summary — {session_id}\n\nCreated: {datetime.now(timezone.utc).isoformat()}\n\n",
            encoding="utf-8",
        )

    return session_dir


def read_state(repo_root: Path | str | None, session_id: str) -> dict:
    """Read session state dictionary from state.json."""
    state_file = get_session_dir(repo_root, session_id) / "state.json"
    if not state_file.is_file():
        return {}
    try:
        return json.loads(state_file.read_text(encoding="utf-8"))
    except json.JSONDecodeError:
        return {}


def write_state(repo_root: Path | str | None, session_id: str, updates: dict) -> dict:
    """Atomically update state.json with new keys and timestamp."""
    session_dir = ensure_session(repo_root, session_id)
    state = read_state(repo_root, session_id)
    state.update(updates)
    state["updated_at"] = datetime.now(timezone.utc).isoformat()

    content = json.dumps(state, indent=2)
    # Atomic write via temp file in same directory
    with tempfile.NamedTemporaryFile("w", dir=session_dir, delete=False, encoding="utf-8") as tf:
        tf.write(content)
        temp_name = tf.name

    target = session_dir / "state.json"
    os.replace(temp_name, target)
    return state


def append_summary(repo_root: Path | str | None, session_id: str, line: str) -> None:
    """Append a timestamped line to summary.md."""
    session_dir = ensure_session(repo_root, session_id)
    summary_file = session_dir / "summary.md"
    ts = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M:%S UTC")
    with open(summary_file, "a", encoding="utf-8") as f:
        f.write(f"- [{ts}] {line}\n")


if __name__ == "__main__":
    sid = "test-" + new_chat_id()[:8]
    s_dir = ensure_session(None, sid)
    assert s_dir.is_dir(), "Session dir must exist"
    write_state(None, sid, {"status": "ok", "test_key": 42})
    read_back = read_state(None, sid)
    assert read_back.get("test_key") == 42, "State write/read roundtrip failed"
    append_summary(None, sid, "Smoke test summary entry")
    summary_text = (s_dir / "summary.md").read_text(encoding="utf-8")
    assert "Smoke test summary entry" in summary_text, "Summary append failed"
    # Cleanup test session
    import shutil
    shutil.rmtree(s_dir)
    print("All history assertions passed.")
