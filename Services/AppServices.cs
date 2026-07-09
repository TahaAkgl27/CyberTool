namespace CyberTool.Services;

public static class AppServices
{
    public static ScanStore ScanStore { get; } = new();
    public static AuthService AuthService { get; } = new();
}
