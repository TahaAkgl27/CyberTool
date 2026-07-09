namespace CyberTool.Models;

public class HardeningSuggestion
{
    public string ActionName { get; set; } = string.Empty; // e.g., "SMB İmzalamayı Aç"
    public string AffectedService { get; set; } = string.Empty; // e.g., "Dosya Paylaşımı (445)"
    public int CurrentRiskScore { get; set; }
    public int ProjectedRiskScore { get; set; }
    public int RiskReduction => ProjectedRiskScore > CurrentRiskScore ? ProjectedRiskScore - CurrentRiskScore : CurrentRiskScore - ProjectedRiskScore; // Handle both schemas, but prefer Gain.
    public string EstimatedEffort { get; set; } = "15 dk"; // e.g., "15 dk"
    public string ImpactLevel { get; set; } = "High"; // High, Medium, Low
    public string Icon { get; set; } = "\uE7ba"; // Default warning icon

    // Auto-Fix Properties
    public string RemediationScript { get; set; } = string.Empty;
    public string RemediationFileName { get; set; } = "fix.ps1";
    public bool HasRemediation => !string.IsNullOrEmpty(RemediationFileName);
    public System.Windows.Input.ICommand DownloadScriptCommand { get; set; }

    // Rollback Engine (Next-Gen)
    public string RollbackScript { get; set; } = string.Empty;
    public string RollbackFileName { get; set; } = "undo_fix.ps1";
    public bool HasRollback => !string.IsNullOrEmpty(RollbackScript);
    public System.Windows.Input.ICommand DownloadRollbackCommand { get; set; }

    // Live Risk Actions
    public System.Windows.Input.ICommand ApplyFixCommand { get; set; }
    public System.Windows.Input.ICommand SimulateNeglectCommand { get; set; } // "Uygulamazsam ne olur?"

    // Security Explain Engine
    public string Description { get; set; } = string.Empty; // ❓ Nedir
    public string RiskEx { get; set; } = string.Empty; // ⚠️ Neden riskli
    public string Consequence { get; set; } = string.Empty; // 💣 Yapılmazsa ne olur
    public string RealScenario { get; set; } = string.Empty; // 🧠 Gerçek senaryo
    public string SideEffects { get; set; } = string.Empty; // ⚙️ Yan etkisi
    
    // Education Mode
    public string EducationalContent { get; set; } = string.Empty; // 📚 Bu nedir?
    public string RealWorldExample { get; set; } = string.Empty; // 🌍 Gerçek Saldırı Örneği
    public string HackerMindset { get; set; } = string.Empty; // 🕵️‍♂️ Hacker burayı nasıl görür?
    
    public double PriorityScore { get; set; } // 0.0 - 10.0 (Decision Support)
    public string WhyAIRecommends { get; set; } = string.Empty; // AI Rationale
}
