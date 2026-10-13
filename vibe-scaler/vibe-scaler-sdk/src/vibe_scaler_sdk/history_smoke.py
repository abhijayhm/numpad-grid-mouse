from __future__ import annotations

import shutil
from pathlib import Path

from vibe_scaler_sdk.history import (
    append_summary,
    ensure_session,
    new_chat_id,
    read_state,
    write_state,
)


def run_smoke() -> None:
    session_id = f"smoke-{new_chat_id()[:8]}"
    print(f"Running history smoke test for session: {session_id}")

    session_dir = ensure_session(None, session_id)
    assert session_dir.is_dir(), f"Expected directory {session_dir} to exist"

    state_path = session_dir / "state.json"
    summary_path = session_dir / "summary.md"
    assert state_path.is_file(), "state.json must be created by ensure_session"
    assert summary_path.is_file(), "summary.md must be created by ensure_session"

    initial_state = read_state(None, session_id)
    assert initial_state.get("session_id") == session_id, "Initial state session_id mismatch"

    updated = write_state(
        None,
        session_id,
        {"pipeline_status": "in_progress", "active_chunk": "c03-history", "counter": 1},
    )
    assert updated.get("active_chunk") == "c03-history"

    re_read = read_state(None, session_id)
    assert re_read.get("counter") == 1
    assert "updated_at" in re_read

    append_summary(None, session_id, "Smoke test milestone reached successfully")
    summary_content = summary_path.read_text(encoding="utf-8")
    assert "Smoke test milestone reached successfully" in summary_content

    # Clean up test session directory
    shutil.rmtree(session_dir)
    print("History smoke tests passed successfully.")


if __name__ == "__main__":
    run_smoke()
