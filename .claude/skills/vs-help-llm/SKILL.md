---
name: vs-help-llm
description: >-
  Print a paste-ready prompt (handbook + optional question + where the playground
  is) for ChatGPT or any other LLM. Use /vs-help-llm or menu Ask another AI.
---

# Help — another LLM

Read **`vibe-scaler/prompts/handbook.md`**. If the user added a question in the same message, include it. If `docs/idea-to-reality-playground/quick-index.md` exists, add a short "where we are" from it (program, status, active chunk). Do not invent status.

Reply with:

1. One sentence: paste the block below into ChatGPT or any LLM.
2. A single fenced block whose body is:
   - the handbook text
   - **Where we are:** (or "No playground yet.")
   - **My question:** (their words, or "Teach me the flow.")
3. Remind them they can come back here and use **/vs-help-teach** or **/vs-hub** for the real project.

Do not run product code.
