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
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CyberTool
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;
        private Window? _loginWindow;

        public static Window? MainWindow { get; private set; }

        private static string ErrorLogPath
        {
            get
            {
                var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CyberTool");
                Directory.CreateDirectory(folder);
                return System.IO.Path.Combine(folder, "errors.log");
            }
        }

        public App()
        {
            this.UnhandledException += App_UnhandledException;
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                File.AppendAllText(ErrorLogPath, $"[CRITICAL] InitializeComponent Failed: {ex}\n");
                throw;
            }
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            File.AppendAllText(ErrorLogPath, $"[CRITICAL] Unhandled Exception: {e.Exception}\n");
            e.Handled = false; // Let it crash so we can see it, or close
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            try
            {
                _window = new MainWindow();
                MainWindow = _window;
                _window.Activate();
            }
            catch (Exception ex)
            {
                File.AppendAllText(ErrorLogPath, $"[CRITICAL] OnLaunched Failed: {ex}\n");
                throw;
            }
        }
    }
}
