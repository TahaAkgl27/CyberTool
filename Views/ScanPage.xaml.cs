using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CyberTool.Services;
using CyberTool.ViewModels;
using Windows.Storage.Pickers;

namespace CyberTool.Views
{
    public sealed partial class ScanPage : Page
    {
        public ScanViewModel ViewModel { get; }

        public ScanPage()
        {
            InitializeComponent();

            ViewModel = new ScanViewModel(AppServices.ScanStore);
            DataContext = ViewModel;
        }

        private async void ImportNmapXml_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".xml");

            if (App.MainWindow is null)
            {
                return;
            }

            PickerHelper.InitializeWithWindow(picker, App.MainWindow);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }

            var session = NmapXmlImporter.Import(file.Path);
            AppServices.ScanStore.Add(session);
            ViewModel.LoadSession(session);
        }
    }
}
