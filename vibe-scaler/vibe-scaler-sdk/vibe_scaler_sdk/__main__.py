from __future__ import annotations

import argparse
import sys

from vibe_scaler_sdk.agents import load_system_prompt, run_cursor_agent
from vibe_scaler_sdk.config import load_settings
from vibe_scaler_sdk.harness import start_pipeline
from vibe_scaler_sdk.history import ensure_session, new_chat_id


def cmd_doctor() -> int:
    try:
        s = load_settings()
        print(f"repo_root: {s['repo_root']}")
        print(f"model: {s['model']}")
        print("CURSOR_API_KEY: set (value hidden)")
        return 0
    except RuntimeError as e:
        print(e, file=sys.stderr)
        return 1


def cmd_chat(message: str, chat_id: str | None) -> int:
    settings = load_settings()
    chat_id = chat_id or new_chat_id()
    ensure_session(__import__("pathlib").Path(settings["repo_root"]), chat_id)
    text = run_cursor_agent(
        api_key=settings["cursor_api_key"],
        model=settings["model"],
        cwd=settings["repo_root"],
        system=load_system_prompt("3"),
        user_message=message,
        for_user_only=True,
    )
    print(text)
    print(f"\n[session: docs/vibe-scaler/history/{chat_id}/]", file=sys.stderr)
    return 0


def cmd_pipeline(dry_run: bool, agent1_only: bool, chat_id: str | None) -> int:
    out = start_pipeline(chat_id=chat_id, dry_run=dry_run, agent1_only=agent1_only)
    print(out)
    return 0


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(prog="vibe_scaler_sdk", description="Vibe Scaler 3-agent harness")
    sub = parser.add_subparsers(dest="cmd", required=True)

    sub.add_parser("doctor", help="Check .env and repo root")

    p_chat = sub.add_parser("chat", help="Agent 3 broker (FOR_USER stream)")
    p_chat.add_argument("message", help="User message")
    p_chat.add_argument("--chat-id", default=None)

    p_pipe = sub.add_parser("pipeline", help="Deterministic start_pipeline → Agent 1 → Agent 2")
    p_pipe.add_argument("--dry-run", action="store_true", help="Resolve chunk only, no SDK calls")
    p_pipe.add_argument("--agent1-only", action="store_true")
    p_pipe.add_argument("--chat-id", default=None)

    args = parser.parse_args(argv)
    if args.cmd == "doctor":
        return cmd_doctor()
    if args.cmd == "chat":
        return cmd_chat(args.message, args.chat_id)
    if args.cmd == "pipeline":
        return cmd_pipeline(args.dry_run, args.agent1_only, args.chat_id)
    return 1


if __name__ == "__main__":
    raise SystemExit(main())
