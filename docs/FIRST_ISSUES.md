# First Issues — Starter Contribution Ideas

25 realistic GitHub issues for early contributors. Copy into GitHub Issues when the repository goes public. Difficulty: **Easy** / **Medium** / **Hard**. Priority: **P0** (critical) / **P1** (high) / **P2** (medium) / **P3** (low).

---

## Documentation

### 1. Verify GitHub URLs and badges

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `documentation`, `good first issue` |
| **Priority** | P3 |
| **Description** | Verify all clone, badge, and security advisory URLs resolve to https://github.com/TahaAkgl27/CyberTool. |

### 2. Capture README screenshots per checklist

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `documentation`, `help wanted` |
| **Priority** | P1 |
| **Description** | Follow [SCREENSHOT_CHECKLIST.md](images/SCREENSHOT_CHECKLIST.md). Capture dashboard, scan, settings, results, and dark mode. Redact secrets. Update README Screenshots section. |

### 3. Record 45-second demo GIF

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `documentation` |
| **Priority** | P2 |
| **Description** | Follow [GIF_RECORDING_GUIDE.md](GIF_RECORDING_GUIDE.md). Save as `docs/images/demo.gif`. Embed in README. |

### 4. Add Turkish/English glossary for UI terms

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `documentation`, `i18n` |
| **Priority** | P2 |
| **Description** | Document Turkish UI strings with English equivalents in `docs/glossary.md` to help international contributors. |

### 5. Expand FAQ with WMI permission scenarios

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `documentation` |
| **Priority** | P2 |
| **Description** | Add FAQ entries for common WMI/RPC failures, firewall rules, and credential requirements to `docs/usage.md`. |

---

## Security & Safety

### 6. Implement DPAPI encryption for API key storage

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement`, `security` |
| **Priority** | P1 |
| **Description** | Encrypt `config.json` API key using `ProtectedData.Protect` (DPAPI). Include migration from plain-text existing configs. Target: v1.1. |

### 7. Add authorization acknowledgment before Attack module

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement`, `security` |
| **Priority** | P1 |
| **Description** | Show modal requiring user to confirm authorized scope before accessing Attack page. Log acknowledgment locally (no telemetry). |

### 8. Audit log for high-risk operations

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement`, `security` |
| **Priority** | P2 |
| **Description** | Append-only local log for Attack module usage, credential tests, and remediation script execution. Store under `%LocalAppData%\CyberTool\audit.log`. |

### 9. Dependency vulnerability scan in CI

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `ci`, `security`, `good first issue` |
| **Priority** | P2 |
| **Description** | Fail CI or open advisory comment when `dotnet list package --vulnerable` finds high/critical CVEs in direct dependencies. |

### 10. Secret scanning pre-commit hook documentation

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `documentation`, `security` |
| **Priority** | P3 |
| **Description** | Document optional `gitleaks` or `trufflehog` setup for contributors in CONTRIBUTING.md. |

---

## Features & UX

### 11. Lab Mode toggle restricting target ranges

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement`, `security` |
| **Priority** | P1 |
| **Description** | Settings toggle limiting scan targets to RFC1918, localhost, and user-defined allowlist. Block scan start for out-of-range IPs. |

### 12. PDF export for executive reports

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement` |
| **Priority** | P2 |
| **Description** | Generate PDF from executive report template. Configurable output path. Target: v1.2. |

### 13. Configurable report output directory

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `enhancement`, `good first issue` |
| **Priority** | P2 |
| **Description** | Settings field for report save location instead of hardcoded Desktop. Persist in `config.json`. |

### 14. Dark theme consistency pass

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement` |
| **Priority** | P2 |
| **Description** | Audit all XAML pages for hardcoded colors. Apply `ThemeResource` brushes for dark/light consistency. |

### 15. Scan progress bar with cancel feedback

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement`, `good first issue` |
| **Priority** | P2 |
| **Description** | Show determinate/indeterminate progress during port scan. Disable Start button while running. Clear cancel state messaging. |

### 16. Import Nmap XML scan results

| Field | Value |
|-------|-------|
| **Difficulty** | Hard |
| **Labels** | `enhancement` |
| **Priority** | P3 |
| **Description** | File picker for Nmap XML output. Parse into existing `ScanResult` model and display in Scan UI. |

### 17. History search and filter by target

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement` |
| **Priority** | P2 |
| **Description** | Add search box on Reports/History page. Filter sessions by target IP/hostname and date range. |

### 18. Keyboard shortcuts for navigation

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement` |
| **Priority** | P3 |
| **Description** | Ctrl+1–6 for sidebar navigation. Document shortcuts in usage guide. |

---

## Architecture & Plugins

### 19. Define IScanPlugin interface

| Field | Value |
|-------|-------|
| **Difficulty** | Hard |
| **Labels** | `enhancement`, `plugin` |
| **Priority** | P2 |
| **Description** | Design and implement plugin contract per [architecture.md](architecture.md). Loader discovers plugins from `%AppData%\CyberTool\Plugins\`. |

### 20. Sample plugin project in samples/

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `documentation`, `plugin`, `good first issue` |
| **Priority** | P2 |
| **Description** | Minimal plugin that adds one custom port check. README with build and install steps. |

### 21. Refactor AppServices to DI container

| Field | Value |
|-------|-------|
| **Difficulty** | Hard |
| **Labels** | `enhancement` |
| **Priority** | P3 |
| **Description** | Replace static `AppServices` locator with `Microsoft.Extensions.DependencyInjection` for testability. No behavior change. |

### 22. Unit tests for ScanService port logic

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `enhancement`, `ci` |
| **Priority** | P2 |
| **Description** | Add `CyberTool.Tests` project. Test risk classification, port parsing, and result aggregation with mocked network. |

---

## CI & Release

### 23. Add dotnet format check to CI

| Field | Value |
|-------|-------|
| **Difficulty** | Easy |
| **Labels** | `ci`, `good first issue` |
| **Priority** | P2 |
| **Description** | Run `dotnet format --verify-no-changes` in GitHub Actions. Document code style in CONTRIBUTING.md. |

### 24. Publish signed release artifacts

| Field | Value |
|-------|-------|
| **Difficulty** | Hard |
| **Labels** | `ci`, `enhancement` |
| **Priority** | P2 |
| **Description** | GitHub Actions workflow to build Release, package MSIX/exe, attach to Releases. Document signing certificate requirements. |

### 25. SBOM generation on release

| Field | Value |
|-------|-------|
| **Difficulty** | Medium |
| **Labels** | `ci`, `security` |
| **Priority** | P3 |
| **Description** | Generate SPDX or CycloneDX SBOM as release asset using `dotnet sbom` or similar tooling. |

---

## How to Use This Document

1. Create GitHub Issues from items above (one issue per item)
2. Apply suggested labels
3. Tag 3–5 items as `good first issue` for launch
4. Link issues to roadmap milestones in descriptions

**Do not invent completion status** — all items above are proposed future work unless explicitly marked done in [roadmap.md](roadmap.md).
