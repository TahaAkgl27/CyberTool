using System;
using System.Collections.ObjectModel;

namespace CyberTool.Models;

public sealed class ScanSession
{
    public Guid Id { get; } = Guid.NewGuid();

    public string Target { get; init; } = string.Empty;
    public string OsDescription { get; set; } = string.Empty;
    public string DeviceType { get; set; } = string.Empty;

    public DateTimeOffset StartedAt { get; init; } = DateTimeOffset.Now;

    public DateTimeOffset? FinishedAt { get; set; }

    // Remote Hardware / System Info
    public string Hostname { get; set; } = string.Empty;
    public double? TotalRamGb { get; set; }
    public string CpuModel { get; set; } = string.Empty;
    public string BiosVersion { get; set; } = string.Empty;
    public string Uptime { get; set; } = string.Empty;   

    // Expanded Enumeration
    public string Domain { get; set; } = string.Empty;
    public string SmbSigningStatus { get; set; } = string.Empty;
    public string WebServerType { get; set; } = string.Empty;
    public string DatabaseInfo { get; set; } = string.Empty;
    public string RemoteManagementInfo { get; set; } = string.Empty;
    public bool IsFtpAnonymous { get; set; } = false;

    public ObservableCollection<PortFinding> Findings { get; set; } = new();
}
