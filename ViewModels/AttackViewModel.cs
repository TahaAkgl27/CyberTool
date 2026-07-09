using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using CyberTool.Services;
using CyberTool.Core;

using Microsoft.UI.Dispatching;

namespace CyberTool.ViewModels;

public class AttackViewModel : ObservableObject
{
    private readonly AttackService _attackService;
    private StringBuilder _logBuffer = new StringBuilder();
    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    public AttackViewModel()
    {
        _attackService = new AttackService();
        _attackService.OnLog += Log; // Subscribe to service logs
        
        IsRunning = false;
        TargetIp = "192.168.100.10"; // Default demo sample
    }

    private void Log(string msg)
    {
        _dispatcherQueue.TryEnqueue(() => 
        {
            _logBuffer.AppendLine(msg);
            OnPropertyChanged(nameof(ConsoleOutput));
        });
    }

    private string _targetIp;
    public string TargetIp { get => _targetIp; set => SetProperty(ref _targetIp, value); }
    
    private string _targetName;
    public string TargetName { get => _targetName; set => SetProperty(ref _targetName, value); }

    private string _targetSurname;
    public string TargetSurname { get => _targetSurname; set => SetProperty(ref _targetSurname, value); }
    
    private bool _isRunning;
    public bool IsRunning { get => _isRunning; set => SetProperty(ref _isRunning, value); }
    
    public string ConsoleOutput => _logBuffer.ToString();

    // Loot Properties
    private string _crackedCredentials; 
    public string CrackedCredentials { get => _crackedCredentials; set => SetProperty(ref _crackedCredentials, value); }

    private CancellationTokenSource _cts;

    public RelayCommand StopAttackCommand => new(() => 
    {
        if (_cts != null && !_cts.IsCancellationRequested)
        {
            _cts.Cancel();
            Log("[!] Stopping attack...");
        }
    });

    public RelayCommand StartAttackCommand => new(async () => 
    {
        if (IsRunning) return;
        
        IsRunning = true;
        _logBuffer.Clear();
        CrackedCredentials = "Bilinmiyor";
        _cts = new CancellationTokenSource();
        
        try
        {
            var result = await _attackService.BruteForceSmbAsync(TargetIp, TargetName, TargetSurname, _cts.Token);
        
            if (result.success)
            {
                 CrackedCredentials = $"{result.user}:{result.pass}";
                 
                 // Auto-Loot Trigger
                 Log($"[+] Triggering Auto-Enumerate (Looting) with {result.user}...");
                 try 
                 {
                     var scanService = new SystemEnumerationService();
                     var loot = await scanService.EnumerateAuthAsync(TargetIp, result.user, result.pass);
                     Log($"[+] Loot Secured: {loot.Hostname}, {loot.OsDescription}");
                     Log($"[+] {loot.Hotfixes.Count} Hotfixes, {loot.InstalledSoftware.Count} Apps found.");
                 }
                 catch (Exception ex)
                 {
                     Log($"[!] Looting failed: {ex.Message}");
                 }
            }
            else
            {
                 CrackedCredentials = "Bulunamadı";
            }
        }
        catch (OperationCanceledException)
        {
            Log("[!] Operation cancelled by user.");
        }
        finally
        {
            IsRunning = false;
            if (_cts != null) { _cts.Dispose(); _cts = null; }
        }
    });
}
