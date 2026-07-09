# GitHub Final Setup Guide

Exact manual settings for https://github.com/TahaAkgl27/CyberTool before Claude for Open Source application.

These cannot be applied from git — configure them in the GitHub web UI.

---

## 1. Repository Description

**Path:** Settings → General → Description

```
Open-source Windows cybersecurity & IT diagnostics toolkit built with WinUI 3 and .NET 8.
```

---

## 2. Homepage

**Path:** Settings → General → Website

```
https://github.com/TahaAkgl27/CyberTool#readme
```

---

## 3. Topics

**Path:** Settings → General → Topics (or click gear next to About on the repo home page)

Add exactly:

```
windows
dotnet
winui3
cybersecurity
security-tools
port-scanner
wmi
blue-team
open-source
windows-security
it-admin
network-scanner
```

---

## 4. Social Preview

**Path:** Settings → General → Social preview → Edit

Upload:

```
docs/images/CyberTool.jpg
```

Recommended crop: 1280×640 if GitHub asks for a banner ratio. The full UI screenshot is acceptable as a starting asset.

---

## 5. Features to Enable

**Path:** Settings → General → Features

| Feature | Enable |
|---------|--------|
| Issues | ✅ On |
| Discussions | ✅ On |
| Wiki | ✅ On (optional; docs already live in `/docs`) |
| Projects | ✅ On (optional for roadmap tracking) |
| Sponsorships | Optional |
| Preserve this repository | Optional |

**Path:** Settings → Code security and analysis

| Feature | Enable |
|---------|--------|
| Dependency graph | ✅ On |
| Dependabot alerts | ✅ On (recommended) |
| Secret scanning | ✅ On (recommended) |
| Private vulnerability reporting / Security advisories | ✅ On |

---

## 6. GitHub Release (if not published yet)

1. Go to https://github.com/TahaAkgl27/CyberTool/releases/new
2. Choose existing tag: **`v1.0.0`**
3. Title: **`CyberTool v1.0.0 — Initial Public Release`**
4. Paste body from [`docs/RELEASE_NOTES_v1.0.0.md`](RELEASE_NOTES_v1.0.0.md)
5. Publish release

Tag is already pushed: `v1.0.0` → commit `8ec7237`

---

## 7. Labels (recommended)

Create if missing: `bug`, `enhancement`, `documentation`, `security`, `good first issue`, `help wanted`, `question`

See also: [GITHUB_REPOSITORY.md](GITHUB_REPOSITORY.md)

---

## 8. Starter Issues

Copy issues from [`STARTER_ISSUES_TO_CREATE.md`](STARTER_ISSUES_TO_CREATE.md) into GitHub Issues after labels exist.

---

## Checklist

- [ ] Description set
- [ ] Homepage set
- [ ] Topics added
- [ ] Social preview uploaded
- [ ] Issues / Discussions enabled
- [ ] Security advisories enabled
- [ ] Release `v1.0.0` published
- [ ] 5–10 starter issues created
