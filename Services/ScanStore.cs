using System.Collections.ObjectModel;
using System.IO;
using CyberTool.Models;

namespace CyberTool.Services;

public sealed class ScanStore
{
    private readonly string _filePath;

    public ObservableCollection<ScanSession> Sessions { get; } = new();

    public ScanStore()
    {
        var folder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "CyberTool");
        try
        {
            System.IO.Directory.CreateDirectory(folder);
        }
        catch { }
        
        _filePath = Path.Combine(folder, "history.json");
        Load();
    }

    public void Add(ScanSession session)
    {
        Sessions.Insert(0, session);
        Save();
    }

    public void Save()
    {
        try
        {
            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            var json = System.Text.Json.JsonSerializer.Serialize(Sessions, options);
            System.IO.File.WriteAllText(_filePath, json);
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving history: {ex.Message}");
        }
    }

    private void Load()
    {
        try
        {
            if (System.IO.File.Exists(_filePath))
            {
                var json = System.IO.File.ReadAllText(_filePath);
                var sessions = System.Text.Json.JsonSerializer.Deserialize<System.Collections.ObjectModel.ObservableCollection<ScanSession>>(json);
                if (sessions != null)
                {
                    Sessions.Clear();
                    foreach (var s in sessions)
                    {
                        Sessions.Add(s);
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading history: {ex.Message}");
        }
    }
}
