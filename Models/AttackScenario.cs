namespace CyberTool.Models;

public sealed class AttackScenario
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Severity { get; init; } = "High"; // Critical, High, Medium, Low
    public string Category { get; init; } = string.Empty; // Entry Point, Lateral Movement, etc.
    public string Impact { get; init; } = string.Empty; // "Bu risk kapatılmazsa..."
    public string AttackerMindset { get; init; } = string.Empty; // "Saldırgan burayı neden seçer?"
    public string Rationale { get; init; } = string.Empty; // "Neden kritik? Neden şimdi?"
    public int DaysOpen { get; set; } // Zaman boyutu

    public string DisplaySeverity => Severity switch
    {
        "Critical" => "KRİTİK",
        "High" => "YÜKSEK",
        "Medium" => "ORTA",
        "Low" => "DÜŞÜK",
        "Info" => "BİLGİ",
        _ => Severity.ToUpper()
    };
}
