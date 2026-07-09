using System.Collections.ObjectModel;
using CyberTool.Models;
using CyberTool.Services;

namespace CyberTool.ViewModels;

public sealed class ReportsViewModel : ViewModelBase
{
    private readonly ScanStore _store;
    private ScanSession? _selectedSession;

    public ReportsViewModel(ScanStore store)
    {
        _store = store;
        Sessions = _store.Sessions;
    }

    public ObservableCollection<ScanSession> Sessions { get; }

    public ScanSession? SelectedSession
    {
        get => _selectedSession;
        set => SetProperty(ref _selectedSession, value);
    }
}
