using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace CyberTool.Services;

public class SystemEnumerationService
{
    public async Task<SystemInfoResult> EnumerateAsync(string ipAddress)
    {
        var result = new SystemInfoResult { IpAddress = ipAddress, Source = "Nmap/Passive" };
        
        // Expanded Protocol Enumeration
        // Added ports based on user feedback: 1433 (SQL), 5985 (WinRM), 2121 (Alt FTP), 8080 (Alt HTTP)
        // Added scripts: mssql-info, ssl-cert, nbstat (NetBIOS)
        string scriptArgs = "snmp-info,snmp-sysdescr,snmp-processes,smb-os-discovery,smb-security-mode,rdp-ntlm-info,http-server-header,http-title,ftp-anon,mssql-info,ssl-cert,nbstat"; 
        
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "nmap",
                // Nmap needs -sU for UDP 137 (NetBIOS). We mix TCP (T:) and UDP (U:) ports.
                // -Pn: No ping. -sV: Version detection. -sU: UDP Scan.
                // T:21,22,53,80,135,139,443,445,161,1433,2121,3389,5985,8080
                // U:137,161 (SNMP is also UDP usually, but 137 is critical for nbstat)
                Arguments = $"-Pn -sV -sU -p U:137,U:161,T:21,T:22,T:53,T:80,T:135,T:139,T:443,T:445,T:1433,T:2121,T:3389,T:5985,T:8080 --script {scriptArgs} -oX - \"{ipAddress}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            var xmlOutput = await process.StandardOutput.ReadToEndAsync();
            var errorOutput = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (!string.IsNullOrWhiteSpace(xmlOutput) && xmlOutput.Contains("<nmaprun"))
            {
                 ParseNmapResult(xmlOutput, result);
            }
            else
            {
                 Debug.WriteLine($"Nmap Error: {errorOutput}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Nmap enum scan failed: {ex.Message}");
        }

        // Failsafe: Try WMI if Nmap missed Hardware Info (Common in Domain Env)
        if (string.IsNullOrEmpty(result.CpuModel) || result.TotalRamGb == null)
        {
            await TryGetWmiInfo(ipAddress, result);
        }

        return result;
    }

    private async Task TryGetWmiInfo(string ip, SystemInfoResult info)
    {
        try
        {
            await Task.Run(() => 
            {
                try
                {
                    var options = new ConnectionOptions 
                    { 
                        Impersonation = ImpersonationLevel.Impersonate,
                        Authentication = AuthenticationLevel.PacketPrivacy,
                        Timeout = TimeSpan.FromSeconds(5) // Fast fail
                    };
                    var scope = new ManagementScope($"\\\\{ip}\\root\\cimv2", options);
                    scope.Connect();

                    // CPU
                    if (string.IsNullOrEmpty(info.CpuModel))
                    {
                        var query = new ObjectQuery("SELECT Name FROM Win32_Processor");
                        using var searcher = new ManagementObjectSearcher(scope, query);
                        foreach (ManagementObject mo in searcher.Get())
                        {
                            info.CpuModel = mo["Name"]?.ToString()?.Trim() ?? "";
                            break; // Just get first CPU
                        }
                    }

                    // RAM
                    if (info.TotalRamGb == null)
                    {
                        var query = new ObjectQuery("SELECT TotalPhysicalMemory, Model FROM Win32_ComputerSystem");
                        using var searcher = new ManagementObjectSearcher(scope, query);
                        foreach (ManagementObject mo in searcher.Get())
                        {
                            if (ulong.TryParse(mo["TotalPhysicalMemory"]?.ToString(), out ulong bytes))
                            {
                                info.TotalRamGb = Math.Round(bytes / (1024.0 * 1024.0 * 1024.0), 1);
                            }
                            
                            // Also get Model if missing
                            var model = mo["Model"]?.ToString();
                            if (!string.IsNullOrEmpty(model) && (string.IsNullOrEmpty(info.OsDescription) || !info.OsDescription.Contains(model)))
                            {
                                // We don't have a specific Model field in result, maybe append to OsDescription or separate?
                                // result.Model? DeviceProfile has it. Let's start with just CPU/RAM.
                            }
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(info.CpuModel) || info.TotalRamGb != null)
                    {
                        info.Source += " + WMI";
                    }
                }
                catch (UnauthorizedAccessException) 
                {
                    // Expected if not admin
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"WMI Internal Error: {ex.Message}");
                }
            });
        }
        catch (Exception ex) 
        { 
            Debug.WriteLine($"WMI Check Failed: {ex.Message}"); 
        }
    }

