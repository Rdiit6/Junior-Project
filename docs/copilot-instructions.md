# Copilot instructions for ClimaPulse

Read `AGENTS.md` in the repository root first. It is the shared rule set for all AI tools. Full rules: `docs/github-ai-agent-rules.md`.

Hard rules in short:
- Work on a feature branch (`type/short-description`). Never commit to `main`. Never force-push.
- Never merge a PR. A teammate reviews and a human merges.
- Never read, print or commit `.env`, secrets or `*.db` files.
- Ask before changing `backend/Database/schema.sql`, adding packages, moving or deleting files, or editing `.gitignore`, build files or CI.
- Keep `ClimaPulse.*` namespaces. Razor components inject only `ClimaPulse.Contracts` interfaces, never backend classes. Only `WeatherService` calls Open-Meteo.
- Read `docs/api-contract.md` before changing a service, DTO or component.
- Run `dotnet build` and `dotnet test` when they exist, and report real results. Do not invent results, links or package names.
- Mention in the PR that AI helped.

Stack: Windows desktop app. WPF with Blazor Hybrid (`BlazorWebView`) in `frontend/`, services and repositories in `backend/`, contract in `shared/ClimaPulse.Contracts`, SQLite, .NET 10.
