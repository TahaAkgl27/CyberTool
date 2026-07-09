using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CyberTool.Core;

namespace CyberTool.ViewModels;

public sealed class RansomwareViewModel : ViewModelBase
{
    private string _subnet = "192.168.1";
    private bool _isScanning;
    private string _statusText = "Hazır";
    
    // 4 Key Metrics
    private int _readinessScore;
    private string _propagationRisk = "-";
    private string _recoveryDifficulty = "-";
    private string _patientZero = "-";

    // Progress
    private double _scanProgress;
    
    public ObservableCollection<VlanHostResult> HostResults { get; } = new();

    public string Subnet
    {
        get => _subnet;
        set => SetProperty(ref _subnet, value);
    }

    public bool IsScanning
    {
        get => _isScanning;
        set
        {
            if (SetProperty(ref _isScanning, value))
            {
                StartScanCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    public int ReadinessScore // 0-100 (100 = Bad or Good? Request says "Risk %", so 100 is Max Risk)
    {
        get => _readinessScore;
        set => SetProperty(ref _readinessScore, value);
    }

    public string PropagationRisk
    {
        get => _propagationRisk;
        set => SetProperty(ref _propagationRisk, value);
    }

    public string RecoveryDifficulty
    {
        get => _recoveryDifficulty;
        set => SetProperty(ref _recoveryDifficulty, value);
    }

    public string PatientZero
    {
        get => _patientZero;
        set => SetProperty(ref _patientZero, value);
    }

    public double ScanProgress
    {
        get => _scanProgress;
        set => SetProperty(ref _scanProgress, value);
    }

    public AsyncRelayCommand StartScanCommand { get; }

    public RansomwareViewModel()
    {
        StartScanCommand = new AsyncRelayCommand(ScanVlanAsync, () => !IsScanning);
    }

    private async Task ScanVlanAsync()
    {
        if (IsScanning) return;
        IsScanning = true;
        
        HostResults.Clear();
        ReadinessScore = 0;
        PropagationRisk = "-";
        RecoveryDifficulty = "-";
        PatientZero = "-";
        ScanProgress = 0;
        
        StatusText = "VLAN Taranıyor...";

        try
        {
            // Normalize Subnet Input
            // Expecting "192.168.1" or "192.168.1."
            var prefix = Subnet.TrimEnd('.');
            var parts = prefix.Split('.');
            if (parts.Length != 3)
            {
                StatusText = "Hatalı Subnet formatı. Örn: 192.168.1";
                IsScanning = false;
                return;
            }

            var tasks = new List<Task<VlanHostResult>>();
            using var semaphore = new SemaphoreSlim(50); // Control concurrency

            var dispatcherQueue = Microsoft.UI.Xaml.Application.Current is App app 
                ? App.MainWindow?.DispatcherQueue 
                : null;

            int totalHosts = 254;
            int completed = 0;

            for (int i = 1; i <= totalHosts; i++)
            {
                string ip = $"{prefix}.{i}";
                tasks.Add(Task.Run(async () => 
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        var result = await CheckHostAsync(ip);
                        
                        Interlocked.Increment(ref completed);
                        var progress = (double)completed / totalHosts * 100;
                        
                        dispatcherQueue?.TryEnqueue(() => 
                        {
                            ScanProgress = progress;
                            StatusText = $"Taranıyor: {ip}";
                            if (result.IsReachable) HostResults.Add(result);
                        });

                        return result;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            var results = await Task.WhenAll(tasks);
            var activeHosts = results.Where(r => r.IsReachable).ToList();

            CalculateMetrics(activeHosts);
            StatusText = $"Analiz Tamamlandı. {activeHosts.Count} aktif cihaz bulundu.";
        }
        catch (Exception ex)
        {
            StatusText = $"Hata: {ex.Message}";
        }
        finally
        {
            IsScanning = false;
        }
    }

    private async Task<VlanHostResult> CheckHostAsync(string ip)
    {
        var result = new VlanHostResult { IpAddress = ip };
        
        // Fast Connect for 445 (SMB) & 3389 (RDP)
        var t1 = IsPortOpen(ip, 445, 500);
        var t2 = IsPortOpen(ip, 3389, 500);
        var t3 = IsPortOpen(ip, 80, 500); // Web for context

        await Task.WhenAll(t1, t2, t3);

        result.HasSmb = t1.Result;
        result.HasRdp = t2.Result;
        result.HasWeb = t3.Result;
        
        // Assume reachable if any port open OR generic ping (skipped for speed, relying on ports)
        // Actually, if doing ransomware check, we only care if they are reachable via vulnerable ports.
        // But to be fair, we should check reachability. 
        // For this Demo: Reachable = HasSmb || HasRdp || HasWeb.
        
        result.IsReachable = result.HasSmb || result.HasRdp || result.HasWeb;
        
        if (result.IsReachable)
        {
             // Calculate Individual Risk Score
             int specificRisk = 0;
             if (result.HasSmb) specificRisk += 60;
             if (result.HasRdp) specificRisk += 40;
             
             // Bonus attributes (simulated)
             if (result.HasSmb && result.HasRdp) specificRisk += 20; // Combo risk
             
             result.RiskScore = Math.Min(100, specificRisk);
        }

        return result;
    }

    private async Task<bool> IsPortOpen(string ip, int port, int timeoutMs)
    {
        try
        {
            using var client = new TcpClient();
            var task = client.ConnectAsync(ip, port);
            if (await Task.WhenAny(task, Task.Delay(timeoutMs)) == task)
            {
                return client.Connected;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private void CalculateMetrics(List<VlanHostResult> activeHosts)
    {
        if (!activeHosts.Any())
        {
            ReadinessScore = 0;
            PropagationRisk = "Düşük";
            RecoveryDifficulty = "Düşük";
            PatientZero = "Yok";
            return;
        }

        // 1. Ransomware Risk % (Overall Network Vulnerability)
        // Logic: Average individual risk? Or Max risk? Usually aggregate.
        // Let's take (Total SMB Hosts + Total RDP Hosts) / Total Hosts * weighting
        double totalSmb = activeHosts.Count(h => h.HasSmb);
        double totalRdp = activeHosts.Count(h => h.HasRdp);
        
        // If 50% of network has SMB open, that's HUGE risk.
        // Formula: (SMB_Ratio * 0.7 + RDP_Ratio * 0.3) * 100 * SeverityMultiplier
        double smbRatio = totalSmb / activeHosts.Count;
        
        int score = (int)(smbRatio * 100); 
        if (totalRdp > 0) score += 20;
        
        ReadinessScore = Math.Min(100, Math.Max(10, score)); // Min 10 just for awareness

        // 2. Propagation Probability
        // Driven by SMB (Lateral Movement)
        if (smbRatio > 0.5) PropagationRisk = "Çok Yüksek (%90)";
        else if (smbRatio > 0.2) PropagationRisk = "Yüksek (%60)";
        else if (smbRatio > 0) PropagationRisk = "Orta (%30)";
        else PropagationRisk = "Düşük (%5)";

        // 3. Recovery Difficulty
        // Heuristic: If risk is high, recovery is hard (encryption spreads fast).
        // If Risk > 80, Very Hard.
        if (ReadinessScore > 80) RecoveryDifficulty = "Zor (Wormable)";
        else if (ReadinessScore > 50) RecoveryDifficulty = "Orta";
        else RecoveryDifficulty = "Kolay";

        // 4. Patient Zero
        // Host with max risk score
        var worst = activeHosts.OrderByDescending(h => h.RiskScore).FirstOrDefault();
        if (worst != null)
        {
            PatientZero = $"{worst.IpAddress} (Risk: {worst.RiskScore})";
        }
    }
}

public class VlanHostResult
{
    public string IpAddress { get; set; } = "";
    public bool IsReachable { get; set; }
    public bool HasSmb { get; set; }
    public bool HasRdp { get; set; }
    public bool HasWeb { get; set; }
    public int RiskScore { get; set; }
}
