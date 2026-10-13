from __future__ import annotations

import re
from pathlib import Path

from vibe_scaler_sdk.config import find_repo_root

_CHECKED_GLYPHS = {"[x]", "[X]", "☑", "✓", "✔", "x"}


def parse_checklist(checklist_path: Path) -> list[dict[str, any]]:
    """Parse markdown checklist table in implementation-checklist.md into structured records."""
    if not checklist_path.is_file():
        return []

    lines = checklist_path.read_text(encoding="utf-8").splitlines()
    records: list[dict[str, any]] = []

    for line in lines:
        line_clean = line.strip()
        if not line_clean.startswith("|") or "Chunk" in line_clean or "---" in line_clean:
            continue
        cells = [c.strip() for c in line_clean.strip("|").split("|")]
        if len(cells) < 3:
            continue

        chunk_id = cells[0]
        agent_mark = cells[1]
        human_mark = cells[2]

        agent_done = any(glyph in agent_mark for glyph in _CHECKED_GLYPHS)
        human_done = any(glyph in human_mark for glyph in _CHECKED_GLYPHS)

        records.append({
            "chunk_id": chunk_id,
            "agent_done": agent_done,
            "human_done": human_done,
            "is_complete": agent_done and human_done,
        })

    return records


def parse_index(index_path: Path) -> dict[str, dict[str, str]]:
    """Parse chunks metadata table from implementation-index.md."""
    if not index_path.is_file():
        return {}

    lines = index_path.read_text(encoding="utf-8").splitlines()
    chunks: dict[str, dict[str, str]] = {}

    for line in lines:
        line_clean = line.strip()
        if not line_clean.startswith("|") or "ID" in line_clean or "---" in line_clean:
            continue
        cells = [c.strip() for c in line_clean.strip("|").split("|")]
        if len(cells) >= 4:
            cid = cells[0]
            name = cells[1]
            doc = cells[2].strip("`")
            depends = cells[3]
            chunks[cid] = {
                "name": name,
                "doc": doc,
                "depends": depends,
            }

    return chunks


def resolve_next_chunk(
    repo_root: Path | None = None,
    program_id: str = "01_vs-sdk-harness",
) -> dict[str, any]:
    """Deterministically determine the next open chunk for a playground program without LLM.

    Matches the exact semantics of /vs-auto-execute for the specified program.
    """
    root = (repo_root or find_repo_root()).resolve()
    program_root = root / "docs" / "idea-to-reality-playground" / program_id
    if not program_root.is_dir():
        raise FileNotFoundError(f"Playground program directory not found: {program_root}")

    checklist_path = program_root / "implementation-checklist.md"
    index_path = program_root / "implementation-index.md"

    checklist_records = parse_checklist(checklist_path)
    index_records = parse_index(index_path)

    active_record: dict[str, any] | None = None
    for record in checklist_records:
        if not record["is_complete"]:
            active_record = record
            break

    if active_record is None:
        return {
            "program_id": program_id,
            "program_root": str(program_root),
            "status": "all_complete",
            "active_chunk": None,
            "chunk_name": None,
            "chunk_doc": None,
            "completed_count": len(checklist_records),
            "total_count": len(checklist_records),
        }

    chunk_id = active_record["chunk_id"]
    # Look up metadata from index (try exact match or prefix)
    chunk_meta = index_records.get(chunk_id)
    if not chunk_meta:
        # Check prefix match (e.g. c06 matching c06-resolver)
        prefix = chunk_id.split("-")[0]
        chunk_meta = index_records.get(prefix, {})

    doc_rel = chunk_meta.get("doc", f"plan/chunks/{chunk_id}.md")
    doc_path = program_root / doc_rel

    completed_count = sum(1 for r in checklist_records if r["is_complete"])

    return {
        "program_id": program_id,
        "program_root": str(program_root),
        "status": "ready",
        "active_chunk": chunk_id,
        "chunk_name": chunk_meta.get("name", chunk_id),
        "chunk_doc": str(doc_path),
        "chunk_doc_rel": doc_rel,
        "depends": chunk_meta.get("depends", "—"),
        "agent_done": active_record["agent_done"],
        "human_done": active_record["human_done"],
        "completed_count": completed_count,
        "total_count": len(checklist_records),
    }


if __name__ == "__main__":
    res = resolve_next_chunk()
    assert res["program_id"] == "01_vs-sdk-harness"
    assert "status" in res
    assert "active_chunk" in res
    print("Playground resolver assertions passed:", res["active_chunk"])
