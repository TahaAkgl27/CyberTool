using Microsoft.UI.Xaml.Controls;
using CyberTool.Services;
using CyberTool.ViewModels;

namespace CyberTool.Views
{
    public sealed partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();

            DataContext = new ReportsViewModel(AppServices.ScanStore);
        }
    }
}
