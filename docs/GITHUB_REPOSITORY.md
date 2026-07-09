# GitHub Repository Quality Guide

Recommendations for presenting CyberTool as a professional open source project on GitHub. Published at [TahaAkgl27/CyberTool](https://github.com/TahaAkgl27/CyberTool).

---

## Repository Description

**Suggested (short, ≤ 350 chars):**

```
Open-source Windows cybersecurity & IT diagnostics toolkit. Network scanning, WMI enumeration, risk assessment, optional AI analysis. Authorized use only. WinUI 3 / .NET 8.
```

**Apply in:** GitHub → Settings → General → Description

---

## Homepage

**Suggested homepage URL:**

```
https://github.com/TahaAkgl27/CyberTool#readme
```

When a project website exists, update to `https://TahaAkgl27.github.io/CyberTool/` or dedicated domain.

**Apply in:** GitHub → Settings → General → Website

---

## Repository Topics

Add these topics in **Settings → General → Topics**:

```
windows
cybersecurity
security-tools
winui3
dotnet
csharp
mvvm
network-scanner
port-scanner
wmi
blue-team
it-admin
open-source
diagnostics
penetration-testing
security-education
windows-security
```

**Core (minimum):** `windows`, `cybersecurity`, `dotnet`, `winui3`, `open-source`, `security-tools`

---

## Social Preview Image

| Field | Recommendation |
|-------|----------------|
| Size | 1280×640 px |
| Content | Logo + tagline + subtle UI mock |
| File | Upload via Settings → Social preview |
| Source | Compose from `docs/images/logo.png` + README tagline |

Until branded asset exists, use a simple banner: dark background, CyberTool title, "Authorized Windows Security Diagnostics".

---

## Suggested Release

### v1.0.0 — Initial Public Release

**Title:**
```
CyberTool v1.0.0 — Initial Open Source Release
```

**Description:**
```markdown
## CyberTool v1.0.0

First public open source release of CyberTool — a local-first Windows security diagnostics toolkit.

### Highlights
- Network port scanning with risk classification
- Windows WMI enumeration and device profiling
- Optional OpenAI-assisted analysis (user-provided API key)
- Local scan history and report export
- MIT license with full security and safety documentation

### Requirements
- Windows 10 (17763+) or Windows 11
- .NET 8 Runtime (for pre-built binaries when available)
- Build from source: .NET SDK 8.0 + Windows App SDK

### Documentation
- [User Guide](docs/usage.md)
- [Architecture](docs/architecture.md)
- [Security Policy](SECURITY.md)
- [Disclaimer](DISCLAIMER.md)

### Authorized Use Only
Read [DISCLAIMER.md](DISCLAIMER.md) before use. For authorized environments, labs, and education only.

### Build
```powershell
dotnet build CyberTool.csproj -c Release -p:Platform=x64
```

### Known Limitations
- API key stored in plain text locally (DPAPI planned v1.1)
- Placeholder screenshots in README (see docs/images/SCREENSHOT_CHECKLIST.md)
- GitHub URLs point to TahaAkgl27/CyberTool
```

---

## Suggested GitHub Labels

Create these labels in **Issues → Labels**:

| Label | Color | Description |
|-------|-------|-------------|
| `bug` | `#d73a4a` | Something isn't working |
| `enhancement` | `#a2eeef` | New feature or request |
| `documentation` | `#0075ca` | Improvements or additions to docs |
| `security` | `#b60205` | Security-related (not secret vulnerabilities) |
| `help wanted` | `#008672` | Extra attention needed; good for contributors |
| `good first issue` | `#7057ff` | Good for newcomers |
| `question` | `#d876e3` | Further information requested |
| `dependencies` | `#0366d6` | Dependency updates |
| `ci` | `#1d76db` | CI/CD and build |
| `windows` | `#0078d4` | Windows-specific |
| `ai` | `#5319e7` | OpenAI / AI features |
| `plugin` | `#fbca04` | Plugin SDK / extensibility |
| `i18n` | `#c5def5` | Localization |
| `wontfix` | `#ffffff` | Will not be addressed |
| `duplicate` | `#cfd3d7` | Duplicate issue |

---

## Branch Protection (Recommended)

When repository goes public:

- Require PR reviews for `main`
- Require status check: `build` workflow
- Disallow force push to `main`
- Enable GitHub Security Advisories

---

## Community Files Checklist

| File | Status |
|------|--------|
| README.md | ✅ |
| LICENSE | ✅ MIT |
| SECURITY.md | ✅ |
| CONTRIBUTING.md | ✅ |
| CODE_OF_CONDUCT.md | ✅ |
| DISCLAIMER.md | ✅ |
| CHANGELOG.md | ✅ |
| Issue templates | ✅ |
| PR template | ✅ |
| CI workflow | ✅ |

---

## Pre-Publish Actions

1. Add repository topics
2. Enable Issues and Discussions (optional)
3. Upload social preview image
4. Create `v1.0.0` release tag after screenshot pass
5. Pin README and CONTRIBUTING in wiki or discussions

---

## Do Not Claim

- Download counts you cannot verify
- Star counts or "widely adopted" language
- Corporate endorsements without permission
- Fabricated contributor statistics

Truthful presentation builds long-term trust.
