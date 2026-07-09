using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using CyberTool.ViewModels;

namespace CyberTool.Views
{
    public sealed partial class DeviceProfilePage : Page
    {
        public DeviceViewModel ViewModel { get; } = new();

        public DeviceProfilePage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.RefreshData();
        }
    }
}
