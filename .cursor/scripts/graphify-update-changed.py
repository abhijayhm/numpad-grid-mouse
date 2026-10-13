#!/usr/bin/env python3
"""Incremental Graphify rebuild for explicit changed paths only."""
from __future__ import annotations

import sys
from pathlib import Path


def _repo_root() -> Path:
    root = Path(".")
    saved = Path("graphify-out") / ".graphify_root"
    if saved.exists():
        text = saved.read_text(encoding="utf-8").strip()
        if text:
            return Path(text)
    return root


def main(argv: list[str]) -> int:
    force = False
    paths: list[Path] = []
    for arg in argv:
        if arg == "--force":
            force = True
            continue
        if arg.startswith("-"):
            print(f"error: unknown option: {arg}", file=sys.stderr)
            return 2
        paths.append(Path(arg))

    if not paths:
        print(
            "usage: graphify-update-changed.py [--force] <file> [file ...]",
            file=sys.stderr,
        )
        return 2

    try:
        from graphify.watch import _rebuild_code
    except ImportError:
        print("error: graphify not installed (pip install graphifyy)", file=sys.stderr)
        return 1

    ok = _rebuild_code(_repo_root(), changed_paths=paths, force=force)
    return 0 if ok else 1


if __name__ == "__main__":
    raise SystemExit(main(sys.argv[1:]))
