# Changelog

All notable changes to CyberTool are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-07-09

### Added

- Initial public release of CyberTool
- Windows cybersecurity diagnostics toolkit (WinUI 3 / .NET 8)
- Network port scanning and risk assessment workflows
- Windows system enumeration via WMI
- SMB and security testing helper modules (authorized use)
- Ransomware propagation simulation for awareness and training
- OpenAI-assisted scan explanation and remediation script suggestions
- Local scan history and session persistence
- Settings page for user-provided OpenAI API key configuration
- Safety documentation: [DISCLAIMER.md](DISCLAIMER.md), [docs/safety.md](docs/safety.md)
- Open source documentation: README, LICENSE (MIT), SECURITY, CONTRIBUTING, Code of Conduct
- GitHub issue/PR templates and CI build workflow

### Security

- Removed hardcoded API keys and personal identifiers from source
- Debug-only demo authentication credentials (`#if DEBUG`)
- Professional `.gitignore` for build artifacts and runtime secrets
- Generic demo network data for lab scenarios

### Changed

- Publisher and branding normalized to **CyberTool** for open source release
- Premium dark UI refresh (WinUI 3) for dashboard, scan, reports, settings, and related pages
- README updated with v1.0.0 screenshot and release badges

[1.0.0]: https://github.com/TahaAkgl27/CyberTool/releases/tag/v1.0.0
