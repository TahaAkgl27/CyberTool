using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CyberTool.Models;

namespace CyberTool.Services;

public class HistoryService
{
    private readonly string _historyPath;

    public HistoryService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var folder = Path.Combine(appData, "CyberTool", "History");
        Directory.CreateDirectory(folder);
        _historyPath = Path.Combine(folder, "scan_history.json");
    }

    public async Task SaveScanAsync(ScanSession session)
    {
        var history = await LoadHistoryAsync();
        
        // Remove old entry for same target if we want only latest, 
        // OR keep log. For this feature ("Days Open"), we need the FIRST time a port was seen.
        // We will append the new scan.
        
        var record = new ScanRecord
        {
            Target = session.Target,
            Date = DateTime.Now,
            Findings = session.Findings.ToList()
        };

        history.Add(record);
        
        // Optimize: Keep only last 50 scans to avoid bloat
        if (history.Count > 50) history = history.OrderByDescending(x => x.Date).Take(50).ToList();

        var json = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_historyPath, json);
    }

    public async Task<List<ScanRecord>> LoadHistoryAsync()
    {
        if (!File.Exists(_historyPath)) return new List<ScanRecord>();
        
        try
        {
            var json = await File.ReadAllTextAsync(_historyPath);
            return JsonSerializer.Deserialize<List<ScanRecord>>(json) ?? new List<ScanRecord>();
        }
        catch
        {
            return new List<ScanRecord>();
        }
    }

    public async Task<int?> GetDaysOpenAsync(string target, int port)
    {
        var history = await LoadHistoryAsync();
        var targetScans = history
            .Where(h => h.Target == target && h.Findings.Any(f => f.Port == port))
            .OrderBy(h => h.Date)
            .ToList();

        if (!targetScans.Any()) return 0; // New risk

        var firstSeen = targetScans.First().Date;
        return (DateTime.Now - firstSeen).Days;
    }
}

public class ScanRecord
{
    public string Target { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<PortFinding> Findings { get; set; } = new();
}
