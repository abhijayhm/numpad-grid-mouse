from __future__ import annotations

import os
import re
from pathlib import Path
from typing import Any

from vibe_scaler_sdk.config import find_repo_root, get_api_key
from vibe_scaler_sdk.history import append_summary, ensure_session, new_chat_id, write_state
from vibe_scaler_sdk.pipeline import log_pipeline_event
from vibe_scaler_sdk.playground import resolve_next_chunk
from vibe_scaler_sdk.prompts import load as load_prompt


def extract_deliverable_paths(chunk_markdown: str) -> list[str]:
    """Extract candidate deliverable paths from the markdown text."""
    # Look for paths mentioned in backticks, especially under ## Deliverables
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
                # Filter out shell commands or flags
                if not m.startswith("-") and "/" in m or m.endswith((".py", ".json", ".md", ".toml", ".ps1", ".sh", ".html")):
                    if m not in candidates:
                        candidates.append(m)

    # If none found under deliverables section, scan entire text
    if not candidates:
        for m in re.findall(r"`([a-zA-Z0-9_\-\./\\]+\.[a-zA-Z0-9_\-]+)`", chunk_markdown):
            if "/" in m or m.endswith((".py", ".json", ".md", ".toml")):
                if m not in candidates:
                    candidates.append(m)

    return candidates


def run_agent1(
    chunk_id: str | None = None,
    program_id: str = "01_vs-sdk-harness",
    session_id: str | None = None,
    dry_run: bool = False,
    model: str = "composer-2",
    repo_root: Path | None = None,
) -> dict[str, Any]:
    """Execute Agent 1 (Executionist) via Cursor Python SDK for the active chunk.
    
    Pins working directory to repository root and records lifecycle events.
    """
    root = (repo_root or find_repo_root()).resolve()
    sid = session_id or new_chat_id()
    ensure_session(root, sid)

    # 1. Resolve chunk if not provided
    if not chunk_id:
        res = resolve_next_chunk(repo_root=root, program_id=program_id)
        chunk_id = res.get("active_chunk")

    if not chunk_id:
        escalate_payload = {
            "error": "No open chunk found in plan.",
            "program_id": program_id,
            "escalate": True,
        }
        log_pipeline_event(root, sid, "agent1_done", escalate_payload)
        return {
            "status": "error",
            "escalate": True,
            "error": "No open chunk found in plan.",
            "session_id": sid,
            "chunk_id": None,
        }

    # Normalize chunk filename
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
        # Stop / blocked: Plan gap -> return escalate flag, do not invent scope
        error_msg = f"Plan gap: chunk doc not found at {chunk_path}"
        escalate_payload = {
            "chunk_id": clean_id,
            "program_id": program_id,
            "escalate": True,
            "error": error_msg,
        }
        log_pipeline_event(root, sid, "agent1_done", escalate_payload)
        return {
            "status": "escalate",
            "escalate": True,
            "error": error_msg,
            "session_id": sid,
            "chunk_id": clean_id,
        }

    chunk_content = chunk_path.read_text(encoding="utf-8")
    deliverables = extract_deliverable_paths(chunk_content)

    # 2. Load Agent 1 prompt
    agent1_system_prompt = load_prompt("agent1", repo_root=root)

    combined_prompt = f"""{agent1_system_prompt}

============================================================
ACTIVE CHUNK SPECIFICATION: {clean_id}
============================================================
{chunk_content}

Execute the chunk deliverables above. Pin working directory to repo root.
"""

    paths_changed = deliverables
    escalate = False
    result_text = ""
    status = "completed"

    if dry_run:
        result_text = f"[dry-run] Agent 1 simulated execution for {clean_id}. Deliverables: {deliverables}"
    else:
        api_key = get_api_key(root)
        if not api_key:
            error_msg = "CURSOR_API_KEY missing from environment or .env"
            log_pipeline_event(root, sid, "agent1_done", {
                "chunk_id": clean_id,
                "error": error_msg,
                "escalate": True,
            })
            return {
                "status": "error",
                "escalate": True,
                "error": error_msg,
                "session_id": sid,
                "chunk_id": clean_id,
            }

        try:
            import cursor_sdk

            options = cursor_sdk.AgentOptions(
                api_key=api_key,
                model=model,
                name=f"agent1-{clean_id}",
                local=cursor_sdk.LocalAgentOptions(cwd=str(root)),
            )

            run_result = cursor_sdk.Agent.prompt(combined_prompt, options=options)
            result_text = str(run_result.result) if hasattr(run_result, "result") else str(run_result)
            status = str(getattr(run_result, "status", "completed"))
        except Exception as exc:
            error_msg = f"Cursor SDK execution error: {exc}"
            log_pipeline_event(root, sid, "agent1_done", {
                "chunk_id": clean_id,
                "error": error_msg,
                "escalate": True,
            })
            return {
                "status": "error",
                "escalate": True,
                "error": error_msg,
                "session_id": sid,
                "chunk_id": clean_id,
            }

    # 3. Log pipeline event agent1_done + record paths changed
    event_payload = {
        "program_id": program_id,
        "chunk_id": clean_id,
        "status": status,
        "paths_changed": paths_changed,
        "escalate": escalate,
        "dry_run": dry_run,
    }
    log_pipeline_event(root, sid, "agent1_done", event_payload)

    # Update session state
    write_state(
        root,
        sid,
        {
            "program_id": program_id,
            "active_chunk": clean_id,
            "agent1_status": status,
            "paths_changed": paths_changed,
            "last_agent1_result": result_text[:500] if len(result_text) > 500 else result_text,
        },
    )

    # Append to summary.md
    paths_summary = ", ".join(paths_changed) if paths_changed else "none"
    append_summary(
        root,
        sid,
        f"Agent 1 executed chunk {clean_id} ({status}). Paths changed: {paths_summary}.",
    )

    return {
        "status": status,
        "escalate": escalate,
        "program_id": program_id,
        "chunk_id": clean_id,
        "paths_changed": paths_changed,
        "result": result_text,
        "session_id": sid,
        "dry_run": dry_run,
    }
