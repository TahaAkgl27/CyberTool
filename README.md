<p align="center">
  <img src="docs/images/logo.png" alt="CyberTool" width="120" />
</p>

<h1 align="center">CyberTool</h1>

<p align="center">
  <strong>Open-source Windows cybersecurity and IT diagnostics for authorized environments.</strong>
</p>

<p align="center">
  <a href="https://github.com/TahaAkgl27/CyberTool/actions/workflows/build.yml"><img src="https://img.shields.io/github/actions/workflow/status/TahaAkgl27/CyberTool/build.yml?branch=main&style=flat-square&label=Build" alt="Build" /></a>
  <a href="https://github.com/TahaAkgl27/CyberTool/releases/tag/v1.0.0"><img src="https://img.shields.io/badge/Release-v1.0.0-blue?style=flat-square" alt="Release" /></a>
  <a href="LICENSE"><img src="https://img.shields.io/badge/License-MIT-blue?style=flat-square" alt="License" /></a>
  <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt=".NET 8" /></a>
  <a href="https://www.microsoft.com/windows"><img src="https://img.shields.io/badge/Windows-10%2F11-0078D4?style=flat-square&logo=windows&logoColor=white" alt="Windows" /></a>
  <a href="SECURITY.md"><img src="https://img.shields.io/badge/Security-Policy-red?style=flat-square" alt="Security Policy" /></a>
</p>

<p align="center">
  <a href="#overview">Overview</a> ·
  <a href="#features">Features</a> ·
  <a href="#installation">Installation</a> ·
  <a href="#screenshots">Screenshots</a> ·
  <a href="#architecture">Architecture</a> ·
  <a href="#roadmap">Roadmap</a> ·
  <a href="#security">Security</a> ·
  <a href="#contributing">Contributing</a> ·
  <a href="#license">License</a>
</p>

---

## Overview

**CyberTool** is a local-first Windows desktop application built with **WinUI 3** and **.NET 8**. It unifies network discovery, Windows system enumeration, security assessment helpers, optional AI-assisted analysis, and training-oriented simulations in one transparent toolkit.

Maintained as an independent open-source project by TahaAkgl27 and contributors.

Organizations and learners often juggle separate scripts, scanners, and ad-hoc PowerShell for basic Windows diagnostics. CyberTool reduces that fragmentation with a structured workflow—from port scan to device profile to exportable reports—while keeping data on the local machine.

> **Authorized use only.** CyberTool includes security testing and simulation capabilities. Read [DISCLAIMER.md](DISCLAIMER.md) before use.

| | |
|---|---|
| **Problem** | Fragmented Windows security diagnostics across multiple tools and scripts |
| **Solution** | Unified, inspectable, local-first diagnostics and training platform |
| **Model** | Open source (MIT), community-driven, safety-documented |

### Who Should Use CyberTool

| Operations & Security | Education & Research |
|-----------------------|----------------------|
| Windows Administrators | Cybersecurity Students |
| IT Engineers | Training Labs |
| Helpdesk Teams | Educational Institutions |
| SOC / Blue Teams | Authorized Red Team exercises |

CyberTool is **not** intended for unauthorized testing or use against systems without explicit permission.

---

## Features

### System Diagnostics

| Capability | Description |
|------------|-------------|
| Device profiling | Host metadata, OS context, and extended protocol hints |
| WMI enumeration | Remote Windows metadata via authenticated queries |
| Hardening insights | Registry and configuration checks for local posture |
| Executive summaries | Risk scoring and stakeholder-ready narratives |

### Network Discovery

| Capability | Description |
|------------|-------------|
| Port scanning | Configurable TCP port range analysis |
| Service identification | Protocol and service mapping per open port |
| Exposure analysis | Internal vs. internet-facing scope classification |
| Attack surface summary | Aggregated external and risky service counts |

### Security Assessment

| Capability | Description |
|------------|-------------|
| Risk scoring | Severity tiers with rationale per finding |
| Compliance hints | Framework-oriented violation mapping |
| Remediation suggestions | Template and AI-assisted fix scripts with rollback |
| Attack graph simulation | Chain-style path modeling for defense planning |

### Windows Enumeration

| Capability | Description |
|------------|-------------|
| WMI deep scan | Authenticated enumeration of remote hosts |
| SMB context | Signing status and related protocol signals |
| System inventory | OS, CPU, RAM, domain/workgroup heuristics |
| Nmap XML import | Ingest external scan results |

### AI Assisted Analysis

