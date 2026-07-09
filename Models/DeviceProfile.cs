using System;
using System.Collections.Generic;

namespace CyberTool.Models;

public class DeviceProfile
{
    public string DeviceName { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Status { get; set; } = "🟢 Sağlam";
    public int PostureScore { get; set; } = 100;
    public DateTime LastSeen { get; set; } = DateTime.Now;

    // KPI: Patch
    public int DaysSinceLastUpdate { get; set; }
    public string OsBuild { get; set; } = string.Empty;
    public string PatchStatus => DaysSinceLastUpdate > 30 ? "🟠 Kritik Güncelleme Eksik" : "🟢 Güncel";

    // KPI: Accounts
    public int LocalAccountCount { get; set; }
    public int SuspiciousAdminCount { get; set; }
    public List<LocalUserAccount> Users { get; set; } = new();

    // KPI: Security
    public bool IsDefenderActive { get; set; }
    public bool IsFirewallActive { get; set; }
    public bool IsBitLockerActive { get; set; }
    public bool IsTamperProtectionActive { get; set; }

    // KPI: Resources
    public double TotalRamGb { get; set; }
    public string CpuModel { get; set; } = string.Empty;
    public double DiskUsagePercentage { get; set; }
    public string DiskStatus => DiskUsagePercentage > 90 ? "🔴 Kritik Doluluk" : "🟢 Normal";

    // AI Summary
    public string ExecutiveSummary { get; set; } = string.Empty;
    public string TopRiskFactors { get; set; } = string.Empty;
    public string QuickWinAction { get; set; } = string.Empty;
    public string BusinessImpact { get; set; } = string.Empty;
    
    // Detailed System Info
    public string WindowsVersion { get; set; } = string.Empty;
    public string OsEdition { get; set; } = string.Empty;
    public string Uptime { get; set; } = string.Empty;
    public string InstallDate { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string BootTime { get; set; } = string.Empty;
    public int ProcessorCount { get; set; }

    // Remote Specific
    public bool IsRemoteTarget { get; set; }
    public List<PortFinding> Findings { get; set; } = new();
    public string OsConfidence { get; set; } = string.Empty;

    // Deep Discovery Details
    public List<string> InstalledSoftwares { get; set; } = new();
    public List<string> Hotfixes { get; set; } = new();
    public List<string> NetworkInterfaces { get; set; } = new();
    public string RawCommandOutput { get; set; } = string.Empty;
}

public class LocalUserAccount
{
    public string Username { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime? LastLogin { get; set; }
    public int PasswordAgeDays { get; set; }
    public double SuspicionScore { get; set; }
    public string SuspicionReason { get; set; } = string.Empty;
}

public class RoadmapItem
{
    public string Phase { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public string Effort { get; set; } = "S"; // S, M, L
    public int ImpactScore { get; set; } // 1-10
}
