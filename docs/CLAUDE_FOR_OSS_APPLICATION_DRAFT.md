# Claude for Open Source — Application Draft

**Project:** CyberTool  
**Repository:** https://github.com/TahaAkgl27/CyberTool  
**License:** MIT  
**Release:** v1.0.0 (initial public release)

Use this draft as a starting point for a Claude for Open Source application. Keep claims factual. Do not invent stars, downloads, or adoption metrics.

---

## What CyberTool Is

CyberTool is an open-source Windows desktop application for **authorized cybersecurity diagnostics and IT administration**. Built with **WinUI 3** and **.NET 8**, it combines:

- Network port scanning and risk classification
- Windows enumeration (including WMI)
- Security assessment helpers and attack-chain visualization
- Optional AI-assisted explanations (user-provided OpenAI API key)
- Local scan history and reporting
- Training-oriented modules for labs and education

It is **local-first**: scan data and configuration stay on the user’s machine unless the user explicitly enables optional cloud AI features.

---

## Why It Is Open Source

Security tools require trust. Open source lets administrators, educators, and auditors **inspect** what the software does, verify that data handling is local, and contribute improvements without vendor lock-in.

CyberTool includes assessment and simulation capabilities that must remain **transparent and ethically framed**. Publishing the code, safety policy, and disclaimer makes those boundaries visible and community-accountable.

---

## Who Benefits

| Audience | Benefit |
|----------|---------|
| Windows administrators & IT engineers | Unified diagnostics workflow instead of scattered scripts |
| Helpdesk / SOC / blue teams | Consistent findings, local history, exportable summaries |
| Students & labs | Hands-on Windows security learning with readable source |
| Educators | Free, inspectable toolkit for authorized classroom use |
| Contributors | Clear MVVM architecture and documented contribution paths |

---

## How It Helps Windows Admins and Students

**Admins** get a structured path from scan → risk view → remediation suggestions without standing up a full enterprise scanner for every lab or staging check.

**Students** learn network fundamentals, Windows internals, and responsible tooling design from a real WinUI codebase—not only theory slides. Safety docs and disclaimers reinforce authorized-use culture.

---

## Safety-First Approach

CyberTool is **not malware** and is **not** intended for unauthorized access.

Commitments reflected in the repository:

- Explicit [DISCLAIMER.md](../DISCLAIMER.md) and [docs/safety.md](safety.md)
- [SECURITY.md](../SECURITY.md) with coordinated disclosure via GitHub Advisories
- No hardcoded API keys in source
- Generic lab demo data only
- Optional AI is opt-in; core features work offline
- Offensive-adjacent modules documented for authorized / training use

---

## Why Claude Would Help

CyberTool has a large **documentation and safety surface**, a WinUI/MVVM codebase that benefits from careful review, and a roadmap that prioritizes secure credential storage, exports, tests, and lab-safe defaults.

Claude can accelerate:

- Clearer safety and contributor documentation
- Design and implementation guidance for DPAPI / Credential Manager
- Test scenario design with generic lab data
- Issue triage and PR review assistance
- Localization and accessibility copy

Human maintainers retain final review. CI validates builds. Security-sensitive modules receive heightened scrutiny.

---

## Planned Work With Claude

Aligned with the public roadmap:

1. **v1.1** — Encrypt API keys (DPAPI / Credential Manager)
2. **Lab Mode** — Restrict targets to private ranges
3. **Exports** — PDF / JSON / CSV reporting
4. **Tests** — Unit tests for scan and risk logic
5. **Docs** — Continuous improvement of architecture, usage, and safety materials
6. **Later** — Plugin SDK foundations and offline AI options (design-first)

Starter issues are drafted in [`STARTER_ISSUES_TO_CREATE.md`](STARTER_ISSUES_TO_CREATE.md).

---

## Honest Note on Project Stage

CyberTool is an **early-stage public release**. We do not claim large community adoption, download counts, or production deployment statistics. The repository is structured for professionalism and trust—complete docs, CI, safety policy, and a tagged `v1.0.0`—so that growth can be earned transparently.

Claude for Open Source support would help maintainers ship safer storage, better tests, and clearer documentation faster—without expanding unauthorized offensive capability.

---

## Summary Sentence (optional short form)

> CyberTool is a local-first, open-source Windows security diagnostics toolkit for authorized admins, SOC practitioners, and students. We are applying for Claude for Open Source to accelerate secure credential storage, testing, and safety-focused documentation on a transparent MIT-licensed codebase.