| Capability | Description |
|------------|-------------|
| Scan explanation | Optional OpenAI-powered attack scenario narratives |
| Remediation generation | Context-aware PowerShell packages (user-reviewed) |
| Fully optional | Core features work without any API key |

### Incident Investigation & Training

| Capability | Description |
|------------|-------------|
| Session history | Local persistence of scan sessions |
| Technical / executive reports | Exportable summaries for stakeholders |
| Lab demo data | Generic sample hosts for classroom scenarios |
| Ransomware simulation | Subnet awareness training (authorized labs only) |

---

## Installation

### Requirements

- Windows 10 (1809+) or Windows 11
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows App SDK (restored via NuGet)
- x64 recommended

### Build from Source

```powershell
git clone https://github.com/TahaAkgl27/CyberTool.git
cd CyberTool
dotnet restore CyberTool.csproj
dotnet build CyberTool.csproj -c Release -p:Platform=x64
```

Run with Visual Studio (**CyberTool (Unpackaged)**) or:

```powershell
.\bin\x64\Release\net8.0-windows10.0.19041.0\CyberTool.exe
```

### Configure OpenAI (Optional)

1. Open **Settings**
2. Enter your API key (`sk-...`)
3. Click **Save**

The key is stored locally at `%AppData%\CyberTool\config.json` — never in source control.

Full guide: [docs/usage.md](docs/usage.md)

---

## Screenshots

<p align="center">
  <img src="docs/images/CyberTool.jpg" alt="CyberTool v1.0.0 dashboard and diagnostics UI" width="100%">
</p>

<p align="center">
  <em>CyberTool v1.0.0 on Windows — premium dark UI with port scanning, attack chain visualization, security recommendations, and host analysis in a controlled demo environment.</em>
</p>

---

## Architecture

CyberTool follows **MVVM** with a service-oriented backend:

```
Views (WinUI 3)  →  ViewModels  →  Services  →  Models / Local Storage
```

- **Views:** Dashboard, Scan, Device Profile, Attack, Ransomware, Reports, Settings
- **Services:** Scan orchestration, WMI, OpenAI, remediation, reporting, history
- **Storage:** `%AppData%\CyberTool\` (config, history), `%LocalAppData%\CyberTool\` (logs)

Full diagrams: [docs/architecture.md](docs/architecture.md)

---

## Roadmap

| Version | Focus |
|---------|-------|
| **v1.0** | Public release, safety docs, CI, premium UI |
| **v1.1** | DPAPI / Credential Manager for API keys |
| **v1.2** | Reporting export, Plugin SDK foundation |
| **v1.5** | Localization, dark theme improvements |
| **v2.0** | Enterprise Safe Mode, plugin marketplace, offline AI |

Details: [docs/roadmap.md](docs/roadmap.md)

---

## Security

| Document | Purpose |
|----------|---------|
| [SECURITY.md](SECURITY.md) | Vulnerability reporting and secure development |
| [DISCLAIMER.md](DISCLAIMER.md) | Authorized-use legal notice |
| [docs/safety.md](docs/safety.md) | Operational safety guide |

Report vulnerabilities via [GitHub Security Advisories](https://github.com/TahaAkgl27/CyberTool/security/advisories/new) only.

---

## Contributing

We welcome contributions that improve safety, documentation, and diagnostics quality.

- [CONTRIBUTING.md](CONTRIBUTING.md) — setup and PR checklist
- [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md) — community standards
- [docs/FIRST_ISSUES.md](docs/FIRST_ISSUES.md) — starter issue ideas

```powershell
dotnet build CyberTool.csproj -c Release -p:Platform=x64
```

---

## License

[MIT License](LICENSE) — Copyright (c) 2026 CyberTool Contributors.

---

## FAQ

<details>
<summary><strong>Is CyberTool malware?</strong></summary>

No. CyberTool is a legitimate diagnostics and training application. See [DISCLAIMER.md](DISCLAIMER.md).
</details>

<details>
<summary><strong>Can I scan any IP address?</strong></summary>

Only systems you own or have **written authorization** to assess.
</details>

<details>
<summary><strong>Does CyberTool send data to the cloud?</strong></summary>

Core scanning and history are local-only. Optional OpenAI features send scan summaries only when you configure an API key.
</details>

<details>
<summary><strong>Do I need an OpenAI API key?</strong></summary>

No. Scanning, enumeration, reporting, and simulation work without AI.
</details>

---

<p align="center">
  <sub>Built with WinUI 3 · .NET 8 · Windows App SDK</sub><br/>
  <sub>Copyright (c) 2026 CyberTool Contributors · <a href="LICENSE">MIT License</a></sub>
</p>
