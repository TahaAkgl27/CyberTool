using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CyberTool.Services;

namespace CyberTool.Views
{
    public sealed partial class SettingsPage : Page
    {
        private readonly ConfigService _configService = new();

        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var existingKey = _configService.OpenAIApiKey;
            if (!string.IsNullOrEmpty(existingKey))
            {
                ApiKeyBox.Password = existingKey;
                ShowStatus("API anahtarı yüklendi.", InfoBarSeverity.Informational);
            }
            else
            {
                ShowStatus("API anahtarı tanımlı değil. Ayarlardan girin.", InfoBarSeverity.Warning);
            }
        }

        private void SaveApiKeyButton_Click(object sender, RoutedEventArgs e)
        {
            var key = ApiKeyBox.Password?.Trim();
            if (string.IsNullOrEmpty(key))
            {
                ShowStatus("Lütfen geçerli bir API anahtarı girin.", InfoBarSeverity.Error);
                return;
            }

            if (!key.StartsWith("sk-", System.StringComparison.OrdinalIgnoreCase))
            {
                ShowStatus("API anahtarı 'sk-' ile başlamalıdır.", InfoBarSeverity.Warning);
                return;
            }

            _configService.OpenAIApiKey = key;
            ShowStatus("API anahtarı kaydedildi.", InfoBarSeverity.Success);
        }

        private void ClearApiKeyButton_Click(object sender, RoutedEventArgs e)
        {
            _configService.OpenAIApiKey = null;
            ApiKeyBox.Password = string.Empty;
            ShowStatus("API anahtarı temizlendi.", InfoBarSeverity.Informational);
        }

        private void ShowStatus(string message, InfoBarSeverity severity)
        {
            ApiKeyStatusBar.Message = message;
            ApiKeyStatusBar.Severity = severity;
            ApiKeyStatusBar.IsOpen = true;
        }
    }
}
