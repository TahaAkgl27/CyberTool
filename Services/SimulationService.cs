using System;
using System.Collections.Generic;
using System.Linq;
using CyberTool.Models;

namespace CyberTool.Services
{
    public class AttackStep
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; } = "➡️"; // Arrow default
        public string Status { get; set; } = "Pending"; // Verified, Potential, Blocked
    }

    public class AttackScenarioResult
    {
        public string Title { get; set; }
        public string Severity { get; set; } // Critical, High, Medium
        public string Description { get; set; }
        public List<AttackStep> Steps { get; set; } = new List<AttackStep>();
        public string RemediationPlan { get; set; }
        public List<string> Impacts { get; set; } = new();
        public int ProbabilityScore { get; set; } // 0-100
    }

    public static class SimulationService
    {
        public static List<AttackScenarioResult> SimulateAttack(IList<PortFinding> findings)
        {
            var scenarios = new List<AttackScenarioResult>();
            
            // 1. Graph Engine'i Başlat
            var engine = new AttackGraphEngine();
            engine.BuildGraph(findings);
            var attackPaths = engine.SimulateAttacks();

            // 2. Yolları Senaryolara Dönüştür
            int index = 1;
            foreach (var path in attackPaths)
            {
                var finalNode = path.Steps.Last().Target;
                var firstStep = path.Steps.First();
                
                var scenario = new AttackScenarioResult
                {
                    Title = $"🚨 Simülasyon #{index}: {finalNode.Name} Hedefli Saldırı",
                    Severity = finalNode.SensitivityLevel >= 8 ? "🔴 Kritik" : (finalNode.SensitivityLevel >= 5 ? "🟠 Yüksek" : "🟡 Orta"),
                    ProbabilityScore = (int)path.Steps.Average(s => s.SuccessProbability),
                    Description = engine.GenerateStory(path), // Hikaye buraya geliyor
                    Steps = path.Steps.Select((step, i) => new AttackStep 
                    { 
                        Title = $"Adım {i + 1}: {step.Source.Name} ➡️ {step.Target.Name}",
                        Description = step.Narrative,
                        Icon = "💀",
                        Status = "Simulated"
                    }).ToList(),
                    Impacts = new List<string> { "Veri Kaybı", "Sistem Kesintisi", "İtibar Hasarı" },
                    RemediationPlan = "Saldırı yolundaki zafiyetleri (node) izole edin ve yamaları uygulayın."
                };
                
                scenarios.Add(scenario);
                index++;
            }

            // Eğer hiç yol bulunamazsa eski fallback (Genel Analiz) dönebilir veya boş dönebilir
            if (!scenarios.Any() && findings.Any())
            {
                 scenarios.Add(new AttackScenarioResult
                 {
                     Title = "✅ Sıkılaştırılmış Sistem",
                     Severity = "🟢 Güvenli",
                     ProbabilityScore = 5,
                     Description = "AI Simülasyon motoru şu an için belirgin bir saldırı yolu (Attack Path) tespit edemedi. Ancak tekil zafiyetler hala mevcut olabilir.",
                     Steps = new List<AttackStep> { new AttackStep { Title = "Tarama Tamamlandı", Description = "Zincirleme saldırı riski düşük.", Icon = "🛡️" } }
                 });
            }

            return scenarios;
        }
    }
}
