using System;
using System.IO;
using System.Text.Json;

namespace CyberTool.Services
{
    public class ConfigService
    {
        private readonly string _configPath;
        private AppConfig _config;

        public ConfigService()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CyberTool");
            Directory.CreateDirectory(folder);
            _configPath = Path.Combine(folder, "config.json");
            Load();
        }

        public string? OpenAIApiKey
        {
            get => _config.OpenAIApiKey;
            set
            {
                _config.OpenAIApiKey = value;
                Save();
            }
        }

        private void Load()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    var json = File.ReadAllText(_configPath);
                    _config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                }
                else
                {
                    _config = new AppConfig();
                }
            }
            catch
            {
                _config = new AppConfig();
            }
        }

        private void Save()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_config, options);
                File.WriteAllText(_configPath, json);
            }
            catch { }
        }

        private class AppConfig
        {
            public string? OpenAIApiKey { get; set; }
        }
    }
}
