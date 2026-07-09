namespace CyberTool.Models;

public class AttackNode
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double X { get; set; }
    public double Y { get; set; }
    public string Color { get; set; } = "#808080";
    public string Icon { get; set; } = "";
    public bool ShowArrow { get; set; } = true;
    public string RiskLevel { get; set; } = "High";
}

public class AttackLink
{
    public double X1 { get; set; }
    public double Y1 { get; set; }
    public double X2 { get; set; }
    public double Y2 { get; set; }
    public string Stroke { get; set; } = "#AAAAAA";
}
