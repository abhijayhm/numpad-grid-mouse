from __future__ import annotations

import json
from pathlib import Path

from vibe_scaler_sdk.agents import load_system_prompt, run_cursor_agent
from vibe_scaler_sdk.config import find_repo_root, load_settings
from vibe_scaler_sdk.history import ensure_session, new_chat_id, write_state


def resolve_auto_execute(repo_root: Path, dry_run: bool) -> dict:
    """Next chunk from playground when present; else demo stub for harness dev."""
    index = repo_root / "docs" / "idea-to-reality-playground" / "implementation-index.md"
    if index.is_file():
        return {
            "source": "playground",
            "program_root": str(index.parent.parent),
            "chunk_id": "from-implementation-index",
            "note": "Wire to vs-auto-execute skill logic in chunk h06.",
        }
    if dry_run:
        return {
            "source": "demo",
            "program_root": None,
            "chunk_id": "h07-demo",
            "note": "No playground yet — using harness demo chunk.",
        }
    raise RuntimeError(
        "No playground program found. Run /vs-ideator for 00_vs-sdk-harness or pass --dry-run."
    )


def start_pipeline(chat_id: str | None = None, dry_run: bool = False, agent1_only: bool = False) -> str:
    settings = load_settings()
    repo = Path(settings["repo_root"])
    chat_id = chat_id or new_chat_id()
    ensure_session(repo, chat_id)

    chunk = resolve_auto_execute(repo, dry_run=dry_run)
    write_state(
        repo,
        chat_id,
        {
            "pipeline_status": "running",
            "active_chunk": chunk.get("chunk_id"),
            "last_resolution": chunk,
        },
    )

    if dry_run and not agent1_only:
        write_state(repo, chat_id, {"pipeline_status": "dry_run_complete"})
        return json.dumps({"chat_id": chat_id, "resolution": chunk, "agents": "skipped"}, indent=2)

    a1_out = run_cursor_agent(
        api_key=settings["cursor_api_key"],
        model=settings["model"],
        cwd=settings["repo_root"],
        system=load_system_prompt("1"),
        user_message=f"Active chunk context:\n{json.dumps(chunk, indent=2)}\n\nExecute per plan.",
        for_user_only=False,
    )

    write_state(repo, chat_id, {"agent1_last_output": a1_out[:8000]})

    if agent1_only:
        write_state(repo, chat_id, {"pipeline_status": "agent1_complete"})
        return a1_out

    a2_out = run_cursor_agent(
        api_key=settings["cursor_api_key"],
        model=settings["model"],
        cwd=settings["repo_root"],
        system=load_system_prompt("2"),
        user_message=f"Agent 1 output:\n{a1_out}\n\nChoose review_and_next action.",
        for_user_only=False,
    )
    write_state(
        repo,
        chat_id,
        {"pipeline_status": "agent2_complete", "agent2_last_output": a2_out[:8000]},
    )
    return a2_out
