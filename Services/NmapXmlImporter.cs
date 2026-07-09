using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CyberTool.Models;

namespace CyberTool.Services;

public static class NmapXmlImporter
{
    public static ScanSession Import(string xmlPath)
    {
        if (string.IsNullOrWhiteSpace(xmlPath))
        {
            throw new ArgumentException("xmlPath is required", nameof(xmlPath));
        }

        if (!File.Exists(xmlPath))
        {
            throw new FileNotFoundException("Nmap XML file not found.", xmlPath);
        }

        var doc = XDocument.Load(xmlPath);

        var host = doc.Descendants("host").FirstOrDefault();
        var address = host?.Descendants("address")
            .FirstOrDefault(a => (string?)a.Attribute("addr") is not null);

        var target = (string?)address?.Attribute("addr")
                     ?? (string?)host?.Element("hostnames")?.Element("hostname")?.Attribute("name")
                     ?? "(unknown)";

        var session = new ScanSession
        {
            Target = target,
            StartedAt = DateTimeOffset.Now,
        };

        // OS Detection
        var osMatch = host?.Descendants("osmatch").FirstOrDefault();
        session.OsDescription = (string?)osMatch?.Attribute("name") ?? string.Empty;
        
        var osClass = host?.Descendants("osclass").FirstOrDefault();
        session.DeviceType = (string?)osClass?.Attribute("type") ?? string.Empty;

        var ports = host?.Descendants("port") ?? Enumerable.Empty<XElement>();

        foreach (var p in ports)
        {
            var state = (string?)p.Element("state")?.Attribute("state") ?? "unknown";
            if (!string.Equals(state, "open", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var protocol = (string?)p.Attribute("protocol") ?? "tcp";
            var portIdStr = (string?)p.Attribute("portid");
            _ = int.TryParse(portIdStr, out var portId);

            var serviceElement = p.Element("service");
            var serviceName = (string?)serviceElement?.Attribute("name");
            var product = (string?)serviceElement?.Attribute("product");
            var version = (string?)serviceElement?.Attribute("version");
            var extra = (string?)serviceElement?.Attribute("extrainfo");

            session.Findings.Add(new PortFinding
            {
                Target = target,
                Port = portId,
                Protocol = protocol,
                State = state,
                Service = serviceName,
                Product = product,
                Version = version,
                ExtraInfo = extra,
                Risk = null,
                Recommendation = null,
            });
        }

        session.FinishedAt = DateTimeOffset.Now;
        return session;
    }
}
