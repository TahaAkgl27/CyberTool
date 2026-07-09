using System;
using System.Text;
using System.Threading.Tasks;

namespace CyberTool.Services;

/// <summary>
/// Sistem sıkılaştırma ve otomatik düzeltme scriptleri üreten servis.
/// Enterprise seviyesinde çözüm önerileri sunar.
/// </summary>
public class RemediationService
{
    private readonly OpenAIService _aiService;

    public RemediationService()
    {
        _aiService = new OpenAIService();
    }

    public class RemediationResult
    {
        public string Script { get; set; } = string.Empty;
        public string Rollback { get; set; } = string.Empty;
        public ExplanationData Explanation { get; set; } = new();
    }

    public class ExplanationData
    {
        public string Description { get; set; } = string.Empty;
        public string RiskEx { get; set; } = string.Empty;
        public string Consequence { get; set; } = string.Empty;
        public string RealScenario { get; set; } = string.Empty;
        public string SideEffects { get; set; } = string.Empty;
        public double PriorityScore { get; set; }
        public string Rationale { get; set; } = string.Empty;
    }

    public async Task<RemediationResult> GenerateScriptAsync(string vulnerabilityId, string description)
    {
        var result = new RemediationResult();

        // 1. Try to generate via AI first
        if (_aiService.HasApiKey)
        {
            string prompt = !string.IsNullOrEmpty(description) ? description : vulnerabilityId;
            string rawResponse = await _aiService.GenerateSecurityScriptAsync(prompt);

            try
            {
                // Try JSON Parsing
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                result = System.Text.Json.JsonSerializer.Deserialize<RemediationResult>(rawResponse, options) ?? new RemediationResult();
            }
            catch
            {
                // Fallback for non-JSON response (Legacy)
                result.Script = rawResponse;
                result.Rollback = "# Rollback parsing failed.";
                result.Explanation.Description = "AI response was not in JSON format.";
            }

            return result;
        }

        // 2. Fallback to templates if no API Key
        var sb = new StringBuilder();
        sb.AppendLine("# ========================================================");
        sb.AppendLine("# CyberTool - Auto-Remediation Script (Offline Template)");
        sb.AppendLine($"# ID: {vulnerabilityId}");
        sb.AppendLine("# Note: Add OpenAI API Key for dynamic generation.");
        sb.AppendLine("# ========================================================");
        sb.AppendLine("");

        string fixScript = "";
        
        switch (vulnerabilityId.ToLower())
        {
            case "smb_hardening":
                sb.AppendLine("Write-Host 'Disabling SMBv1...'");
                sb.AppendLine("Set-ItemProperty -Path \"HKLM:\\SYSTEM\\CurrentControlSet\\Services\\LanmanServer\\Parameters\" -Name SMB1 -Value 0 -Force");
                fixScript = sb.ToString();
                
                result.Script = fixScript;
                result.Rollback = "Set-ItemProperty -Path \"HKLM:\\SYSTEM\\CurrentControlSet\\Services\\LanmanServer\\Parameters\" -Name SMB1 -Value 1 -Force";
                result.Explanation.Description = "SMBv1 protokolünü devre dışı bırakır.";
                result.Explanation.RiskEx = "WannaCry gibi fidye yazılımları bu protokolü kullanır.";
                result.Explanation.PriorityScore = 9.8;
                break;

            case "rdp_public":
                sb.AppendLine("Write-Host 'Blocking Public RDP...'");
                sb.AppendLine("New-NetFirewallRule -DisplayName \"Block Public RDP\" -Direction Inbound -LocalPort 3389 -Protocol TCP -Action Block -Profile Public");
                fixScript = sb.ToString();

                result.Script = fixScript;
                result.Rollback = "Remove-NetFirewallRule -DisplayName \"Block Public RDP\"";
                result.Explanation.Description = "RDP servisine internetten erişimi keser.";
                result.Explanation.RiskEx = "Brute-force saldırıları ile sunucu ele geçirilebilir.";
                result.Explanation.PriorityScore = 10.0;
                break;

            default:
                sb.AppendLine("# No offline template found. Please enable AI.");
                fixScript = sb.ToString();
                result.Script = fixScript;
                break;
        }

        return result;
    }
}
