from __future__ import annotations

from pathlib import Path

from vibe_scaler_sdk.config import find_repo_root

_PROMPT_ALIASES: dict[str, str] = {
    "1": "agent1_executionist.md",
    "agent1": "agent1_executionist.md",
    "agent1_executionist": "agent1_executionist.md",
    "executionist": "agent1_executionist.md",
    "2": "agent2_supervisor.md",
    "agent2": "agent2_supervisor.md",
    "agent2_supervisor": "agent2_supervisor.md",
    "supervisor": "agent2_supervisor.md",
    "3": "agent3_broker.md",
    "agent3": "agent3_broker.md",
    "agent3_broker": "agent3_broker.md",
    "broker": "agent3_broker.md",
}


def get_prompts_dir(repo_root: Path | None = None) -> Path:
    """Find the prompts directory in vibe-scaler-sdk package tree."""
    root = repo_root or find_repo_root()
    candidates = [
        Path(__file__).resolve().parent.parent.parent / "prompts",
        Path(__file__).resolve().parent / "prompts",
        root / "vibe-scaler" / "vibe-scaler-sdk" / "prompts",
    ]
    for candidate in candidates:
        if candidate.is_dir():
            return candidate
    return candidates[0]


def load(name: str | int, repo_root: Path | None = None) -> str:
    """Load role prompt markdown text by name, number, or alias.

    Examples:
        load('agent3')
        load('1')
        load('agent2_supervisor')
    """
    key = str(name).strip().lower()
    filename = _PROMPT_ALIASES.get(key)
    if not filename:
        filename = f"{key}.md" if not key.endswith(".md") else key

    prompts_dir = get_prompts_dir(repo_root)
    target_file = prompts_dir / filename
    if not target_file.is_file():
        # Fallback to direct name match if alias differed
        for candidate_file in prompts_dir.glob("*.md"):
            if key in candidate_file.stem.lower():
                target_file = candidate_file
                break

    if not target_file.is_file():
        raise FileNotFoundError(
            f"Prompt '{name}' not found in {prompts_dir}. Available: {list_prompts()}"
        )
    return target_file.read_text(encoding="utf-8")


def list_prompts() -> list[str]:
    """List available prompt files in prompts directory."""
    prompts_dir = get_prompts_dir()
    if not prompts_dir.is_dir():
        return []
    return [p.name for p in sorted(prompts_dir.glob("*.md"))]


if __name__ == "__main__":
    for alias in ["1", "agent1", "agent1_executionist", "2", "agent2", "3", "agent3", "agent3_broker"]:
        content = load(alias)
        assert len(content) > 50, f"Prompt {alias} too short"
        assert "VOICE.md" in content, f"Prompt {alias} must reference VOICE.md"
    print("All prompt loader assertions passed.")
