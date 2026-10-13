from __future__ import annotations

import re
from pathlib import Path
from typing import Any

from vibe_scaler_sdk.config import find_repo_root, get_api_key
from vibe_scaler_sdk.history import append_summary, ensure_session, new_chat_id, write_state
from vibe_scaler_sdk.pipeline import log_pipeline_event
from vibe_scaler_sdk.playground import resolve_next_chunk
from vibe_scaler_sdk.prompts import load as load_prompt


def extract_deliverable_paths(chunk_markdown: str) -> list[str]:
    """Extract candidate deliverable paths from markdown text."""
    lines = chunk_markdown.splitlines()
    in_deliverables = False
    candidates: list[str] = []

    for line in lines:
        if line.strip().startswith("## Deliverables"):
            in_deliverables = True
            continue
        elif in_deliverables and line.strip().startswith("## "):
            break

        if in_deliverables:
            matches = re.findall(r"`([^`\n]+\.[a-zA-Z0-9_\-]+)`", line)
            for m in matches:
                if not m.startswith("-") and ("/" in m or m.endswith((".py", ".json", ".md", ".toml", ".ps1", ".sh", ".html"))):
                    if m not in candidates:
                        candidates.append(m)

    if not candidates:
        for m in re.findall(r"`([a-zA-Z0-9_\-\./\\]+\.[a-zA-Z0-9_\-]+)`", chunk_markdown):
            if "/" in m or m.endswith((".py", ".json", ".md", ".toml")):
                if m not in candidates:
                    candidates.append(m)

    return candidates


def review_and_next(
    chunk_id: str | None = None,
    agent1_result: dict[str, Any] | None = None,
    program_id: str = "01_vs-sdk-harness",
    session_id: str | None = None,
    dry_run: bool = False,
    reinvoke_count: int = 0,
    repo_root: Path | None = None,
) -> dict[str, Any]:
    """Evaluate deliverables and return state decision: 'continue' | 'escalate' | 'reinvoke_1'.

    Escalation messages are formatted specifically for the Agent 3 broker REPL, avoiding jargon.
    """
    root = (repo_root or find_repo_root()).resolve()
    sid = session_id or new_chat_id()
    ensure_session(root, sid)

    if not chunk_id:
        res = resolve_next_chunk(repo_root=root, program_id=program_id)
        chunk_id = res.get("active_chunk")

    if not chunk_id:
        escalate_msg = "The program appears to be complete or has no open chunks in the plan."
        return {
            "decision": "escalate",
            "chunk_id": None,
            "message": "No open chunk found.",
            "escalate_message_for_agent3": escalate_msg,
            "missing_files": [],
            "session_id": sid,
        }

    clean_id = chunk_id[:-3] if chunk_id.endswith(".md") else chunk_id
    chunk_path = (
        root
        / "docs"
        / "idea-to-reality-playground"
        / program_id
        / "plan"
        / "chunks"
        / f"{clean_id}.md"
    )

    if not chunk_path.exists():
        escalate_msg = (
            f"I couldn't find the plan specification for '{clean_id}'. "
            "Please check the plan files or ask the project supervisor."
        )
        return {
            "decision": "escalate",
            "chunk_id": clean_id,
            "message": f"Plan chunk file not found at {chunk_path}",
            "escalate_message_for_agent3": escalate_msg,
            "missing_files": [],
            "session_id": sid,
        }

    chunk_content = chunk_path.read_text(encoding="utf-8")
    deliverables = extract_deliverable_paths(chunk_content)

    # Check if Agent 1 flagged an escalation
    if agent1_result and agent1_result.get("escalate"):
        raw_error = agent1_result.get("error", "Unknown execution error")
        escalate_msg = (
            f"Agent 1 hit an unexpected roadblock on '{clean_id}'. "
            f"Details: {raw_error}. Let's review this together before continuing."
        )
        return {
            "decision": "escalate",
            "chunk_id": clean_id,
            "message": raw_error,
            "escalate_message_for_agent3": escalate_msg,
            "missing_files": [],
            "session_id": sid,
        }

    # Inspect filesystem for required deliverables
    missing_files: list[str] = []
    for d in deliverables:
        # Check relative to repo root, or inside vibe-scaler-sdk
        p1 = root / d
        p2 = root / "vibe-scaler" / "vibe-scaler-sdk" / d
        if not p1.exists() and not p2.exists():
            missing_files.append(d)

    # State decision logic
    if not missing_files:
        decision = "continue"
        msg = f"All {len(deliverables)} required deliverable(s) verified on disk for {clean_id}."
        escalate_msg = None
    elif reinvoke_count < 1 and not dry_run:
        decision = "reinvoke_1"
        msg = f"Missing deliverables ({len(missing_files)}): {missing_files}. Re-invoking Agent 1 remediation pass."
        escalate_msg = None
    else:
        # If missing and re-invocation already tried (or dry_run without existing files)
        decision = "escalate"
        msg = f"Missing deliverables ({len(missing_files)}): {missing_files}."
        escalate_msg = (
            f"Some expected files for '{clean_id}' are still missing after review: {', '.join(missing_files)}. "
            "Would you like me to inspect them or retry the build?"
        )

    return {
        "decision": decision,
        "chunk_id": clean_id,
        "program_id": program_id,
        "message": msg,
        "missing_files": missing_files,
        "deliverables": deliverables,
        "escalate_message_for_agent3": escalate_msg,
        "session_id": sid,
        "dry_run": dry_run,
    }


def run_agent2(
    chunk_id: str | None = None,
    program_id: str = "01_vs-sdk-harness",
    agent1_result: dict[str, Any] | None = None,
    session_id: str | None = None,
    dry_run: bool = False,
    reinvoke_count: int = 0,
    repo_root: Path | None = None,
) -> dict[str, Any]:
    """Execute Agent 2 (Supervisor) review and record lifecycle event agent2_done."""
    root = (repo_root or find_repo_root()).resolve()
    sid = session_id or new_chat_id()
    ensure_session(root, sid)

    # 1. Log lifecycle event spawn_2
    log_pipeline_event(root, sid, "spawn_2", {"chunk_id": chunk_id, "program_id": program_id})

    # 2. Evaluate state transitions
    review = review_and_next(
        chunk_id=chunk_id,
        agent1_result=agent1_result,
        program_id=program_id,
        session_id=sid,
        dry_run=dry_run,
        reinvoke_count=reinvoke_count,
        repo_root=root,
    )

    clean_id = review.get("chunk_id")
    decision = review["decision"]

    # 3. Log lifecycle event agent2_done
    event_payload = {
        "program_id": program_id,
        "chunk_id": clean_id,
        "decision": decision,
        "message": review["message"],
        "missing_files": review["missing_files"],
        "escalate_message_for_agent3": review["escalate_message_for_agent3"],
        "dry_run": dry_run,
    }
    log_pipeline_event(root, sid, "agent2_done", event_payload)

    # Update session state
    write_state(
        root,
        sid,
        {
            "program_id": program_id,
            "active_chunk": clean_id,
            "agent2_decision": decision,
            "supervisor_review": review["message"],
            "agent3_notice": review["escalate_message_for_agent3"],
        },
    )

    # Append to summary.md
    append_summary(
        root,
        sid,
        f"Agent 2 (Supervisor) reviewed chunk {clean_id}: decision = '{decision}'. {review['message']}",
    )

    return review
