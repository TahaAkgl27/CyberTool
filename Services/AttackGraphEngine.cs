using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyberTool.Models;

namespace CyberTool.Services
{
    public class SimulationNode
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Type { get; set; } // "Internet", "Service", "Database", "System", "AdminPanel"
        public string Description { get; set; }
        public bool IsCompromised { get; set; }
        public int SensitivityLevel { get; set; } // 0-10
    }

    public class SimulationEdge
    {
        public SimulationNode Source { get; set; }
        public SimulationNode Target { get; set; }
        public string ExploitMethod { get; set; } // "SQL Injection", "Brute Force", "Default Creds"
        public int SuccessProbability { get; set; } // 0-100
        public string Narrative { get; set; } // "Şuraya zıplardım" kısmı için
    }

    public class SimulationPath
    {
        public List<SimulationEdge> Steps { get; set; } = new List<SimulationEdge>();
        public int TotalRiskScore => Steps.Sum(s => s.SuccessProbability * s.Target.SensitivityLevel);
    }

    public class AttackGraphEngine
    {
        private List<SimulationNode> _nodes = new List<SimulationNode>();
        private List<SimulationEdge> _edges = new List<SimulationEdge>();

        public void BuildGraph(IList<PortFinding> findings)
        {
            _nodes.Clear();
            _edges.Clear();

            // 1. Root Node: Attacker (Internet)
            var attackerNode = new SimulationNode { Name = "İnternet (Saldırgan)", Type = "Internet", Description = "Dış dünya", SensitivityLevel = 0, IsCompromised = true };
            _nodes.Add(attackerNode);

            foreach (var finding in findings)
            {
                // Her bulgu bir potansiyel giriş noktasıdır
                var serviceNode = new SimulationNode 
                { 
                    Name = $"{finding.Service} ({finding.Port})", 
                    Type = "Service", 
                    Description = finding.RiskReason ?? "Açık Servis",
                    SensitivityLevel = 3
                };
                _nodes.Add(serviceNode);

                // Attack Edge: Internet -> Service
                _edges.Add(new SimulationEdge
                {
                    Source = attackerNode,
                    Target = serviceNode,
                    ExploitMethod = "Port Exploitation",
                    SuccessProbability = EvaluateProbability(finding),
                    Narrative = $"Önce açık bulduğum {finding.Port} portundaki {finding.Service ?? "servis"} üzerinden sisteme sızmaya çalışırdım."
                });

                // 2. Lateral Movement Logic (Basit Kurallar)
                if (finding.Service != null && (finding.Service.Contains("sql") || finding.Port == 1433 || finding.Port == 3306))
                {
                    var  dbNode = new SimulationNode { Name = "Veritabanı", Type = "Database", Description = "Kritik Veriler", SensitivityLevel = 9 };
                    _nodes.Add(dbNode);
                    
                    _edges.Add(new SimulationEdge 
                    { 
                        Source = serviceNode, 
                        Target = dbNode, 
                        ExploitMethod = "SQL Injection / Credential Stuffing",
                        SuccessProbability = 80,
                        Narrative = "Buradan elde ettiğim erişimle doğrudan veritabanına sıçrar, tüm verileri çekerdim."
                    });
                }
                else if (finding.Port == 445 || finding.Port == 139)
                {
                    var fileServerNode = new SimulationNode { Name = "Dosya Sunucusu", Type = "System", Description = "Hassas Dosyalar", SensitivityLevel = 7 };
                    _nodes.Add(fileServerNode);

                    _edges.Add(new SimulationEdge
                    {
                        Source = serviceNode,
                        Target = fileServerNode,
                        ExploitMethod = "SMB Exploitation",
                        SuccessProbability = 60,
                        Narrative = "Paylaşıma açık dosyaları tarar, yapılandırma dosyalarından şifreleri çalardım."
                    });
                }
                else if (finding.Port == 3389) // RDP
                {
                    var adminNode = new SimulationNode { Name = "Yönetici Yetkisi", Type = "AdminPanel", Description = "Tam Kontrol", SensitivityLevel = 10 };
                    _nodes.Add(adminNode);

                    _edges.Add(new SimulationEdge
                    {
                        Source = serviceNode,
                        Target = adminNode,
                        ExploitMethod = "BlueKeep / Brute Force",
                        SuccessProbability = 40,
                        Narrative = "RDP üzerinden yönetici parolası deneyerek sunucuyu tamamen ele geçirirdim."
                    });
                }
            }
        }

        /// <summary>
        /// Gelişmiş Matematiksel Risk Modeli
        /// </summary>
        private int EvaluateProbability(PortFinding finding)
        {
            // 1. Base Score (Zafiyet Ciddiyeti)
            double baseScore = 0;
            switch (finding.Risk)
            {
                case "Critical": baseScore = 90; break;
                case "High": baseScore = 75; break;
                case "Medium": baseScore = 50; break;
                case "Low": baseScore = 20; break;
                default: baseScore = 10; break;
            }

            // 2. Exploit Availability (Saldırı Aracı Çarpanı)
            // Gerçek dünyada bu CVE veritabanından çekilir. Burada simüle ediyoruz.
            // Popüler servislerde exploit bulunma ihtimali daha yüksektir.
            double exploitRunMultiplier = 1.0;
            
            if (finding.Service != null)
            {
                var s = finding.Service.ToLower();
                if (s.Contains("smb") || s.Contains("microsoft-ds")) { exploitRunMultiplier = 1.4; } // EternalBlue vb.
                else if (s.Contains("rdp") || s.Contains("ms-wbt-server")) { exploitRunMultiplier = 1.3; } // BlueKeep
                else if (s.Contains("ftp") || s.Contains("telnet")) { exploitRunMultiplier = 1.25; } // Brute-force kolaylığı
                else if (s.Contains("http")) { exploitRunMultiplier = 1.1; } // Web zafiyetleri yaygındır
            }

            // 3. Exposure Factor (Dış Erişim Çarpanı)
            // Simülasyon olduğu için tüm portları "Internet" node'una bağlı kabul ediyoruz (External Scan varsayımı)
            // Eğer Internal Scan olsaydı bu çarpan düşebilirdi.
            double exposureMultiplier = 1.2; 

            // 4. Trend Factor (Global Saldırı Trendleri)
            // Güncel siber istihbarat verilerine göre (Simülasyon)
            double trendBonus = 0;
            if (finding.Port == 445 || finding.Port == 3389) trendBonus = 5; // Ransomware gruplarının favorisi

            // --- HESAPLAMA ---
            double totalProbability = (baseScore * exploitRunMultiplier * exposureMultiplier) + trendBonus;

            // 0-99 arası Clamp yap (Asla %100 güvenli veya %100 hacklendi denemez, %99 tavan)
            if (totalProbability > 99) totalProbability = 99;
            if (totalProbability < 1) totalProbability = 1;

            return (int)totalProbability;
        }

        public List<SimulationPath> SimulateAttacks()
        {
            var paths = new List<SimulationPath>();
            var startNode = _nodes.FirstOrDefault(n => n.Type == "Internet");
            if (startNode == null) return paths;

            // Basit bir BFS ile tüm yolları bulalım (Derinlik şu an 2-3 seviye ile sınırlı)
            // Daha gelişmiş graph traversal yapılabilir.
            
            foreach (var edge in _edges.Where(e => e.Source == startNode))
            {
                var path = new SimulationPath();
                path.Steps.Add(edge);
                
                // 2. Sekme (Hop) varsa onu da ekle
                var nextEdges = _edges.Where(e => e.Source == edge.Target).ToList();
                if (nextEdges.Any())
                {
                    // Şimdilik en yüksek olasılıklı yolu seçelim
                    var bestNextHop = nextEdges.OrderByDescending(e => e.SuccessProbability).First();
                    path.Steps.Add(bestNextHop);
                }

                paths.Add(path);
            }

            return paths.OrderByDescending(p => p.TotalRiskScore).ToList();
        }

        public string GenerateStory(SimulationPath path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("🕵️ **Saldırgan Günlüğü & Risk Analizi:**");
            sb.AppendLine("*(Bu senaryo Gelişmiş Olasılık Motoru ile hesaplanmıştır)*");
            sb.AppendLine("");
            
            int stepCount = 1;
            foreach (var step in path.Steps)
            {
                sb.AppendLine($"**Adım {stepCount}:** {step.Narrative}");
                
                // Risk Faktörü Açıklaması Ekle
                if (step.SuccessProbability > 70)
                {
                    sb.AppendLine($"   ⚠️ *Yüksek Başarı İhtimali (%{step.SuccessProbability}):* Hedef servis üzerinde bilinen exploitler veya zayıf yapılandırma tespit edildi.");
                    if (step.Target.Name.Contains("SMB") || step.Target.Name.Contains("RDP"))
                    {
                        sb.AppendLine($"   🌍 *Global Trend:* Bu servisler fidye yazılımı grupları tarafından sıkça hedef alınmaktadır.");
                    }
                }
                else if (step.SuccessProbability > 40)
                {
                     sb.AppendLine($"   🎲 *Orta İhtimal (%{step.SuccessProbability}):* Saldırganın başarı şansı var ancak efor gerektirir.");
                }
                
                sb.AppendLine("");
                stepCount++;
            }

            var finalNode = path.Steps.Last().Target;
            sb.AppendLine("---");
            sb.AppendLine($"💥 **SONUÇ:** {finalNode.Name} ele geçirildi!");
            sb.AppendLine($"📊 **Kümülatif Risk Skoru:** {path.TotalRiskScore}");
            
            return sb.ToString();
        }
    }
}
