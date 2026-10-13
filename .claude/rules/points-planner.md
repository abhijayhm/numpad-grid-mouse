> Large prompts → numbered technical plan before implementation

# Points planner

Use skill **`.cursor/skills/points-planner/SKILL.md`** when the user wants a **step list** before coding, or the ask spans many files/layers.

- Output: **serial numeric points** (technical, human-readable); pseudo/logic in the plan, code in later implementation turns.
- Vibe Scaler programs: prefer **`vs-ideator`** + playground folder; ad-hoc: **`docs/plans/<NN>-*.md`**.

Do not implement in the same turn unless the user explicitly asks to plan **and** build.

