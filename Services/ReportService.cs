using System;
using System.Text;
using System.Linq;
using CyberTool.Models;
using System.Collections.Generic;

namespace CyberTool.Services;

public class ReportService
{
    public string GenerateExecutiveReport(ScanSession session)
    {
        var sb = new StringBuilder();
        sb.AppendLine("CONFIDENTIAL - CYBER EXECUTIVE REPORT");
        sb.AppendLine($"Date: {DateTime.Now:dd MMM yyyy HH:mm}");
        sb.AppendLine($"Target: {session.Target}");
        sb.AppendLine(new string('=', 50));
        sb.AppendLine();

        sb.AppendLine("1. EXECUTIVE SUMMARY");
        sb.AppendLine("--------------------------------------------------");
        
        // Calculate Risk Score (Simplified logic from ScanViewModel)
        var findings = session.Findings;
        int riskScore = 100;
        int external = findings.Count(f => f.AccessScope?.Contains("İnternet") ?? false);
        int critical = findings.Count(f => f.Risk?.Contains("Kritik") == true);
        
        riskScore -= (external * 5) + (critical * 10);
        if(riskScore < 0) riskScore = 0;

        sb.AppendLine($"Overall Security Score: {riskScore}/100");
        sb.AppendLine($"Risk Status: {(riskScore < 50 ? "CRITICAL RISK" : riskScore < 80 ? "HIGH RISK" : "ACCEPTABLE")}");
        sb.AppendLine();

        sb.AppendLine("2. BUSINESS IMPACT");
        sb.AppendLine("--------------------------------------------------");
        if (findings.Any(f => f.Port == 3389 || f.Port == 445))
        {
            sb.AppendLine("⚠️ IMMEDIATE THREAT: Ransomware & Data Loss");
            sb.AppendLine("Potential Financial Impact: High (Estimated >$50k)");
            sb.AppendLine("Operational Risk: Business Interruption > 48 Hours");
        }
        else if (findings.Any(f => f.Port == 80 && !findings.Any(x => x.Port == 443)))
        {
             sb.AppendLine("⚠️ COMPLIANCE RISK: GDPR/KVKK Violation");
             sb.AppendLine("Reason: Unencrypted data transfer detected.");
        }
        else
        {
             sb.AppendLine("✅ System appears stable. Regular maintenance recommended.");
        }
        sb.AppendLine();

        sb.AppendLine("3. STRATEGIC RECOMMENDATIONS");
        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine("1. [Immediate] Close public RDP/SMB ports if detected.");
        sb.AppendLine("2. [Week 1] Implement SSL/TLS for all web services.");
        sb.AppendLine("3. [Month 1] Schedule penetration test.");

        return sb.ToString();
    }

    public string GenerateTechnicalReport(ScanSession session)
    {
        var sb = new StringBuilder();
        sb.AppendLine("TECHNICAL VULNERABILITY ASSESSMENT REPORT");
        sb.AppendLine($"Target: {session.Target} | Scan Date: {session.StartedAt}");
        sb.AppendLine(new string('-', 80));
        sb.AppendLine();

        sb.AppendLine("[PORT SCAN RESULTS]");
        sb.AppendLine(string.Format("{0,-10} {1,-10} {2,-20} {3,-15}", "PORT", "STATE", "SERVICE", "VERSION"));
        foreach(var f in session.Findings.OrderBy(x => x.Port))
        {
             sb.AppendLine(string.Format("{0,-10} {1,-10} {2,-20} {3,-15}", f.Port, f.State, f.Service, "Detected"));
        }
        sb.AppendLine();

        sb.AppendLine("[VULNERABILITY ANALYSIS]");
        foreach(var f in session.Findings.Where(x => x.Risk != "Info" && x.Risk != "Düşük"))
        {
            sb.AppendLine($"🔴 PORT {f.Port} ({f.Service}) - {f.Risk}");
            sb.AppendLine($"   Scope: {f.AccessScope}");
            sb.AppendLine($"   Issue: {f.RiskReason}");
            sb.AppendLine($"   Fix:   {f.Recommendation}");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
