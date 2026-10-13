---
name: vs-goat
description: >-
  Final program close after senior (goat) approval. User sends shared secret;
  agent MD5-hashes and compares to digest in goat-verification-complete.md only.
  Never store plaintext. Use /vs-goat when marking closed.
---

# Goat verifier 00 — base

## When

- Program status **`goat_verification_pending`** or user says **goat verify close**
- All chunks done, checker ingest complete, **code review** satisfied

## Steps

1. Resolve **PROGRAM_ROOT** (`SYSTEM-PATHS.md`).
2. Confirm `index.md` not already **`closed`** / **`archived`**.
3. Ask senior: **“Enter goat verification secret.”** (do not echo expected value.)
4. Compute **MD5** (UTF-8) of user message trimmed; compare to digest in **`PROGRAM_ROOT/goat-verification-complete.md`** (`29a82940f3fc3dda9d3d24df85d0cf1c` for kit template — regenerate file per program from template, same digest unless host overrides).
5. **On match:** set `index.md` status **`closed`**, playground `index.md` row, log entry, write `goat-verification-complete.md` `verified_at`.
6. **On fail:** stay **`goat_verification_pending`**; no status change.

Never write the plaintext secret to any file.

## Voice

**`vibe-scaler/VOICE.md`** — explain final close in simple terms ("mark the program officially done"); no crypto lecture unless user wrote **`pro`**.
