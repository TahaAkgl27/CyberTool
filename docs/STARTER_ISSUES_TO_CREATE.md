# Starter Issues to Create on GitHub

Copy each block into a new GitHub Issue on https://github.com/TahaAkgl27/CyberTool/issues/new

Suggested labels: create them first if missing (`enhancement`, `security`, `documentation`, `good first issue`, `help wanted`).

---

## 1. Encrypt OpenAI API key using DPAPI

**Title:** Encrypt OpenAI API key using Windows DPAPI

**Labels:** `enhancement`, `security`

**Difficulty:** Medium

**Description:**  
Today the OpenAI API key is stored in plain text in `%AppData%\CyberTool\config.json`. Protect the key at rest using Windows DPAPI (`ProtectedData.Protect` / `Unprotect`) so it is not readable as cleartext on disk.

**Acceptance criteria:**
- [ ] Key is encrypted when saved from Settings
- [ ] Key decrypts correctly for OpenAI API calls
- [ ] Existing plain-text configs migrate safely on first load
- [ ] No key material written to logs
- [ ] Documented in `docs/usage.md` and CHANGELOG

---

## 2. Add Credential Manager support

**Title:** Store API key in Windows Credential Manager

**Labels:** `enhancement`, `security`

**Difficulty:** Medium

**Description:**  
As an alternative (or complement) to DPAPI file encryption, store the OpenAI API key in Windows Credential Manager so credentials follow OS-level secret storage practices.

**Acceptance criteria:**
- [ ] Save / clear from Settings updates Credential Manager
- [ ] App reads key from Credential Manager when present
- [ ] Fallback behavior documented if Credential Manager is unavailable
- [ ] Unit or manual test steps documented

---

## 3. Add export to PDF report

**Title:** Export executive report as PDF

**Labels:** `enhancement`, `help wanted`

**Difficulty:** Medium

**Description:**  
Users currently export text reports. Add PDF export for executive summaries suitable for stakeholders, with redaction-friendly layout.

**Acceptance criteria:**
- [ ] PDF generated from current scan session
- [ ] Includes risk score, summary, and top findings
- [ ] Output path configurable or clearly documented
- [ ] No secrets (API keys) embedded in PDF

---

## 4. Add JSON/CSV export

**Title:** Export scan findings as JSON and CSV

**Labels:** `enhancement`, `good first issue`

**Difficulty:** Easy

**Description:**  
Add machine-readable export of port findings for SIEM ingestion, spreadsheets, and lab reports.

**Acceptance criteria:**
- [ ] CSV export of findings table
- [ ] JSON export of session + findings
- [ ] Stable field names documented
- [ ] Works without OpenAI API key

---

## 5. Add unit tests for scan services

**Title:** Add unit tests for scan / risk classification logic

**Labels:** `enhancement`, `good first issue`

**Difficulty:** Medium

**Description:**  
Introduce a `CyberTool.Tests` project covering risk classification, port parsing, and result aggregation with mocked network I/O where needed.

**Acceptance criteria:**
- [ ] Test project builds in CI
- [ ] At least 5 meaningful tests for risk/port helpers
- [ ] No live network calls in unit tests
- [ ] Document how to run tests in CONTRIBUTING.md

---

## 6. Add safe lab mode toggle

**Title:** Add Lab Mode toggle restricting scan targets

**Labels:** `enhancement`, `security`

**Difficulty:** Medium

**Description:**  
Add a Settings toggle that restricts scan targets to localhost and private RFC1918 ranges (plus optional allowlist) to reduce accidental scanning of unauthorized hosts.

**Acceptance criteria:**
- [ ] Lab Mode toggle in Settings
- [ ] Scan start blocked for out-of-range targets with clear message
- [ ] Preference persisted locally
- [ ] Documented in usage and safety docs

---

## 7. Add Turkish localization

**Title:** Add resource-based Turkish / English localization

**Labels:** `enhancement`, `documentation`, `help wanted`

**Difficulty:** Medium

**Description:**  
UI strings are mixed Turkish/English. Introduce resource files so language can be switched cleanly and contributors can translate.

**Acceptance criteria:**
- [ ] Core navigation and Settings strings externalized
- [ ] Turkish and English resource sets
- [ ] Contributor guide for adding strings
- [ ] No behavior change to scan logic

---

## 8. Improve port risk scoring rules

**Title:** Improve port risk scoring rules and documentation

**Labels:** `enhancement`, `documentation`

**Difficulty:** Medium

**Description:**  
Review and refine risk tiers for common Windows/service ports. Document the scoring rationale so students and admins understand why a finding is Critical vs Medium.

**Acceptance criteria:**
- [ ] Scoring rules documented in `docs/`
- [ ] Edge cases (accepted risk, internal vs external) clarified
- [ ] Existing UI still shows severity consistently
- [ ] Examples use lab IPs only

---

## 9. Add plugin SDK foundation

**Title:** Add IScanPlugin foundation for extensibility

**Labels:** `enhancement`

**Difficulty:** Hard

**Description:**  
Design and implement a minimal plugin contract so community checks can be added without modifying core ViewModels. Align with architecture notes in `docs/architecture.md`.

**Acceptance criteria:**
- [ ] `IScanPlugin` (or equivalent) interface defined
- [ ] Sample plugin project under `samples/`
- [ ] Loader discovers plugins from a documented folder
- [ ] README / docs for authors

---

## 10. Add offline AI analysis roadmap

**Title:** Document and spike offline AI analysis option

**Labels:** `enhancement`, `documentation`

**Difficulty:** Hard (spike) / Easy (docs)

**Description:**  
Plan an offline AI path for air-gapped labs so analysis does not require OpenAI. Start with a design doc and optional spike; do not remove the existing optional OpenAI path.

**Acceptance criteria:**
- [ ] Design note in `docs/` covering options and privacy
- [ ] Clear non-goals for v1.x
- [ ] Roadmap entry updated
- [ ] No regression to current OpenAI opt-in flow

---

## After Creating Issues

- Pin 2–3 as `good first issue`
- Link issues to milestone `v1.1` / `v1.2` where appropriate
- Reference this file in Discussions if useful
