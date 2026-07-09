using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CyberTool.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CyberTool
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Auth check removed per user request
            // if (!Services.AppServices.AuthService.IsAuthenticated) { ... }

            ContentFrame.Navigate(typeof(DashboardPage));
            AppNav.SelectedItem = AppNav.MenuItems[0];
        }

        private void AppNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is not NavigationViewItem item)
            {
                return;
            }

            var tag = item.Tag?.ToString();
            switch (tag)
            {
                case "dashboard":
                    ContentFrame.Navigate(typeof(DashboardPage));
                    break;
                case "device":
                    ContentFrame.Navigate(typeof(DeviceProfilePage));
                    break;
                case "scan":
                    ContentFrame.Navigate(typeof(ScanPage));
                    break;
                case "reports":
                    ContentFrame.Navigate(typeof(ReportsPage));
                    break;
                case "attack":
                    ContentFrame.Navigate(typeof(AttackPage));
                    break;

                case "ransomware":
                    ContentFrame.Navigate(typeof(RansomwarePage));
                    break;
                case "settings":
                    ContentFrame.Navigate(typeof(SettingsPage));
                    break;
            }
        }
    }
}
