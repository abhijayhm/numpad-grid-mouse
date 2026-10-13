from __future__ import annotations

import sys
from pathlib import Path

from vibe_scaler_sdk.config import load_settings, mask_api_key
from vibe_scaler_sdk.history import append_summary, ensure_session, new_chat_id
from vibe_scaler_sdk.prompts import load as load_prompt


def run_agent3(
    message: str,
    session_id: str | None = None,
    dry_run: bool = False,
) -> str:
    """Send user message to Agent 3 (Broker) via Cursor SDK and return user-facing text."""
    try:
        settings = load_settings()
    except RuntimeError as err:
        return f"[Vibe Scaler Config Error] {err}"

    sid = session_id or new_chat_id()
    repo_root = Path(settings["repo_root"])
    ensure_session(repo_root, sid)

    if dry_run:
        reply = f"[Dry Run] Agent 3 Broker received: '{message}'. Session ID: {sid}"
        append_summary(repo_root, sid, f"User: {message} | Broker (Dry Run): {reply}")
        return reply

    system_prompt = load_prompt("agent3")
    full_prompt = f"{system_prompt}\n\n---\n\nUser request: {message}"

    try:
        from cursor_sdk import Agent, LocalAgentOptions

        with Agent.create(
            model=settings["model"],
            api_key=settings["cursor_api_key"],
            local=LocalAgentOptions(cwd=str(repo_root)),
        ) as agent:
            run = agent.send(full_prompt)
            parts: list[str] = []
            for msg in run.messages():
                if getattr(msg, "type", None) == "assistant":
                    content = getattr(msg, "message", None)
                    if content and hasattr(content, "content"):
                        for block in content.content:
                            if getattr(block, "type", None) == "text" and hasattr(block, "text"):
                                parts.append(block.text)
            run.wait()
            reply = "".join(parts).strip()
            if not reply:
                reply = "[Agent 3] Task processed with no additional text output."
    except Exception as exc:
        reply = f"[Agent 3 Error] Unable to complete request via Cursor SDK: {exc}"

    append_summary(repo_root, sid, f"User: {message} | Broker: {reply[:120]}...")
    return reply


def cmd_doctor() -> int:
    """Validate environment, API key, and repo root."""
    try:
        settings = load_settings()
        print("Vibe Scaler Environment Health Check:")
        print(f"  • Repo root:      {settings['repo_root']}")
        print(f"  • Model:          {settings['model']}")
        print(f"  • CURSOR_API_KEY: {mask_api_key(settings['cursor_api_key'])}")
        print("✓ All configuration settings verified.")
        return 0
    except RuntimeError as exc:
        print(f"✗ Doctor check failed: {exc}", file=sys.stderr)
        return 1


def run_repl(session_id: str | None = None) -> None:
    """Run interactive Agent 3 REPL loop."""
    sid = session_id or new_chat_id()
    try:
        settings = load_settings()
        repo_root = Path(settings["repo_root"])
        ensure_session(repo_root, sid)
    except RuntimeError as err:
        print(f"Error initializing session: {err}", file=sys.stderr)
        return

    print("=" * 60)
    print("  Vibe Scaler — Agent 3 (Broker) Interactive REPL")
    print(f"  Session ID: {sid}")
    print("  Commands: 'help', 'run', 'doctor', 'quit' or 'exit'")
    print("=" * 60)

    while True:
        try:
            user_input = input("vibe-scaler> ").strip()
        except (KeyboardInterrupt, EOFError):
            print("\nExiting Vibe Scaler REPL.")
            break

        if not user_input:
            continue

        cmd = user_input.lower()
        if cmd in ("quit", "exit", "q"):
            print("Session ended.")
            break
        elif cmd == "help":
            print("\nAvailable REPL commands:")
            print("  help    - Show this help message")
            print("  run     - Trigger execution pipeline (stubbed until c07)")
            print("  doctor  - Verify .env and Cursor SDK configuration")
            print("  quit    - Exit the REPL session\n")
            print("Or enter any prompt to talk to Agent 3 (Broker).\n")
        elif cmd == "doctor":
            cmd_doctor()
        elif cmd == "run":
            from vibe_scaler_sdk.pipeline import start_pipeline
            res = start_pipeline(session_id=sid, dry_run=True)
            print("[vibe-scaler] Pipeline triggered (dry-run mode).")
            print(f"  • Program:       {res['program_id']}")
            print(f"  • Active chunk:  {res['active_chunk']}")
            print(f"  • Status:        {res['pipeline_status']}")
            print("  • Events logged: 'auto_execute' -> 'pause'")
            print("  • Live Agent 1 execution will run in Chunk c08.")
        else:
            response = run_agent3(user_input, session_id=sid)
            print(f"\n{response}\n")
