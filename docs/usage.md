# CyberTool User Guide

Complete operator guide for authorized Windows security diagnostics with CyberTool.

---

## Table of Contents

1. [Installation](#installation)
2. [First Launch](#first-launch)
3. [Configure API Key](#configure-api-key)
4. [Run a Scan](#run-a-scan)
5. [Analyze Results](#analyze-results)
6. [Device Profile](#device-profile)
7. [History & Sessions](#history--sessions)
8. [Reports & Export](#reports--export)
9. [Security Modules](#security-modules-authorized-only)
10. [Troubleshooting](#troubleshooting)
11. [Best Practices](#best-practices)
12. [FAQ](#faq)

---

## Installation

### From Source (Current)

**Prerequisites:**

- Windows 10 (build 17763+) or Windows 11
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 with Windows development workload (recommended)

**Steps:**

```powershell
git clone https://github.com/TahaAkgl27/CyberTool.git
cd CyberTool
dotnet restore CyberTool.csproj
dotnet build CyberTool.csproj -c Release -p:Platform=x64
```

**Launch:**

- Visual Studio: profile **CyberTool (Unpackaged)**
- Or run: `.\bin\x64\Release\net8.0-windows10.0.19041.0\CyberTool.exe`

### Future Binary Releases

Pre-built releases will be published on [GitHub Releases](https://github.com/TahaAkgl27/CyberTool/releases) when available.

---

## First Launch

1. Start CyberTool
2. Review the **Dashboard** safety notice
3. Confirm you are operating in an **authorized environment**
4. Navigate via the left menu:
   - **Dashboard** — orientation
   - **Scan** — primary workflow
   - **Device Profile** — host details
   - **Reports** — session history
   - **Settings** — API configuration

Authentication is optional in the current release; the application opens directly to the dashboard.

---

## Configure API Key

AI features are **optional**. Without a key, scanning and enumeration work normally.

### Steps

1. Open **Settings**
2. Locate **OpenAI API Anahtarı** section
3. Enter your key (format: `sk-...`)
4. Click **Kaydet** (Save)
5. Confirm status message appears

### Storage Location

```
%AppData%\CyberTool\config.json
```

The key is stored in plain text locally (DPAPI planned for v1.1). Never commit this file or share it.

### Clear Key

Click **Temizle** (Clear) in Settings to remove the stored key.

---

## Run a Scan

### Basic Scan

1. Navigate to **Scan**
2. Enter **Target** IP or hostname (authorized only)
   - Local: `127.0.0.1`
   - Lab demo: `192.168.100.10`
3. Set **Port From** / **Port To** (default: 1–1000)
4. Click **Start** / scan button
5. Monitor **Status** text during execution
6. Review results when complete

### What Happens Internally

- TCP connect probes per port in range
- Service identification and risk classification
- Optional enumeration enrichment
- Session saved to local history

### Cancel

Use the cancel control to stop an in-progress scan.

---

## Analyze Results

### Result Columns

Each finding typically includes:

| Field | Meaning |
|-------|---------|
| Port | TCP port number |
| State | open / closed / filtered |
| Service | Detected or inferred service |
| Risk | Severity tier (Critical, High, Medium, Low) |
| Access Scope | Internal vs. internet-facing |
| Recommendation | Suggested remediation |

### Risk Dashboard

The scan page displays:

- **Risk Score** (0–100)
- Critical / Medium / Low counts
- External vs. internal service breakdown
- Executive summary text
- Attack surface summary

### AI Analysis (Optional)

If API key is configured:

1. Complete a scan with verified open ports
2. Trigger AI analysis (when available in UI)
3. Review generated **Attack Scenarios**
4. Treat output as advisory—validate before action

### Remediation

Hardening suggestions may include:

- Download remediation script
- Apply fix (review script first)
- Rollback script where provided

**Always review PowerShell scripts before execution.**

---

## Device Profile

After scanning a target:

1. Open **Device Profile**
2. Review simulated or enumerated data:
   - Hostname, OS, domain
   - CPU, RAM, manufacturer
   - Extended protocol analysis (SMB, web, database hints)
3. Use **Deep Scan** with authorized credentials for WMI enrichment

### Demo Lab Targets

| Target | Display Name |
|--------|--------------|
| `192.168.100.10` | LAB-PC01 |
| `192.168.100.11` | DEMO-PC01 |

These show generic training data, not real infrastructure.

---

## History & Sessions

### Session Store

```
%AppData%\CyberTool\history.json
```

Contains recent scan sessions with findings.

### Trend History

```
%LocalAppData%\CyberTool\History\scan_history.json
```

Tracks when ports were first observed open (days-open metric).

### Reports Page

View and manage prior sessions from the **Reports** navigation item.

---

## Reports & Export

### Executive Report

- Summary for stakeholders
- Risk score and business impact language
- Saved to Desktop: `Executive_Report_{target}_{time}.txt`

### Technical Report

- Port table and vulnerability detail
- Saved to Desktop: `Technical_Report_{target}_{time}.txt`

---

## Security Modules (Authorized Only)

> **Requires written authorization.** See [DISCLAIMER.md](../DISCLAIMER.md).

| Module | Purpose |
|--------|---------|
| **Attack** | Authorized SMB credential assessment helpers |
| **Ransomware** | Subnet propagation awareness simulation |

Document your scope, targets, and approval before use.

---

## Troubleshooting

| Symptom | Possible Cause | Action |
|---------|----------------|--------|
| Build fails | Missing Windows SDK | Install VS Windows workload |
| Scan timeout | Firewall / unreachable host | Verify connectivity, reduce port range |
| WMI fails | Credentials / permissions | Provide valid admin creds; check RPC |
| AI unavailable | Missing/invalid API key | Re-enter key in Settings |
| Empty device profile | No scan session | Run scan first |
| High false positives | Heuristic limits | Cross-validate with manual tools |

### Logs

```
%LocalAppData%\CyberTool\errors.log
```

Include redacted excerpts in bug reports—never secrets.

---

## Best Practices

1. **Scope first** — Document authorization before any scan
2. **Start local** — Practice on `127.0.0.1` or lab VMs
3. **Notify SOC** — Inform monitoring teams in production-adjacent environments
4. **Review AI output** — Treat as draft, not gospel
5. **Backup before remediation** — Especially for registry/script changes
6. **Use generic examples** — In docs, issues, and demos
7. **Rotate API keys** — If a key may have been exposed
8. **Keep CyberTool updated** — Watch releases for security fixes

---

## FAQ

**Q: Can I scan my entire corporate network?**  
A: Only with explicit IT/security authorization and defined scope.

**Q: Does CyberTool replace Nessus/OpenVAS?**  
A: No. It complements workflows for hands-on Windows diagnostics and education.

**Q: Is internet required?**  
A: No for core features. Yes for optional OpenAI analysis.

**Q: Where is data stored?**  
A: Locally under `%AppData%` and `%LocalAppData%`. See [architecture.md](architecture.md).

**Q: How do I contribute?**  
A: See [CONTRIBUTING.md](../CONTRIBUTING.md) and [FIRST_ISSUES.md](FIRST_ISSUES.md).

---

## Related

- [safety.md](safety.md)
- [architecture.md](architecture.md)
- [roadmap.md](roadmap.md)
