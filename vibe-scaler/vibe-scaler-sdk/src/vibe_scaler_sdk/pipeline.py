from __future__ import annotations

import argparse
import json
import sys
from datetime import datetime, timezone
from pathlib import Path

from vibe_scaler_sdk.config import find_repo_root
from vibe_scaler_sdk.history import append_summary, ensure_session, new_chat_id, read_state, write_state
from vibe_scaler_sdk.playground import resolve_next_chunk

VALID_EVENTS = {
    "auto_execute",
    "spawn_1",
    "spawn_2",
    "agent1_done",
    "agent2_done",
    "pause",
    "resume",
}


def log_pipeline_event(
    repo_root: Path | None,
    session_id: str,
    event_name: str,
    payload: dict | None = None,
) -> dict:
    """Record a discrete pipeline lifecycle event into state.json and summary.md."""
    if event_name not in VALID_EVENTS:
        raise ValueError(f"Invalid event: {event_name}. Allowed: {VALID_EVENTS}")

    ensure_session(repo_root, session_id)
    current_state = read_state(repo_root, session_id)
    events = current_state.get("events", [])

    event_record = {
        "event": event_name,
        "timestamp": datetime.now(timezone.utc).isoformat(),
        "payload": payload or {},
    }
    events.append(event_record)

    state_update = {
        "last_event": event_name,
        "events": events,
    }
    updated = write_state(repo_root, session_id, state_update)

    summary_note = f"Pipeline event '{event_name}'"
    if payload:
        summary_note += f": {json.dumps(payload)}"
    append_summary(repo_root, session_id, summary_note)

    return updated


record_pipeline_event = log_pipeline_event


def start_pipeline(
    session_id: str | None = None,
    dry_run: bool = False,
    program_id: str = "01_vs-sdk-harness",
    chunk_id: str | None = None,
    agent: int | None = None,
    repo_root: Path | None = None,
) -> dict:
    """Resolve chunk and orchestrate agent execution across pipeline lifecycle."""
    root = (repo_root or find_repo_root()).resolve()
    sid = session_id or new_chat_id()
    ensure_session(root, sid)

    # 1. Event: auto_execute
    log_pipeline_event(root, sid, "auto_execute", {"program_id": program_id, "chunk_id": chunk_id})

    # 2. Resolve active chunk
    if chunk_id:
        target_chunk = chunk_id[:-3] if chunk_id.endswith(".md") else chunk_id
        resolution = {"active_chunk": target_chunk, "program_id": program_id}
    else:
        resolution = resolve_next_chunk(repo_root=root, program_id=program_id)
        target_chunk = resolution.get("active_chunk")

    write_state(
        root,
        sid,
        {
            "program_id": program_id,
            "active_chunk": target_chunk,
            "last_resolution": resolution,
            "pipeline_status": "dry_run_complete" if dry_run and not agent else "running",
        },
    )

    # If specifically running Agent 1
    if agent == 1:
        log_pipeline_event(root, sid, "spawn_1", {"target_chunk": target_chunk})
        from vibe_scaler_sdk.agents.agent1 import run_agent1

        a1_result = run_agent1(
            chunk_id=target_chunk,
            program_id=program_id,
            session_id=sid,
            dry_run=dry_run,
            repo_root=root,
        )
        return {
            "session_id": sid,
            "program_id": program_id,
            "active_chunk": target_chunk,
            "agent": 1,
            "agent_result": a1_result,
            "pipeline_status": a1_result.get("status", "completed"),
            "dry_run": dry_run,
        }

    # If specifically running Agent 2
    if agent == 2:
        from vibe_scaler_sdk.agents.agent2 import run_agent2

        a2_result = run_agent2(
            chunk_id=target_chunk,
            program_id=program_id,
            session_id=sid,
            dry_run=dry_run,
            repo_root=root,
        )
        return {
            "session_id": sid,
            "program_id": program_id,
            "active_chunk": target_chunk,
            "agent": 2,
            "agent_result": a2_result,
            "decision": a2_result.get("decision"),
            "pipeline_status": f"supervisor_{a2_result.get('decision')}",
            "dry_run": dry_run,
        }

    if dry_run:
        # Pause event for dry run completion
        log_pipeline_event(root, sid, "pause", {"reason": "dry_run_complete"})
        return {
            "session_id": sid,
            "program_id": program_id,
            "active_chunk": target_chunk,
            "resolution": resolution,
            "pipeline_status": "dry_run_complete",
            "dry_run": True,
        }

    # Default live dispatch: spawn Agent 1
    log_pipeline_event(root, sid, "spawn_1", {"target_chunk": target_chunk})
    from vibe_scaler_sdk.agents.agent1 import run_agent1

    a1_result = run_agent1(
        chunk_id=target_chunk,
        program_id=program_id,
        session_id=sid,
        dry_run=False,
        repo_root=root,
    )
    return {
        "session_id": sid,
        "program_id": program_id,
        "active_chunk": target_chunk,
        "agent": 1,
        "agent_result": a1_result,
        "pipeline_status": a1_result.get("status", "completed"),
        "dry_run": False,
    }


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(
        prog="vibe-scaler-pipeline",
        description="Pipeline controller for resolving and orchestrating chunk execution",
    )
    parser.add_argument(
        "--dry-run",
        action="store_true",
        help="Simulate execution and log lifecycle events without calling external services",
    )
    parser.add_argument(
        "--program",
        "-p",
        default="01_vs-sdk-harness",
        help="Program ID under docs/idea-to-reality-playground/",
    )
    parser.add_argument(
        "--chunk",
        "-c",
        default=None,
        help="Target chunk ID (e.g. c01-package)",
    )
    parser.add_argument(
        "--agent",
        "-a",
        type=int,
        choices=[1, 2, 3],
        default=None,
        help="Agent number to run (1: Executionist, 2: Supervisor, 3: Broker)",
    )
    parser.add_argument(
        "--session-id",
        default=None,
        help="Optional session ID to resume or attach to",
    )
    parser.add_argument(
        "--json",
        action="store_true",
        help="Output raw JSON results",
    )

    args = parser.parse_args(argv)

    result = start_pipeline(
        session_id=args.session_id,
        dry_run=args.dry_run,
        program_id=args.program,
        chunk_id=args.chunk,
        agent=args.agent,
    )

    if args.json:
        print(json.dumps(result, indent=2))
        return 0

    print("=" * 60)
    print("  Vibe Scaler Pipeline Controller")
    print(f"  Session ID:      {result.get('session_id')}")
    print(f"  Program:         {result.get('program_id')}")
    print(f"  Active Chunk:    {result.get('active_chunk')}")
    print(f"  Pipeline Status: {result.get('pipeline_status')}")
    if "agent" in result:
        print(f"  Agent Executed:  Agent {result.get('agent')}")
    print(f"  Dry Run:         {result.get('dry_run')}")
    print("=" * 60)
    print("✓ Pipeline run successfully logged to session history.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
