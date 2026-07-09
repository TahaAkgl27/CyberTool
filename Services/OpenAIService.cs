using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CyberTool.Models;

namespace CyberTool.Services
{
    public class OpenAIService
    {
        private readonly ConfigService _configService;
        private readonly HttpClient _httpClient;

        public OpenAIService()
        {
            _configService = new ConfigService();
            _httpClient = new HttpClient();
        }

        public void SaveApiKey(string key)
        {
            _configService.OpenAIApiKey = key;
        }

        public bool HasApiKey => !string.IsNullOrEmpty(_configService.OpenAIApiKey);

        /// <summary>
        /// Generates a succinct security analysis using optimized prompts to save tokens.
        /// </summary>
        public async Task<List<AttackScenario>> AnalyzeScanResultsAsync(string target, IEnumerable<PortFinding> findings)
        {
            var apiKey = _configService.OpenAIApiKey;
            if (string.IsNullOrEmpty(apiKey)) return new List<AttackScenario>();

            // 1. Optimize Input (Token Saving)
            // Instead of sending full objects, send a minified CSV-like string: "Port:Service:State"
            var riskyPorts = findings.Where(f => f.State == "open" && f.IsVerified).OrderBy(f => f.Port).ToList();
            
            if (!riskyPorts.Any()) return new List<AttackScenario>();

            var sb = new StringBuilder();
            sb.AppendLine($"Target: {target}");
            sb.AppendLine("Open Ports (Verified):");
            foreach (var f in riskyPorts)
            {
                sb.AppendLine($"- {f.Port}/{f.Service} ({f.AccessScope})");
            }

            // 2. Optimized System Prompt (Turkish)
            // Instructing AI to be concise and return ONLY JSON in Turkish
            var systemPrompt = "Sen uzman bir Siber Güvenlik Analistisin. Açık portları analiz et ve en kritik 3-5 Saldırı Senaryosunu TÜRKÇE olarak JSON formatında döndür. " +
                               "JSON Formatı: [{ Title, Severity (Critical/High/Medium/Low), Category (Web/Database/Network/System), Description, Impact (Bu neye sebep olur?), AttackerMindset (Saldırgan ne düşünür?), Rationale (Neden kritik?) }]. " +
                               "Severity değerleri İngilizce kalsın (Critical, High vb.), diğer TÜM metinler Türkçe olsun. " +
                               "Category değerleri şunlardan biri olsun: 'Entry Point', 'Lateral Movement', 'Privilege Escalation', 'Data Exfiltration', 'Impact'. " +
                               "Rationale alanı şu formatta olsun: '🧠 Bu risk kritik çünkü: [Neden 1] - [Neden 2] - [Neden 3]'. " +
                               "Giriş ve çıkış cümlesi kurma, sadece saf JSON döndür.";

            var userPrompt = sb.ToString();

            // 3. Call API (Using gpt-3.5-turbo for cost efficiency, or gpt-4o-mini if available in your plan)
            try
            {
                var requestBody = new
                {
                    model = "gpt-3.5-turbo", // Cost-effective model
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userPrompt }
                    },
                    temperature = 0.3, // Low temperature for deterministic/focused results
                    max_tokens = 2000   // Limit output tokens
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
                request.Headers.Add("Authorization", $"Bearer {apiKey}");
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var responseJson = JsonSerializer.Deserialize<JsonElement>(responseString);
                var content = responseJson.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                // Clean Markdown code blocks if present
                if (content.StartsWith("```json"))
                {
                    content = content.Replace("```json", "").Replace("```", "").Trim();
                }
                else if (content.StartsWith("```"))
                {
                    content = content.Replace("```", "").Trim();
                }

                return JsonSerializer.Deserialize<List<AttackScenario>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AttackScenario>();
            }
            catch (Exception ex)
            {
                // Fallback or Log
                System.Diagnostics.Debug.WriteLine($"AI Error: {ex.Message}");
                return new List<AttackScenario> 
                { 
                    new AttackScenario 
                    { 
                        Title = "Yapay Zeka Analizi Başarısız", 
                        Description = $"Analiz tamamlanamadı: {ex.Message}. Lütfen API Anahtarınızı kontrol edin.", 
                        Severity = "Info" 
                    } 
                };
            }
        }


        public async Task<string> GenerateSecurityScriptAsync(string vulnerabilityName, string platform = "Windows PowerShell")
        {
            var apiKey = _configService.OpenAIApiKey;
            if (string.IsNullOrEmpty(apiKey)) return "# API Key eksik. Lütfen ayarlardan OpenAI anahtarını girin.";

            var systemPrompt = "Sen kıdemli bir Sistem Güvenlik Mühendisisin (Level 3). Görevin, tespit edilen zafiyetleri kapatmak için 'Enterprise Grade' çözümler üretmektir.\n" +
                               "Çıktı FORMATI (Saf JSON): \n" +
                               "{ \n" +
                               "  \"script\": \"...PowerShell fix kodu...\", \n" +
                               "  \"rollback\": \"...PowerShell geri alma kodu...\", \n" +
                               "  \"explanation\": { \n" +
                               "    \"description\": \"...Bu ayar nedir?...\", \n" +
                               "    \"riskEx\": \"...Neden riskli?...\", \n" +
                               "    \"consequence\": \"...Yapılmazsa ne olur?...\", \n" +
                               "    \"realScenario\": \"...Gerçek saldırı örneği (örn: EternalBlue)...\", \n" +
                               "    \"sideEffects\": \"...Performans/Erişim etkisi...\", \n" +
                               "    \"priorityScore\": 9.5, \n" +
                               "    \"rationale\": \"...Neden bu puanı verdim?...\" \n" +
                               "  } \n" +
                               "} \n" +
                               "Kurallar:\n" +
                               "1. Context-Aware ol (OS, Rol kontrolü ekle).\n" +
                               "2. Yorumları TÜRKÇE yaz.\n" +
                               "3. Sadece ve sadece JSON döndür. Markdown '```json' bloğu kullanma.";

            var userPrompt = $"Aşağıdaki zafiyet için {platform} çözüm paketi üret:\n" +
                             $"Zafiyet: {vulnerabilityName}\n" +
                             $"Hedef: Bu zafiyeti açıkla ve kapat.\n" +
                             "Çıktı sadece JSON olsun.";

            try
            {
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userPrompt }
                    },
                    temperature = 0.2, // Low creativity, high precision
                    max_tokens = 1000
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
                request.Headers.Add("Authorization", $"Bearer {apiKey}");
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var responseJson = JsonSerializer.Deserialize<JsonElement>(responseString);
                var content = responseJson.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                // Clean Markdown
                if (content.StartsWith("```powershell")) content = content.Replace("```powershell", "").Replace("```", "");
                if (content.StartsWith("```bash")) content = content.Replace("```bash", "").Replace("```", "");
                if (content.StartsWith("```")) content = content.Replace("```", "");

                return content.Trim();
            }
            catch (Exception ex)
            {
                return $"# Hata: Script üretilemedi. {ex.Message}";
            }
        }
    }
}
