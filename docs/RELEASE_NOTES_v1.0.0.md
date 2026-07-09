# CyberTool v1.0.0 — Initial Public Release

**Tag:** `v1.0.0`  
**Repository:** https://github.com/TahaAkgl27/CyberTool  
**Date:** July 9, 2026

---

## Highlights

- First public open-source release of CyberTool
- Premium dark WinUI 3 interface for Windows 10/11
- Local-first diagnostics — scan data stays on your machine
- Optional AI analysis via user-provided OpenAI API key
- Full safety documentation: SECURITY, DISCLAIMER, CONTRIBUTING

---

## Main Features

- **Network discovery** — TCP port scanning, service identification, risk scoring
- **Windows enumeration** — WMI deep scan, device profiling, system inventory
- **Security assessment** — attack chain visualization, compliance hints, remediation suggestions
- **Reports & history** — local session persistence, executive and technical exports
- **Training modules** — authorized attack helpers and ransomware awareness simulation
- **Settings** — OpenAI API key configuration (optional)

---

## Requirements

- Windows 10 (build 17763+) or Windows 11
- .NET SDK 8.0
- Windows App SDK (via NuGet restore)
- x64 recommended

---

## Installation

```powershell
git clone https://github.com/TahaAkgl27/CyberTool.git
cd CyberTool
dotnet restore CyberTool.csproj
dotnet build CyberTool.csproj -c Release -p:Platform=x64
.\bin\x64\Release\net8.0-windows10.0.19041.0\CyberTool.exe
```

Or open in Visual Studio and run the **CyberTool (Unpackaged)** profile.

---

## Safety Notice

CyberTool is for **authorized use only** — labs, education, and systems you own or have written permission to assess.

Read before use:

- [DISCLAIMER.md](../DISCLAIMER.md)
- [SECURITY.md](../SECURITY.md)
- [docs/safety.md](safety.md)

Do not scan third-party networks without authorization.

---

## Known Limitations

- OpenAI API key stored in plain text locally (DPAPI planned for v1.1)
- No signed release binaries in this tag — build from source
- Settings section navigation is visual; core API key save/clear is fully functional
- Demo / lab data only — do not use against unauthorized targets

---

## Roadmap

| Version | Focus |
|---------|-------|
| v1.1 | DPAPI / Credential Manager |
| v1.2 | Reporting export, Plugin SDK |
| v1.5 | Localization |
| v2.0 | Enterprise Safe Mode, offline AI |

See [docs/roadmap.md](roadmap.md).

---

## License

[MIT License](../LICENSE) — Copyright (c) 2026 CyberTool Contributors.

---

## Links

- [README](https://github.com/TahaAkgl27/CyberTool#readme)
- [Security Policy](https://github.com/TahaAkgl27/CyberTool/blob/main/SECURITY.md)
- [Architecture](https://github.com/TahaAkgl27/CyberTool/blob/main/docs/architecture.md)
- [User Guide](https://github.com/TahaAkgl27/CyberTool/blob/main/docs/usage.md)
