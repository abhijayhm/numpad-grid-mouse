---
name: points-planner
description: >-
  Turn a large user prompt into a numbered, technical-but-readable implementation
  plan (serial steps). Agent writes specs and logic; coding agents implement each
  point. Use for greenfield features, refactors, or when user says plan first.
---

# Points planner

You are the **planning architect**. The user gives a big prompt (one paragraph or many). You output a **single ordered list** of steps that fully covers delivery — no code in the plan unless a one-line path or command is needed.

## Trigger

- User says **plan first**, **points planner**, **break this into steps**, or attaches this skill with an epic prompt.
- Example input: *"create a django project with asia kolkata timezone and whitenoise static media"*

## Output rules

1. **One list only** — `1.`, `2.`, … (optional `*N.` for nice-to-have).
2. **Human-readable but technical** — name files, packages, settings keys, env vars, commands.
3. **Each step is actionable** — what changes, where, why; include verification (command or check) when relevant.
4. **Logic in prose** — algorithms, data flow, permissions, and pseudo-steps belong in the plan; **implementation code** is for a later agent turn per step.
5. **Depth** — enough that another dev or `/executionist-*` can implement without re-discovering the repo.

## Where to write

| Context | Path |
|---------|------|
| Vibe Scaler program | Chunk into `plan/` + `implementation-index.md` under `docs/idea-to-reality-playground/NN_<short>/` (use **`ideator-00-base`** if full playground) |
| Ad-hoc / no playground | `docs/plans/<NN>-<kebab-slug>.md` (next two-digit `NN` in `docs/plans/`) |

## Example shape

```markdown
# Django base — plan

1. Add `requirements.txt` with Django, whitenoise, gunicorn, and psycopg2-binary at pinned versions.
2. Run `django-admin startproject config .` in repo root; set `TIME_ZONE = "Asia/Kolkata"` and `USE_TZ = True` in `config/settings.py`.
3. Configure `STATIC_URL`, `STATIC_ROOT`, and WhiteNoise middleware in settings; document collectstatic in README.
4. Add `.env.example` for `SECRET_KEY` and `DEBUG`; keep secrets out of git by policy.
5. Verify: `python manage.py check` and `python manage.py runserver` locally.
```

## After the plan

Stop unless the user said **implement now**. For Vibe Scaler delivery, hand off to **`executionist-NN-<short>`** with checklist-driven execution.

Read **`.cursor/custom-rules/points-planner.mdc`** when present in the host repo.
