# Security Policy

CyberTool is security software that includes assessment and simulation capabilities. This policy defines how we handle vulnerabilities, dependencies, privacy, and responsible AI use.

---

## Supported Versions

| Version | Supported | Notes |
|---------|-----------|-------|
| 1.0.x   | Yes       | Current public release line |
| < 1.0   | No        | Pre-release development |

Security fixes are prioritized on the latest release branch. Upgrade guidance will be published in [CHANGELOG.md](../CHANGELOG.md) and GitHub Security Advisories.

---

## Reporting a Vulnerability

**Please report security issues through [GitHub Security Advisories](https://github.com/TahaAkgl27/CyberTool/security/advisories/new).**

### Do NOT use public issues for:

- Undisclosed vulnerabilities
- Leaked secrets or credentials
- Exploitable flaws in authentication, storage, or offensive modules

Public disclosure before coordination may put users at risk.

### Include in your report:

- Description and impact
- Affected version(s)
- Steps to reproduce (sanitized)
- Suggested fix (optional)

**Do not include:** production IPs, customer data, live API keys, or real credentials.

---

## Disclosure Policy

We follow coordinated disclosure:

| Phase | Target |
|-------|--------|
| Acknowledgment | Within 7 business days |
| Triage | Within 14 business days |
| Fix or mitigation | Severity-dependent |
| Advisory publication | After fix or agreed timeline |

Researchers who report in good faith will not be pursued for authorized testing of their own installations or agreed scope.

---

## Dependency Policy

### Current Dependencies

| Package | Purpose |
|---------|---------|
| Microsoft.WindowsAppSDK | WinUI 3 runtime |
| Microsoft.Windows.SDK.BuildTools | Windows build |
| System.Management | WMI access |
| System.Diagnostics.EventLog | Local audit checks |

### Practices

- CI runs `dotnet list package --vulnerable` on each build (informational)
- Critical CVEs in direct dependencies are addressed in patch releases
- Transitive dependency risk reviewed during release preparation
- No unnecessary packages added to reduce attack surface

### Updating Dependencies

Dependency bumps require:

- Successful Release x64 build
- Smoke test of scan and settings flows
- CHANGELOG entry for security-relevant updates

---

## Secure Development

### Source Code

- No hardcoded API keys, tokens, or credentials in tracked source
- DEBUG-only demo authentication (`#if DEBUG`)
- Generic demo network data only
- `.gitignore` blocks `config.json`, `history.json`, build artifacts

### Code Review Expectations

PRs touching these areas receive heightened review:

- `AuthService`, `ConfigService`
- `AttackService`, `SystemEnumerationService`
- `OpenAIService`, `RemediationService`
- File I/O and path handling

### Build & CI

- GitHub Actions: Windows runner, Release x64
- No secrets in workflow files
- Build failure blocks merge

---

## Threat Model

### Assets

| Asset | Risk if Compromised |
|-------|---------------------|
| User API keys (local) | Unauthorized OpenAI usage, cost |
| Scan history (local) | Exposure of assessed infrastructure |
| WMI credentials (runtime) | Unauthorized host access |
| AI-generated scripts | Unsafe system changes if executed blindly |

### Threats & Mitigations

| Threat | Mitigation |
|--------|------------|
| Secret committed to repo | Sprint cleanup, `.gitignore`, PR checklist |
| Malicious PR introducing backdoor | Review, CI, community scrutiny |
| User runs tool without authorization | DISCLAIMER, safety docs, UI warnings |
| AI script causes system harm | User review required; rollback scripts provided |
| Local config theft | DPAPI planned v1.1; OS-level disk encryption recommended |
| Misuse of offensive modules | Documentation, authorized-use framing |

### Out of Scope (User Responsibility)

- Physical access to the machine
- Compromised Windows user account
- Organizational policy enforcement
- Network-level IDS/IPS response

---

## Privacy

### Data Collection by CyberTool

CyberTool does **not** include built-in telemetry, analytics, or cloud sync.

| Data | Location | Leaves Device? |
|------|----------|----------------|
| Scan sessions | `%AppData%\CyberTool\` | No |
| Scan history | `%LocalAppData%\CyberTool\` | No |
| API key | `%AppData%\CyberTool\config.json` | No (except OpenAI API calls) |
| Error logs | `%LocalAppData%\CyberTool\errors.log` | No |
| Exported reports | User Desktop | User-controlled |

### User Responsibilities

- Classify and protect exported reports
- Secure the workstation running CyberTool
- Follow organizational data handling policies

---

## Responsible AI

### Design Choices

- AI is **opt-in** via user-provided API key
- Input is **minified** (port/service summary, not full payloads)
- Output is **advisory** — not auto-executed
- Fallback templates exist when AI is unavailable

### OpenAI Data Handling

When AI features are enabled:

1. User configures their own OpenAI API key
2. CyberTool sends scan summaries to `https://api.openai.com/v1/chat/completions`
3. Data handling is governed by **OpenAI's terms** and your organizational policy
4. CyberTool does not store AI responses server-side

### Recommendations

- Do not send classified or regulated data without approval
- Use organizational API accounts with appropriate policies
- Disable AI in air-gapped environments
- Review all generated scripts before execution

See [docs/safety.md](docs/safety.md).

---

## Safe-Use Policy

CyberTool must only be used on systems you own or have **explicit written permission** to test.

Maintainers do not condone:

- Unauthorized scanning
- Credential attacks against third parties
- Ransomware simulation on production networks without approval
- Deployment as covert surveillance or malware

See [DISCLAIMER.md](../DISCLAIMER.md).

---

## Security Contact

Report vulnerabilities via **GitHub Security Advisories** only.

For security design discussions (not vulnerabilities), use the [security discussion issue template](../.github/ISSUE_TEMPLATE/security_discussion.md).

---

## Acknowledgments

We appreciate responsible disclosure from security researchers and contributors who help keep CyberTool safe for authorized users.
