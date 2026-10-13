> Ensure Graphify CLI, Cursor rule, and knowledge graph before architecture questions

# Graphify Repository Context

Before answering architectural, dependency, code-navigation, refactoring, or cross-file relationship questions, ensure Graphify is fully set up for **this repository**.

Custom rules live in `.cursor/custom-rules/` (loaded via `rule-router.mdc`). Pair with `graphify-auto.mdc` for post-edit sync and `.graphifyignore` maintenance.

A repo init script may automate setup:

1. `pip install graphifyy` if the CLI is missing
2. `graphify cursor install` if `.cursor/custom-rules/graphify.mdc` is missing — then **move** from `.cursor/rules/graphify.mdc` if the CLI wrote there
3. `graphify update .` if `graphify-out/graph.json` is missing (AST-only build, no API key)
4. `.graphifyignore` at repo root if missing (see `graphify-auto.mdc` for template and maintenance)

After init, Cursor agents can use `graphify query`, `graphify path`, and `graphify explain` immediately — no manual `/graphify .` step.

## Background vs blocking

- **Default**: after edits, sync **only touched files** via `.cursor/scripts/graphify-update-changed.py` (background); never hold code or responses waiting on sync.
- **Full repo** (`graphify update .`): only when `graph.json` is missing, `.graphifyignore` changed, or the touch set is unclear/large.
- **Block only when required**: wait for setup or sync only if the **current** task depends on graph-backed navigation and the graph is missing or clearly stale for that query.

## If setup is incomplete (manual recovery)

### Graphify CLI missing

1. Verify: `graphify --version`
2. Install: `pip install graphifyy` (PyPI package **graphifyy**, CLI command **graphify**)

### `.cursor/custom-rules/graphify.mdc` missing

1. From repo root: `graphify cursor install`
2. If output landed in `.cursor/rules/graphify.mdc`, move it to `.cursor/custom-rules/graphify.mdc`

### `graphify-out/graph.json` missing

1. Ensure `.graphifyignore` exists (create from template in `graphify-auto.mdc` if absent)
2. From repo root: `graphify update .` (code/AST only, no LLM cost)
3. For semantic doc extraction later: run full `/graphify .` in Cursor or `graphify extract .` with an API key

### `.graphifyignore` missing

1. Create at repo root before first full graph run (gitignore syntax; see `graphify-auto.mdc`)
2. Kick off `graphify update . --force` in the background so stale nodes are pruned

## When the graph already exists

* Follow `graphify.mdc` and prefer graph-backed navigation before broad file search.
* After code changes: kick off incremental sync on touched paths (`.cursor/scripts/graphify-update-changed.py`) in the background; use full `graphify update .` only when required.
* Verify implementation-critical conclusions against source files before editing.

## Do not

* Manually generate, copy, or recreate `graphify.mdc`.
* Use symlinks for the Graphify Cursor rule.
* Leave `graphify.mdc` in `.cursor/rules/` — only `rule-router.mdc` belongs there.

Graphify context is repository-specific. Never assume a globally installed CLI means this repo has been mapped.

