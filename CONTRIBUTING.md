# Contributing to CyberTool

Thank you for your interest in contributing to CyberTool. This project welcomes improvements that make Windows security diagnostics safer, clearer, and more useful for authorized environments.

## How to Contribute

1. Fork the repository and create a branch from `main`
2. Make focused changes with clear commit messages
3. Run a Release build locally (see below)
4. Open a pull request using the PR template
5. Respond to review feedback

## Development Setup

### Prerequisites

- Windows 10/11 (build 17763 or later)
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) (see `global.json`)
- Visual Studio 2022 with **Windows application development** workload, or VS Code + Windows App SDK tooling

### Build from Source

```powershell
git clone https://github.com/TahaAkgl27/CyberTool.git
cd CyberTool
dotnet restore CyberTool.csproj
dotnet build CyberTool.csproj -c Release -p:Platform=x64
```

Run the application from Visual Studio using the **CyberTool (Unpackaged)** profile, or execute the built binary under `bin\x64\Release\net8.0-windows10.0.19041.0\`.

### Optional: OpenAI Features

AI-assisted analysis requires a user-provided OpenAI API key configured in **Settings**. Do not commit API keys or add them to source code.

## Branch Naming

Use descriptive branch names:

- `feature/short-description`
- `fix/issue-description`
- `docs/topic`
- `security/hardening-topic`

## Commit Style

Write clear, imperative commit messages:

```
Add WMI timeout handling for remote enumeration

Improve error messaging when target host is unreachable.
```

Avoid bundling unrelated changes in a single commit.

## Pull Request Checklist

Before submitting a PR, confirm:

- [ ] `dotnet build CyberTool.csproj -c Release -p:Platform=x64` succeeds with **0 errors**
- [ ] No secrets, API keys, tokens, or credentials in code or logs
- [ ] No real customer names, production IPs, or internal hostnames in examples
- [ ] Demo/sample data uses generic values (e.g. `192.168.100.x`, `EXAMPLE.LOCAL`)
- [ ] Security-sensitive changes are documented in the PR description
- [ ] New offensive or simulation behavior includes safety notes in docs if applicable
- [ ] UI changes include screenshots in the PR (if applicable)

## Security-Related Contribution Rules

CyberTool includes modules for authorized security testing. Contributors must:

- **Not** add malicious payloads, backdoors, or unauthorized data exfiltration
- **Not** embed real credentials, license keys, or private certificates
- **Not** hardcode production infrastructure details
- Keep brute-force, simulation, and enumeration features clearly scoped to authorized use
- Update [DISCLAIMER.md](DISCLAIMER.md) or [docs/safety.md](docs/safety.md) when behavior affects user safety expectations

## Code Guidelines

- Match existing project conventions (C#, WinUI 3, MVVM patterns)
- Prefer minimal, focused diffs
- Do not refactor unrelated code in the same PR
- Preserve working features unless the PR explicitly fixes or replaces them

## Questions

Open a GitHub Discussion or Issue for questions about setup, architecture, or feature direction. For vulnerabilities, use [GitHub Security Advisories](SECURITY.md)—not public issues.

## License

By contributing, you agree that your contributions will be licensed under the [MIT License](LICENSE).
