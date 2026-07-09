using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CyberTool.Models;

public sealed class PortFinding : INotifyPropertyChanged
{
    private string? _risk;
    private string? _riskReason;
    private string? _changeType;
    private string? _recommendation;
    private string? _shortRecommendation;
    private string? _service;
    private string? _accessScope;

    public string Target { get; init; } = string.Empty;

    public int Port { get; init; }

    private string _state = string.Empty;
    public string State
    {
        get => _state;
        set { _state = value; OnPropertyChanged(); }
    }

    private string _protocol = string.Empty;
    public string Protocol
    {
        get => _protocol;
        set { _protocol = value; OnPropertyChanged(); }
    }
    public string? Service
    {
        get => _service;
        set { _service = value; OnPropertyChanged(); }
    }

    public string? Product { get; set; }
    public string? Version { get; set; }
    public string? ExtraInfo { get; set; }

    public string? Risk
    {
        get => _risk;
        set { _risk = value; OnPropertyChanged(); }
    }
    
    public string? RiskReason
    {
        get => _riskReason;
        set { _riskReason = value; OnPropertyChanged(); }
    }

    public string? AccessScope
    {
        get => _accessScope;
        set { _accessScope = value; OnPropertyChanged(); }
    }

    public DateTimeOffset FirstSeen { get; set; } = DateTimeOffset.Now;

    private string? _ageDescription;
    public string? AgeDescription
    {
        get => _ageDescription;
        set { _ageDescription = value; OnPropertyChanged(); }
    }

    public string? ChangeType
    {
        get => _changeType;
        set { _changeType = value; OnPropertyChanged(); }
    }

    private string? _openReason;
    public string? OpenReason
    {
        get => _openReason;
        set { _openReason = value; OnPropertyChanged(); }
    }

    private string? _responsibleTeam;
    public string? ResponsibleTeam
    {
        get => _responsibleTeam;
        set { _responsibleTeam = value; OnPropertyChanged(); }
    }

    private bool _isRiskAccepted;
    public bool IsRiskAccepted
    {
        get => _isRiskAccepted;
        set 
        { 
            if (_isRiskAccepted != value)
            {
                _isRiskAccepted = value; 
                OnPropertyChanged(); 
            }
        }
    }

    private bool _isVerified;
    public bool IsVerified // True if confirmed by banner/exploit
    {
        get => _isVerified;
        set { _isVerified = value; OnPropertyChanged(); }
    }

    private bool _bannerVerified;
    public bool BannerVerified // ✅ Banner confirmed
    {
        get => _bannerVerified;
        set { _bannerVerified = value; OnPropertyChanged(); }
    }
    
    private bool _versionMissing;
    public bool VersionMissing // ⚠️ Version info missing
    {
        get => _versionMissing;
        set { _versionMissing = value; OnPropertyChanged(); }
    }

    public string? ShortRecommendation
    {
        get => _shortRecommendation;
        set { _shortRecommendation = value; OnPropertyChanged(); }
    }

    public string? Recommendation
    {
        get => _recommendation;
        set { _recommendation = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
