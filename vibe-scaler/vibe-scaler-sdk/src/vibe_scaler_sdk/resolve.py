"""CLI wrapper for deterministic playground plan resolution."""

from __future__ import annotations

import argparse
import json
import sys

from vibe_scaler_sdk.playground import resolve_next_chunk


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(
        prog="vibe-scaler-resolve",
        description="Deterministically resolve the next open chunk for a playground program",
    )
    parser.add_argument(
        "--program",
        "-p",
        default="01_vs-sdk-harness",
        help="Program directory name under docs/idea-to-reality-playground/",
    )
    parser.add_argument(
        "--json",
        action="store_true",
        help="Output raw JSON result",
    )

    args = parser.parse_args(argv)

    try:
        result = resolve_next_chunk(program_id=args.program)
    except FileNotFoundError as err:
        print(f"Error: {err}", file=sys.stderr)
        return 1

    if args.json:
        print(json.dumps(result, indent=2))
        return 0

    if result["status"] == "all_complete":
        print(f"Program '{result['program_id']}': All {result['total_count']} chunks are complete!")
    else:
        print(f"Program:      {result['program_id']}")
        print(f"Active chunk: {result['active_chunk']}")
        print(f"Name:         {result['chunk_name']}")
        print(f"Doc:          {result['chunk_doc_rel']}")
        print(f"Depends:      {result['depends']}")
        print(f"Progress:     {result['completed_count']}/{result['total_count']} chunks completed")

    return 0


if __name__ == "__main__":
    sys.exit(main())
