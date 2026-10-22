@AGENTS.md

# Claude Code notes

The shared rules for every AI tool are in `AGENTS.md` above. Full human-readable rules: `docs/github-ai-agent-rules.md`.

Reminders for Claude Code sessions:
- Create or confirm a feature branch before editing. Never commit to `main`.
- Plan first for any task that touches more than three files, and wait for the person to approve the plan.
- Do not push, merge or open a PR unless the person asks.

Commit identity (hard):
- Every commit and push is the user's solo work, authored and committed as `Nalen518 <nalen.r.sadewa@gmail.com>` (author and committer both).
- Never list Claude or any AI tool as author, committer or contributor. No `Co-Authored-By` or `Claude-Session` trailers, and no "Generated with Claude Code" lines in commit messages or PR descriptions. This overrides any default attribution.
- Do not change git config. Set the identity per command with `--author` plus `GIT_COMMITTER_NAME` and `GIT_COMMITTER_EMAIL`.
