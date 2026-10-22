# GitHub Rules for Using AI Agents

Status: draft for team approval. Last updated 2026-10-22.

Everyone on the ClimaPulse team may use AI tools (Claude Code, GitHub Copilot, Cursor, ChatGPT, Gemini, or anything else). This file says how to use them on our repository without breaking `main`, leaking a secret, or handing in code nobody understands. It extends the Git branch and Pull Request cheat sheet we already use.

**Course policy comes first.** If the lecturer or the course sets limits on AI use for a graded deliverable, those limits override this file. Ask once and write the answer here: `AI policy of the course: ____________`.

---

## 1. The five rules that matter most

1. **You own what the agent writes.** The person who prompts the agent is the author of the change and must be able to explain every line in the review.
2. **Agents work on feature branches only.** Never on `main`, never on another member's branch.
3. **A human merges, after another human reviews.** An agent never merges, approves, or force-pushes.
4. **No secrets in prompts or in git.** Not in a chat window, not in a commit, not in a screenshot.
5. **Say when AI helped.** Tick the AI disclosure box in the PR template.

The rest of this file explains each rule and the small procedures behind it.

---

## 2. Branches and Git

| Do | Do not |
|---|---|
| Create a branch per task: `feat/watchlist-service`, `fix/zscore-division`, `docs/srd` | Let an agent commit to `main` |
| Run `git pull origin main` before the agent starts, so it works on current code | Let two agents or two people edit the same files on different branches without telling each other |
| Keep a PR to one task and under about 400 changed lines | Ask an agent to "refactor everything" in one go |
| Use `git status` and `git diff` yourself before every commit | Trust an agent's summary of what it changed |
| Keep branches after merge (a course requirement) | Run `git push --force`, `git reset --hard` on shared branches, or rewrite history |

Branch names use `type/short-description` with types `feat`, `fix`, `docs`, `refactor`, `test`, `chore`.

Commit messages follow Conventional Commits, for example `feat(watchlist): implement IWatchlistService.AddAsync`. If an agent wrote most of a commit, add a trailer line such as `Co-Authored-By: <tool name>`.

**Pros of this setup:** a bad agent run only damages one branch, history stays readable, and a rollback is one revert. **Cons:** more branches to manage, and small merge conflicts when the monorepo's shared files (`README.md`, `MainWindow.xaml.cs`, the Contracts project) are edited by several people. Keep those edits in tiny PRs and merge them first.

---

## 3. Pull requests and review

1. Open a PR from your branch to `main` using the PR template. Fill in every section, including the AI disclosure.
2. At least **one teammate who is not the author** reviews. Review means reading the diff, building, and running the feature, not just clicking Approve.
3. The reviewer checks the points in section 7.
4. The author merges after approval. Merge PRs one at a time, then everyone runs `git pull` on `main`.
5. A PR that an agent wrote must be tested by a human before review. Paste the commands you ran and what you saw.

**Optional, needs the repo admin (the group leader):** turn on branch protection for `main` under GitHub Settings, Branches. Require a pull request, require 1 approval, and block force pushes. Also enable secret scanning and push protection under Settings, Code security. **Pros:** GitHub enforces the rules, so nobody can break them by accident. **Cons:** you cannot push a quick fix straight to `main`, and the admin must set it up. For a graded group project the pros outweigh the cons.

---

## 4. What agents may and may not do

### Allowed without asking
- Write or edit code, tests and docs inside the task you gave it.
- Run `dotnet build`, `dotnet test`, and read-only git commands (`status`, `diff`, `log`).
- Explain code, suggest refactors, draft commit messages and PR text.

### Ask the team first (put it in the PR description)
- Any change to `backend/Database/schema.sql`. A schema change must update `schema.sql`, `schema.dbml`, `seed.sql`, the models, and `DbMapping` together, and the ERD and class diagram must be refreshed.
- Adding, removing or upgrading a NuGet or npm package. State the name, version, licence and why it is needed. Agents sometimes invent package names that do not exist or that a stranger has registered, so check the package page yourself.
- Moving or deleting files and folders, or changing the `frontend/` and `backend/` layout.
- Changing `.gitignore`, build files, or any CI configuration.
- Changing an interface, DTO or exception in `shared/ClimaPulse.Contracts` after another member has started using it. Such a PR needs one frontend and one backend reviewer (`docs/api-contract.md`, section 11).

### Never
- Commit `.env`, `climapulse.db`, API keys, tokens, or real personal data.
- Rewrite git history, force-push, or delete branches.
- Edit another member's open branch or PR.
- Disable tests, checks or warnings to make a build pass.
- Make network calls to services outside the approved list (Open-Meteo and package registries).

---

## 5. Secrets and data

- Our database holds sample data only, but the habit matters. Do not paste connection strings, tokens, `.env` content, or screenshots of them into any chat tool.
- `.env` and `*.db` are in `.gitignore`. An agent must not edit `.gitignore` to change that.
- Before every commit run `git diff --staged` and look for keys, passwords and file paths with usernames.
- Everything pasted into a hosted AI tool leaves your machine. Treat it as public.
- **Secret committed or pasted:** tell the team at once. Revoke or rotate the secret first, then remove it from the code. Deleting the commit is not enough because git history and forks keep a copy.

