# Screenshot Checklist

Use this checklist before public launch or major release announcements. **Do not fabricate screenshots**—capture real application UI with sensitive data redacted.

---

## General Guidelines

| Requirement | Detail |
|-------------|--------|
| Resolution | 1280×720 minimum (16:9) |
| Theme | Capture both light and dark mode where noted |
| Redaction | Blur or omit API keys, real hostnames, internal IPs, credentials |
| Format | PNG preferred; optimize for GitHub README (< 500 KB each) |
| Consistency | Same Windows version and app build for all captures |

### Demo Data to Use

- Targets: `127.0.0.1`, `192.168.100.10`, `192.168.100.11`
- Hostnames: `LAB-PC01`, `DEMO-PC01`, `EXAMPLE.LOCAL`
- Never use production infrastructure identifiers

---

## Required Screenshots

### 1. Dashboard

| Field | Value |
|-------|-------|
| **Filename** | `screenshot-dashboard.png` |
| **Title** | CyberTool Dashboard |
| **Description** | Main landing page showing navigation sidebar, safety notice, and orientation content. Demonstrates first-launch experience for new users. |
| **Capture steps** | Launch app → Dashboard selected → ensure safety banner visible |
| **Status** | ⬜ Not captured |

---

### 2. Settings

| Field | Value |
|-------|-------|
| **Filename** | `screenshot-settings.png` |
| **Title** | Settings & API Configuration |
| **Description** | Settings page showing OpenAI API key field with placeholder or redacted value. Demonstrates local configuration without exposing secrets. |
| **Capture steps** | Settings → API section → enter `sk-••••••••` or blur field → Save button visible |
| **Status** | ⬜ Not captured |

---

### 3. Scan

| Field | Value |
|-------|-------|
| **Filename** | `screenshot-scan.png` |
| **Title** | Network Scan Workflow |
| **Description** | Scan page mid- or post-scan showing target input, port range, status, and results grid with risk classifications. |
| **Capture steps** | Scan `127.0.0.1` or `192.168.100.10` → wait for results → capture full page |
| **Status** | ⬜ Not captured |

---

### 4. Attack (Authorized Modules)

| Field | Value |
|-------|-------|
| **Filename** | `screenshot-attack.png` |
| **Title** | Authorized Security Assessment |
| **Description** | Attack module page with lab target and authorization context visible. Shows module exists for training—not covert use. |
| **Capture steps** | Attack page → lab IP `192.168.100.10` → generic credentials → disclaimer/warning visible if present |
| **Status** | ⬜ Not captured |

---

### 5. History / Reports

| Field | Value |
|-------|-------|
| **Filename** | `screenshot-history.png` |
| **Title** | Scan History & Sessions |
| **Description** | Reports or history view listing prior scan sessions with timestamps and targets (generic lab data only). |
| **Capture steps** | Run 2+ scans → open Reports/History → capture session list |
| **Status** | ⬜ Not captured |

---

### 6. Results / Risk Dashboard

| Field | Value |
|-------|-------|
| **Filename** | `screenshot-results.png` |
| **Title** | Risk Analysis Dashboard |
| **Description** | Post-scan risk score, severity breakdown, executive summary, and recommendations panel. |
| **Capture steps** | Complete scan with open ports → capture risk dashboard section |
| **Status** | ⬜ Not captured |

---

### 7. AI Analysis

| Field | Value |
|-------|-------|
| **Filename** | `screenshot-ai-analysis.png` |
| **Title** | AI-Assisted Analysis |
| **Description** | Attack scenarios or AI-generated analysis panel after scan. API key must be configured; redact any key references. |
| **Capture steps** | Configure API key → run scan → trigger AI analysis → capture output panel |
| **Status** | ⬜ Not captured |

---

### 8. Dark Mode

| Field | Value |
|-------|-------|
| **Filename** | `screenshot-dark-mode.png` |
| **Title** | Dark Theme |
| **Description** | Application in dark mode showing Scan or Dashboard for visual polish in README and release notes. |
| **Capture steps** | Enable Windows/app dark theme → capture Scan or Dashboard |
| **Status** | ⬜ Not captured |

---

## Optional Screenshots

| Filename | Title | Description |
|----------|-------|-------------|
| `screenshot-device-profile.png` | Device Profile | WMI-enriched host details after scan |
| `screenshot-export.png` | Report Export | Desktop export confirmation or sample report |
| `screenshot-ransomware.png` | Propagation Awareness | Training simulation module (lab context) |

---

## Logo Asset

| Field | Value |
|-------|-------|
| **Filename** | `logo.png` |
| **Title** | Project Logo |
| **Description** | Square or wide logo for README hero section. Currently placeholder until branded asset is designed. |
| **Recommended size** | 256×256 or 512×512 PNG with transparency |
| **Status** | ✅ `logo.png` committed (branded asset may replace later) |

---

## README Integration

After capturing, update `README.md` Screenshots section:

```markdown
| Dashboard | Scan | Settings |
|:---:|:---:|:---:|
| ![Dashboard](docs/images/screenshot-dashboard.png) | ![Scan](docs/images/screenshot-scan.png) | ![Settings](docs/images/screenshot-settings.png) |
```

---

## Verification Checklist

- [ ] All filenames match this document
- [ ] No real API keys visible
- [ ] No production hostnames or IPs
- [ ] No personal paths (`C:\Users\...`)
- [ ] Images committed to `docs/images/`
- [ ] README links updated
- [ ] Social preview image created (1280×640 for GitHub)
