"""CLI entrypoint for vibe-scaler-sdk: Agent 3 (Broker) REPL and diagnostic tools."""

from __future__ import annotations

import argparse
import sys

from vibe_scaler_sdk.repl import cmd_doctor, run_agent3, run_repl


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(
        prog="vibe-scaler-sdk",
        description="Vibe Scaler three-agent Python harness (Agent 3 REPL)",
    )
    sub = parser.add_subparsers(dest="cmd")

    # doctor command
    sub.add_parser("doctor", help="Check .env, API key, and repo root configuration")

    # chat command
    p_chat = sub.add_parser("chat", help="Send a single message to Agent 3 (Broker)")
    p_chat.add_argument("message", help="Message to send to Agent 3")
    p_chat.add_argument("--chat-id", default=None, help="Optional session ID")
    p_chat.add_argument("--dry-run", action="store_true", help="Simulate agent response without network call")

    # run command (stub until c07)
    sub.add_parser("run", help="Run execution pipeline (stub until chunk c07)")

    args = parser.parse_args(argv)

    if args.cmd == "doctor":
        return cmd_doctor()
    elif args.cmd == "chat":
        output = run_agent3(args.message, session_id=args.chat_id, dry_run=args.dry_run)
        print(output)
        return 0
    elif args.cmd == "run":
        from vibe_scaler_sdk.pipeline import start_pipeline

        res = start_pipeline(dry_run=True)
        print("[vibe-scaler] Pipeline started (dry-run mode):")
        print(f"  • Program:       {res['program_id']}")
        print(f"  • Active chunk:  {res['active_chunk']}")
        print(f"  • Status:        {res['pipeline_status']}")
        print(f"  • Session:       {res['session_id']}")
        return 0
    else:
        # Default with no subcommand: launch interactive REPL loop
        run_repl()
        return 0


if __name__ == "__main__":
    sys.exit(main())