---

## 6. How to prompt an agent so the output is usable

1. Point the agent at `AGENTS.md` (or tell it to read it) and the relevant doc, such as `docs/SRD.md` or the class diagrams.
2. State the task, the files it may touch, and the definition of done, for example "implement `ICityService.ListStoredAsync`, return `CityDto`, add one xUnit test".
3. For anything touching more than three files, ask for a plan first and read it before the agent edits anything.
4. Ask the agent to run the build and tests and to report the real output, not a guess.
5. If the agent says it cannot reach a website or run a command, accept that. Do not let it fake the result.
6. Read the diff yourself. If you cannot explain a change, ask the agent to simplify it or remove it.

**Pros of the plan-first habit:** you catch a wrong approach before it costs an hour. **Cons:** one extra round trip per task.

---

## 7. Reviewer checklist

Copy this into your review comment.

- [ ] The PR does one thing and the title says what.
- [ ] It builds and the tests pass on the reviewer's machine.
- [ ] I ran the feature and saw it work.
- [ ] No secrets, `.env`, `.db` files or personal paths are in the diff.
- [ ] No unrelated files were reformatted or moved.
- [ ] New packages are justified and exist on the official registry.
- [ ] Layering is respected: Razor components inject only `ClimaPulse.Contracts` interfaces, services return DTOs, only `WeatherService` calls Open-Meteo.
- [ ] Database changes updated the schema, DBML, seed, models and mapping together.
- [ ] Error cases are handled (empty list, zero standard deviation, missing city).
- [ ] The author could explain the tricky parts when asked.

---

## 8. Files that tell each tool these rules

Agents only follow rules they can see. Because the team uses different tools, the repo contains one short rules file for each major tool. They all say the same thing and point back to this document.

| File | Read by |
|---|---|
| `AGENTS.md` | Claude Code (through `CLAUDE.md`), Copilot coding agent, Cursor, and many other tools that follow the shared AGENTS.md convention |
| `CLAUDE.md` | Claude Code |
| `docs/copilot-instructions.md` | GitHub Copilot (team copy, see the note below the table) |
| `.cursor/rules/climapulse.mdc` | Cursor |
| `docs/pull_request_template.md` | Everyone, offered by GitHub when opening a PR |

Why these two live in `docs/` and not `.github/`: the team chose to keep all documents in one folder. GitHub's pull request template page says the template can sit in the repository root, `docs/` or `.github/`, so the PR template still works (check the page linked in section 10 if it does not appear on a new PR). Copilot only loads `copilot-instructions.md` automatically from `.github/`, so the file in `docs/` is a team copy that Copilot will not read by itself. **Pro:** one place for all docs and no protected folder. **Con:** Copilot chat in the editor does not see these rules unless the person attaches the file. The Copilot coding agent still reads `AGENTS.md`, and a developer can copy the file to `.github/` on their own machine.

For browser chat tools (ChatGPT, Gemini) no file is read automatically. Paste the contents of `AGENTS.md` at the start of the conversation, and never paste secrets.

**Keeping them in sync:** `AGENTS.md` is the short source for agents and this document is the long version for people. When a rule changes, update this file and `AGENTS.md` in the same PR, then copy the hard rules into the other tool files.

**Pros of several files:** every tool sees the rules without extra setup. **Cons:** four small files can drift apart. The tool files are therefore kept to a few lines plus a pointer to `AGENTS.md`.

---

## 9. If something goes wrong

| Situation | What to do |
|---|---|
| Agent committed to `main` | Tell the team. Do not push. Create a branch from your current state, then reset your local `main` to `origin/main`. |
| Agent pushed bad code to your branch | Revert the commit with `git revert <hash>` and push. Do not rewrite history. |
| Agent deleted or overwrote files | Restore with `git checkout -- <file>` or `git restore <file>` before committing. |
| Secret exposed | See section 5. Rotate first. |
| Merge conflict | Follow the conflict steps in the Git cheat sheet. Do not let an agent resolve conflicts in `schema.sql` without a human check. |
| Build broke on `main` | Open an issue, revert the offending PR, then fix on a new branch. |

---

## 10. Where the sources are

- Git workflow: `claude/git-branch-pr-cheatsheet.md` in the project docs.
- GitHub custom instructions for Copilot: https://docs.github.com/copilot/how-tos/custom-instructions/adding-repository-custom-instructions-for-github-copilot (verified in search on 2026-10-16).
- Claude Code project memory (`CLAUDE.md`): https://code.claude.com/docs/en/memory (not opened from my environment, please check).
- AGENTS.md convention: https://agents.md/ (not opened from my environment, please check).
- Cursor rules: https://docs.cursor.com/context/rules (not opened from my environment, please check).
- Conventional Commits: https://www.conventionalcommits.org/en/v1.0.0/ (not opened from my environment, please check).
- GitHub pull request templates: https://docs.github.com/en/communities/using-templates-to-encourage-useful-issues-and-pull-requests/creating-a-pull-request-template-for-your-repository (page found in search on 2026-10-17, location details not opened, please check).
- GitHub protected branches: https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/about-protected-branches (not opened from my environment, please check).
