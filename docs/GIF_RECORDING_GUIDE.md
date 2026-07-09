# GIF Recording Guide

Record a **30–60 second** demo GIF for README, release notes, or social preview. Keep it professional, use lab data only, and redact secrets.

---

## Recommended Tools

| Tool | Platform | Notes |
|------|----------|-------|
| [ScreenToGif](https://www.screentogif.com/) | Windows | Free, export GIF/MP4, easy frame editing |
| [ShareX](https://getsharex.com/) | Windows | Screen capture + GIF encoding |
| [LICEcap](https://www.cockos.com/licecap/) | Windows/macOS | Simple, lightweight |
| OBS Studio | Windows | Record MP4 → convert to GIF if needed |

---

## Recording Settings

| Setting | Recommendation |
|---------|----------------|
| Duration | 30–60 seconds |
| Resolution | 1280×720 |
| Frame rate | 10–15 fps (GIF) or 30 fps (MP4) |
| Window | CyberTool maximized or fixed 1280×720 region |
| Cursor | Visible; move deliberately |
| Theme | Light mode (or match README screenshots) |

### GIF Optimization

- Target file size: **< 5 MB** for GitHub README
- Reduce colors in ScreenToGif (128–256)
- Crop to app window only (no desktop clutter)

---

## Suggested Demo Flow (45 seconds)

| Time | Action | Narrative Purpose |
|------|--------|-------------------|
| 0:00–0:05 | Launch CyberTool | Show clean startup |
| 0:05–0:10 | Dashboard glance | Safety-first orientation |
| 0:10–0:25 | Navigate to Scan → enter `127.0.0.1` → start scan | Core workflow |
| 0:25–0:35 | Results appear → risk score visible | Value demonstration |
| 0:35–0:42 | Open History or Reports | Persistence feature |
| 0:42–0:48 | Quick Settings view (key redacted) | Configuration |
| 0:48–0:55 | Export report or Device Profile | Depth of toolkit |
| 0:55–1:00 | Return to Dashboard | Clean ending |

Adjust timing to stay within 60 seconds.

---

## Pre-Recording Checklist

- [ ] Build Release x64 successfully
- [ ] Clear `%AppData%\CyberTool\` of sensitive history (or use fresh VM)
- [ ] API key field empty or redacted in Settings
- [ ] Target only `127.0.0.1` or `192.168.100.x`
- [ ] Close unrelated windows and notifications
- [ ] Disable Windows focus assist / notifications

---

## Post-Recording Checklist

- [ ] Review every frame for leaked secrets
- [ ] Blur taskbar if personal info visible
- [ ] Save as `docs/images/demo.gif` or attach to release
- [ ] Add alt text in README: "CyberTool 45-second workflow demo"
- [ ] Optional: upload MP4 to release assets for higher quality

---

## README Embed

```markdown
<p align="center">
  <img src="docs/images/demo.gif" alt="CyberTool demo: scan, analyze, and review history" width="800" />
</p>
```

---

## What NOT to Show

- Real corporate hostnames or IP ranges
- Valid API keys or passwords
- Offensive module exploitation against live targets
- Error dialogs from failed builds
- Personal file paths in Explorer or logs

---

## Alternative: Silent Walkthrough

For Claude for Open Source or GitHub social preview, a **silent GIF** with on-screen UI labels (no audio) often performs better than narrated video. Let the workflow speak for itself.
