from __future__ import annotations

from pathlib import Path

from vibe_scaler_sdk.for_user import extract_for_user


def _prompt_path(name: str) -> Path:
    return Path(__file__).resolve().parent / "prompts" / name


def load_system_prompt(agent: str) -> str:
    path = _prompt_path(f"agent{agent}.md")
    return path.read_text(encoding="utf-8")


def run_cursor_agent(
    *,
    api_key: str,
    model: str,
    cwd: str,
    system: str,
    user_message: str,
    for_user_only: bool = False,
) -> str:
    from cursor_sdk import Agent, LocalAgentOptions

    full_user = f"{system}\n\n---\n\n{user_message}"
    with Agent.create(
        model=model,
        api_key=api_key,
        local=LocalAgentOptions(cwd=cwd),
    ) as agent:
        run = agent.send(full_user)
        parts: list[str] = []
        for message in run.messages():
            if message.type != "assistant":
                continue
            for block in message.message.content:
                if block.type == "text":
                    parts.append(block.text)
        run.wait()
    text = "".join(parts)
    return extract_for_user(text) if for_user_only else text
