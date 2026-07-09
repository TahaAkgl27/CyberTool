# Safety Guide

CyberTool includes capabilities that can resemble offensive security tooling. This guide explains how to use the project responsibly.

## Authorized Environments Only

Use CyberTool only when you have **explicit permission** to assess the target:

| Environment | Typical Use |
|-------------|-------------|
| Your own workstation | Local diagnostics and learning |
| Corporate assets | With IT/security team approval |
| Lab VMs | Classroom, certification, or R&D |
| Penetration test scope | With signed rules of engagement |

**Never** scan or test third-party systems, ISP networks, or public infrastructure without authorization.

## Lab Usage

For learning and demos:

- Use isolated lab networks (e.g. `192.168.100.0/24`)
- Prefer virtual machines with snapshots
- Avoid connecting lab exercises to production Active Directory
- Use generic hostnames (`LAB-PC01`, `EXAMPLE.LOCAL`)

CyberTool includes demo data for sample lab IPs to support training without exposing real infrastructure.

## Red Team / Blue Team Education

CyberTool modules can support:

- **Blue team:** Identify exposed services, weak configurations, and attack paths
- **Red team (authorized):** Validate credential policies and lateral movement assumptions
- **Awareness:** Ransomware propagation simulation to demonstrate blast radius in a controlled subnet

These features simulate attacker behavior **for defense validation and training**, not for unauthorized access.

## Data Privacy

### Local Data

Scan results, history, and configuration are stored **locally** on the machine running CyberTool. No telemetry or cloud sync is built into the core application.

### OpenAI Data Warning

When you provide an OpenAI API key and enable AI features:

- Port scan summaries and vulnerability context may be sent to **OpenAI's API**
- Data handling is governed by **your OpenAI account terms**
- Do not send classified, regulated, or highly sensitive production data without organizational approval

Disable AI features or use organizational API policies if data residency is a concern.

### Reports

Exported reports may contain target IPs, port states, and risk narratives. Handle reports according to your organization's data classification policy.

## Module-Specific Guidance

| Module | Safety Note |
|--------|-------------|
| Network Scan | May trigger IDS/IPS alerts; notify SOC if testing in monitored environments |
| WMI Enumeration | Requires appropriate credentials and network access |
| Attack Helpers | Authorized credential testing only; rate-limit and scope carefully |
| Ransomware Simulation | Subnet scan for awareness; do not run against production without approval |
| Remediation Scripts | Review AI-generated PowerShell before execution |

## Incident Response

If CyberTool activity causes unexpected alerts:

1. Stop the active operation
2. Notify your security team with scope and timestamps
3. Document authorization and test objectives

## Reporting Misuse

If you discover vulnerabilities in CyberTool itself, report via [GitHub Security Advisories](../SECURITY.md)—not public issues.

## Legal Reminder

You are responsible for compliance with applicable laws and contracts. See [DISCLAIMER.md](../DISCLAIMER.md).