    public async Task<SystemInfoResult> EnumerateAuthAsync(string ip, string username, string password)
    {
        var result = new SystemInfoResult { IpAddress = ip, Source = "Authenticated WMI (Deep)" };
        
        try
        {
            await Task.Run(() => 
            {
                var options = new ConnectionOptions 
                { 
                    Username = username,
                    Password = password,
                    Impersonation = ImpersonationLevel.Impersonate,
                    Authentication = AuthenticationLevel.PacketPrivacy
                };
                
                var scope = new ManagementScope($"\\\\{ip}\\root\\cimv2", options);
                scope.Connect();

                // 1. OS & Version
                var queryOs = new ObjectQuery("SELECT Caption, Version, BuildNumber, CSName, InstallDate, LastBootUpTime, TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");
                using var searcherOs = new ManagementObjectSearcher(scope, queryOs);
                foreach (ManagementObject mo in searcherOs.Get())
                {
                    result.OsDescription = mo["Caption"]?.ToString() ?? "";
                    result.Hostname = mo["CSName"]?.ToString() ?? ""; // Authoritative Hostname
                    // Add build info to description if needed, or separate
                    var version = mo["Version"]?.ToString();
                    if (!string.IsNullOrEmpty(version)) result.OsDescription += $" (Build {version})";
                    
                    // Memory from OS view (more accurate for visible)
                    if (ulong.TryParse(mo["TotalVisibleMemorySize"]?.ToString(), out ulong kbytes))
                    {
                        result.TotalRamGb = Math.Round(kbytes / (1024.0 * 1024.0), 1);
                    }
                }

                // 2. CPU
                var queryCpu = new ObjectQuery("SELECT Name, NumberOfCores FROM Win32_Processor");
                using var searcherCpu = new ManagementObjectSearcher(scope, queryCpu);
                foreach (ManagementObject mo in searcherCpu.Get())
                {
                    result.CpuModel = mo["Name"]?.ToString()?.Trim() ?? "";
                    // Could also get core count if needed
                    break; 
                }

                // 3. System Info (Model/Manufacturer)
                var querySys = new ObjectQuery("SELECT Manufacturer, Model, Domain FROM Win32_ComputerSystem");
                using var searcherSys = new ManagementObjectSearcher(scope, querySys);
                foreach (ManagementObject mo in searcherSys.Get())
                {
                    result.Domain = mo["Domain"]?.ToString() ?? "";
                    // Store Model/Manufacturer in result if we add those fields, or append to existing string fields for now
                    // For now, let's keep it simple or expand SystemInfoResult if needed. 
                    // Let's assume the ViewModel will handle merging extra data or we ignore it for now.
                    // Actually, let's return them in a dictionary or just use valid fields.
                }

                // 4. Installed Hotfixes (Detailed)
                var queryHotfix = new ObjectQuery("SELECT HotFixID FROM Win32_QuickFixEngineering");
                using var searcherHotfix = new ManagementObjectSearcher(scope, queryHotfix);
                foreach (ManagementObject mo in searcherHotfix.Get())
                {
                    var id = mo["HotFixID"]?.ToString();
                    if (!string.IsNullOrEmpty(id)) result.Hotfixes.Add(id);
                }
                result.DatabaseInfo = $"{result.Hotfixes.Count} Hotfixes Installed"; 

                // 5. Installed Software (Win32_Product is heavy, but standard for full list)
                // Warning: querying Win32_Product can trigger consistency check. 
                // Alternative: Registry. But for remote WMI, Win32_Product is easiest for MVP.
                try 
                {
                    var querySoft = new ObjectQuery("SELECT Name, Version FROM Win32_Product");
                    using var searcherSoft = new ManagementObjectSearcher(scope, querySoft);
                    foreach (ManagementObject mo in searcherSoft.Get())
                    {
                        var name = mo["Name"]?.ToString();
                        var ver = mo["Version"]?.ToString();
                        if (!string.IsNullOrEmpty(name)) result.InstalledSoftware.Add($"{name} ({ver})");
                    }
                }
                catch { /* Ignore if Win32_Product fails or times out */ }

                // 6. Services (Running or Auto-Start Stopped)
                // Get critical ones
                var querySvc = new ObjectQuery("SELECT Name, DisplayName, State, StartMode FROM Win32_Service WHERE State='Running' OR (StartMode='Auto' AND State='Stopped')");
                using var searcherSvc = new ManagementObjectSearcher(scope, querySvc);
                foreach (ManagementObject mo in searcherSvc.Get())
                {
                    var dName = mo["DisplayName"]?.ToString();
                    var state = mo["State"]?.ToString();
                    var mode = mo["StartMode"]?.ToString();
                    
                    // Filter out common noise if needed, or just add all
                    result.Services.Add($"{dName} [{state}]");
                }
                
                result.RemoteManagementInfo = "WMI Deep Scan Successful";
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Auth WMI Failed: {ex.Message}");
            throw; // Re-throw so ViewModel knows it failed
        }

        return result;
    }

    private void ParseNmapResult(string xml, SystemInfoResult info)
    {
        try
        {
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            var host = doc.Descendants("host").FirstOrDefault();
            if (host == null) return;

            // 1. Service Version Fallback
            var ports = host.Descendants("port");
            foreach (var port in ports)
            {
                var service = port.Element("service");
                if (service != null)
                {
                    var product = (string?)service.Attribute("product");
                    var version = (string?)service.Attribute("version");
                    var extrainfo = (string?)service.Attribute("extrainfo");
                    var portId = (string?)port.Attribute("portid");
                    var protocol = (string?)port.Attribute("protocol");

                    // UDP 137 NetBIOS Banner Parsing (Very reliable for Workgroup)
                    if (portId == "137" && protocol == "udp" && !string.IsNullOrEmpty(extrainfo))
                    {
                        // extrainfo="workgroup: EXAMPLE"
                        if (extrainfo.Contains("workgroup:", StringComparison.OrdinalIgnoreCase))
                        {
                            var parts = extrainfo.Split(new[] { "workgroup:" }, StringSplitOptions.None);
                            if (parts.Length > 1) 
                            {
                                var wg = parts[1].Trim().TrimEnd(')');
                                if (string.IsNullOrEmpty(info.Domain) || info.Domain == "N/A") 
                                    info.Domain = wg;
                            }
                        }
                        
                        // Sometimes service has hostname attribute directly
                        var svcHostname = (string?)service.Attribute("hostname");
                        if (!string.IsNullOrEmpty(svcHostname) && string.IsNullOrEmpty(info.Hostname))
                        {
                            info.Hostname = svcHostname;
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(product))
                    {
                        if (product.Contains("IIS") || product.Contains("Apache") || product.Contains("nginx"))
                        {
                            info.WebServerType = $"{product} {version}";
                        }
                        
                        // OS Hint
                        if (string.IsNullOrEmpty(info.OsDescription) && (extrainfo?.Contains("Windows") ?? false))
                        {
                            info.OsDescription = extrainfo;
                        }
                        
                        // Hostname from Service info? (Sometimes in extrainfo or hostname tag)
                        // Nmap XML puts discovered hostnames in <hostnames> tag usually, let's check that too
                    }
                }
            }
            
            // Check global hostnames tag
            var hostnames = host.Element("hostnames");
            if (hostnames != null)
            {
                foreach(var hn in hostnames.Descendants("hostname"))
                {
                    var name = (string?)hn.Attribute("name");
                    var type = (string?)hn.Attribute("type"); // user, PTR
                    if (!string.IsNullOrEmpty(name))
                    {
                         if (string.IsNullOrEmpty(info.Hostname)) info.Hostname = name;
                         
                         // Heuristic: If hostname has dots, it might contain domain
                         // e.g. workstation.corp.local
                         if (name.Contains(".") && string.IsNullOrEmpty(info.Domain))
                         {
                             var parts = name.Split('.', 2);
                             if (parts.Length > 1) info.Domain = parts[1].ToUpper();
                         }
                    }
                }
            }

            // 2. Script Parsing
            var allScripts = doc.Descendants("script");

            foreach (var script in allScripts)
            {
                var id = (string?)script.Attribute("id");
                var output = (string?)script.Attribute("output");

                if (string.IsNullOrEmpty(output)) continue;

                // --- SNMP ---
                if (id == "snmp-info" || id == "snmp-sysdescr")
                {
                    // Extract RAM if available (e.g. "Physical Memory: 4096MB")
                    if (output.Contains("Physical Memory:"))
                    {
                        try 
                        {
                            var ramPart = output.Split(new[] { "Physical Memory:" }, StringSplitOptions.None)[1]
                                                .Split(new[] { '\n', ',' }, StringSplitOptions.RemoveEmptyEntries)[0]
                                                .Trim();
                            
                            if (ramPart.EndsWith("MB", StringComparison.OrdinalIgnoreCase))
                            {
                                 if (double.TryParse(ramPart.Substring(0, ramPart.Length - 2).Trim(), out double mb))
                                     info.TotalRamGb = Math.Round(mb / 1024.0, 1);
                            }
                            else if (ramPart.EndsWith("GB", StringComparison.OrdinalIgnoreCase))
                            {
                                 if (double.TryParse(ramPart.Substring(0, ramPart.Length - 2).Trim(), out double gb))
                                     info.TotalRamGb = gb;
                            }
                        } catch {}
                    }

                    if (output.Contains("Windows"))
                    {
                        info.OsFamily = "Windows";
                        var softwareIndex = output.IndexOf("Software:", StringComparison.OrdinalIgnoreCase);
                        if (softwareIndex >= 0)
                        {
                            var softPart = output.Substring(softwareIndex + 9).Split('\n')[0].Trim();
                            info.OsDescription = softPart; 
                        }
                        else info.OsDescription = output;
                    }
                    else info.OsDescription = output;
                }
                
                if (id == "snmp-processors")
                {
                   var lines = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                   var cpuLine = lines.FirstOrDefault();
                   if (cpuLine != null)
                   {
                       var idx = cpuLine.IndexOf(':');
                       info.CpuModel = (idx >= 0 && idx < 5) ? cpuLine.Substring(idx + 1).Trim() : cpuLine.Trim();
                   }
                }

                // --- SMB ---
                if (id == "smb-os-discovery")
                {
                    foreach(var elem in script.Descendants("elem"))
                    {
                        var key = (string?)elem.Attribute("key");
                        var val = elem.Value;
                        
                        if (key == "os") info.OsDescription = val;
                        if (key == "server") 
                        {
                            info.Hostname = val;
                        }
                        if (key == "domain_dns" || key == "forest_dns") info.Domain = val;
                        else if ((key == "domain" || key == "workgroup") && string.IsNullOrEmpty(info.Domain)) info.Domain = val;
                    }
                }
                
                // --- NetBIOS (nbstat) ---
                if (id == "nbstat")
                {
                     // Output example: 
                     // NetBIOS name: JUPITER, NetBIOS user: <unknown>, NetBIOS MAC: ...
                     // Names:
                     //   JUPITER<00>          Flags: <unique><active>
                     //   WORKGROUP<00>        Flags: <group><active>
                     //   JUPITER<20>          Flags: <unique><active>
                     
                     var lines = output.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                     foreach(var line in lines)
                     {
                         if (line.Contains("NetBIOS name:")) 
                         {
                             var parts = line.Split(',');
                             var namePart = parts.FirstOrDefault(p => p.Trim().StartsWith("NetBIOS name:"));
                             if (namePart != null) info.Hostname = namePart.Split(':')[1].Trim();
                         }
                     }
                     
                     // Parse Table for Group Name
                     // Regex to find names with <group> flag
                     // Matches:  NAME<00> ... <group>
                     try 
                     {
                         // Simple heuristic: Look for lines with "<group>"
                         foreach(var line in lines)
                         {
                             if (line.Contains("<group>") && line.Contains("<00>"))
                             {
                                 // "WORKGROUP<00> ..."
                                 var groupNameEnd = line.IndexOf("<00>");
                                 if (groupNameEnd > 0)
                                 {
                                     var groupName = line.Substring(0, groupNameEnd).Trim();
                                     if (!string.IsNullOrEmpty(groupName) && groupName != info.Hostname)
                                     {
                                         info.Domain = groupName; // This is Workgroup or Domain
                                     }
                                 }
                             }
                         }
                     }
                     catch {}
                }

                if (id == "smb-security-mode")
                {
                    // Parsing "message_signing: disabled"
                    if (output.Contains("message_signing: disabled")) info.SmbSigningStatus = "Disabled (Risky)";
                    else if (output.Contains("message_signing: enabled")) info.SmbSigningStatus = "Enabled";
                    else if (output.Contains("message_signing: required")) info.SmbSigningStatus = "Required (Secure)";
                }

                // --- RDP ---
                if (id == "rdp-ntlm-info")
                {
                    foreach (var elem in script.Descendants("elem"))
                    {
                        var key = (string?)elem.Attribute("key");
                        if (key == "Target_Name") info.Hostname = elem.Value; // Fallback
                        if (key == "Domain_Name") info.Domain = elem.Value;
                        if (key == "Product_Version") 
                        {
                            // Hint for OS version
                        }
                    }
                }

                // --- HTTP ---
                if (id == "http-server-header")
                {
                     var header = script.Descendants("elem").FirstOrDefault()?.Value ?? output;
                    info.WebServerType = header;
                }

                // --- MSSQL ---
                if (id == "mssql-info")
                {
                    // <elem key="version">15.0.2000</elem>
                    // <elem key="product-name">Microsoft SQL Server 2019</elem>
                    string pName = "";
                    string pVer = "";
                    foreach (var elem in script.Descendants("elem"))
                    {
                        var key = (string?)elem.Attribute("key");
                        if (key == "product-name") pName = elem.Value;
                        if (key == "version") pVer = elem.Value;
                    }
                    if (!string.IsNullOrEmpty(pName)) info.DatabaseInfo = $"{pName} {pVer}".Trim();
                    else info.DatabaseInfo = output;
                }

                // --- FTP ---
                if (id == "ftp-anon")
                {
                    if (output.Contains("Anonymous FTP login allowed")) 
                        info.IsFtpAnonymous = true;
                }
            }

            // Check for WinRM (5985) manually via Ports if script didn't catch it
            // WinRM usually runs on 5985 HTTP. detailed -sV might show "Microsoft HTTPAPI httpd 2.0 (SSDP/UPnP)"
            // But we can just infer presence if port is open in Nmap results
            var winRmPort = doc.Descendants("port").FirstOrDefault(p => (string?)p.Attribute("portid") == "5985" && p.Element("state")?.Attribute("state")?.Value == "open");
            if (winRmPort != null)
            {
                var svc = winRmPort.Element("service");
                var prod = (string?)svc?.Attribute("product") ?? "Windows Remote Management";
                info.RemoteManagementInfo = $"{prod} (Port 5985 Open)";
            }
            
            // Post-Process Logic
            if(!string.IsNullOrEmpty(info.DatabaseInfo) || !string.IsNullOrEmpty(info.RemoteManagementInfo))
            {
                info.Source = "Nmap NSE (Deep)";
            }

        }
        catch { /* Silent parse fail */ }
    }
}

public class SystemInfoResult
{
    public string IpAddress { get; set; } = string.Empty;
    public string Hostname { get; set; } = string.Empty;
    public string OsFamily { get; set; } = "Unknown";
    public string OsDescription { get; set; } = string.Empty;
    public double? TotalRamGb { get; set; }
    public string CpuModel { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string SmbSigningStatus { get; set; } = string.Empty;
    public string WebServerType { get; set; } = string.Empty;
    public string DatabaseInfo { get; set; } = string.Empty;
    public string RemoteManagementInfo { get; set; } = string.Empty;
    public bool IsFtpAnonymous { get; set; } = false;
    public List<string> Hotfixes { get; set; } = new();
    public List<string> InstalledSoftware { get; set; } = new();
    public List<string> Services { get; set; } = new(); // Format: "Name (State)"
    public string Source { get; set; } = string.Empty;
}
