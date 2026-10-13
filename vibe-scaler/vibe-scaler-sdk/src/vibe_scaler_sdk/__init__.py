"""Vibe Scaler SDK harness — Plan 1 implementation in progress."""

from vibe_scaler_sdk.config import find_repo_root, get_api_key, load_settings, mask_api_key
from vibe_scaler_sdk.history import append_summary, ensure_session, new_chat_id, read_state, write_state
from vibe_scaler_sdk.prompts import load as load_prompt
from vibe_scaler_sdk.playground import resolve_next_chunk
from vibe_scaler_sdk.pipeline import record_pipeline_event, start_pipeline
from vibe_scaler_sdk.agents.agent1 import run_agent1
from vibe_scaler_sdk.agents.agent2 import review_and_next, run_agent2
from vibe_scaler_sdk.repl import cmd_doctor, run_agent3, run_repl

__version__ = "0.1.0"
__all__ = [
    "__version__",
    "find_repo_root",
    "get_api_key",
    "load_settings",
    "mask_api_key",
    "new_chat_id",
    "ensure_session",
    "read_state",
    "write_state",
    "append_summary",
    "load_prompt",
    "cmd_doctor",
    "run_agent3",
    "run_repl",
    "resolve_next_chunk",
    "start_pipeline",
    "record_pipeline_event",
    "run_agent1",
    "run_agent2",
    "review_and_next",
]
