using System;

namespace CyberTool.Services;

public class AuthService
{
    public bool IsAuthenticated { get; private set; }
    public event EventHandler? AuthenticationChanged;

#if DEBUG
    private const string DefaultUser = "admin";
    private const string DefaultPass = "admin123";
#endif

    public bool Login(string username, string password)
    {
#if DEBUG
        if (username == DefaultUser && password == DefaultPass)
        {
            IsAuthenticated = true;
            AuthenticationChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
#endif

        return false;
    }

    public void Logout()
    {
        IsAuthenticated = false;
        AuthenticationChanged?.Invoke(this, EventArgs.Empty);
    }
}
