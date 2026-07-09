using System.Collections.Generic;

namespace CyberTool.Models;

public class ComplianceStandard
{
    public string StandardName { get; set; } = string.Empty; // e.g., "KVKK Uyumu"
    public bool IsCompliant { get; set; }
    public string StatusText => IsCompliant ? "Uyumlu" : "Risk Altında";
    public string StatusColor => IsCompliant ? "#2ECC71" : "#E74C3C"; // Green / Red
    public string Icon => IsCompliant ? "\uE73E" : "\uE711"; // Check / Cancel
    public List<string> ViolationReasons { get; set; } = new();
}
