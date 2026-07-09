using Microsoft.UI.Xaml;
using CyberTool.Services;
using System;

namespace CyberTool.Views
{
    public sealed partial class LoginWindow : Window
    {
        private readonly AuthService _authService;

        public LoginWindow(AuthService authService)
        {
            this.InitializeComponent();
            _authService = authService;
            
            this.Title = "CyberTool - Giriş Yap";

            // Resize the window to 400x450
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new Windows.Graphics.SizeInt32(400, 450));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorBar.IsOpen = false;
            
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorBar.Message = "Lütfen tüm alanları doldurunuz.";
                ErrorBar.IsOpen = true;
                return;
            }

            bool success = _authService.Login(username, password);

            if (!success)
            {
                ErrorBar.Message = "Kullanıcı adı veya şifre hatalı.";
                ErrorBar.IsOpen = true;
            }
            // Logic for success is handled in App.xaml.cs via event subscription or direct call there? 
            // Better pattern: App.xaml.cs controls the flow. 
            // BUT, for simplicity in WinUI, we might need to signal readiness here.
            // Actually, the App.xaml.cs Plan was to subscribe to the event. 
            // So we don't need to do anything here on success other than let the event fire.
        }
    }
}
