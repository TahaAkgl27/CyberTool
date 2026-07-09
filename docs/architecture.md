# CyberTool Architecture

This document describes the technical architecture of CyberTool as implemented today, and outlines planned extensibility.

## Design Principles

| Principle | Implementation |
|-----------|----------------|
| Local-first | Scan data and history stored on the user's machine |
| MVVM separation | Views bind to ViewModels; business logic in Services |
| Optional AI | OpenAI integration is user-configured and non-blocking |
| Authorized scope | Offensive modules documented and training-oriented |
| Inspectability | Open source codebase for audit and education |

---

## Layer Overview

```mermaid
flowchart TB
    subgraph Presentation["Presentation Layer"]
        V[Views - WinUI 3 XAML]
        VM[ViewModels]
        C[Converters]
    end

    subgraph Application["Application Layer"]
        AS[AppServices]
        AUTH[AuthService]
        STORE[ScanStore]
    end

    subgraph Domain["Service Layer"]
        SCAN[ScanViewModel orchestration]
        ENUM[SystemEnumerationService]
        ATK[AttackService]
        AI[OpenAIService]
        REM[RemediationService]
        RPT[ReportService]
        HIST[HistoryService]
        HARD[SystemHardeningService]
        SIM[SimulationService]
        GRAPH[AttackGraphEngine]
    end

    subgraph Data["Data Layer"]
        M[Models]
        CFG[ConfigService]
        FS[(Local JSON Files)]
    end

    V --> VM
    VM --> AS
    VM --> SCAN
    VM --> ENUM
    VM --> ATK
    VM --> AI
    VM --> REM
    VM --> RPT
    VM --> HIST
    VM --> HARD
    VM --> SIM
    AS --> STORE
    AS --> AUTH
    AI --> CFG
    STORE --> FS
    CFG --> FS
    HIST --> FS
    ENUM --> M
    ATK --> M
    SIM --> GRAPH
```

---

## MVVM Structure

### Views (`Views/`)

WinUI 3 `Page` and `Window` components. Data context is set in XAML or code-behind. Navigation is handled by `MainWindow` via `NavigationView` tags.

| View | Purpose |
|------|---------|
| `DashboardPage` | Landing, safety messaging, quick orientation |
| `ScanPage` | Port scan, results, AI analysis, remediation |
| `DeviceProfilePage` | Host profile and enumeration display |
| `AttackPage` | Authorized attack helper workflows |
| `RansomwarePage` | Subnet simulation for training |
| `ReportsPage` | Session and report management |
| `SettingsPage` | OpenAI API key configuration |
| `LoginWindow` | Optional authentication UI |

### ViewModels (`ViewModels/`)

Inherit from `ObservableObject`. Expose properties and `RelayCommand` / `AsyncRelayCommand` for UI binding.

| ViewModel | Responsibility |
|-----------|----------------|
| `ScanViewModel` | Scan lifecycle, risk stats, AI, remediation, reports |
| `DeviceViewModel` | Device profile rendering, deep scan, roadmap |
| `AttackViewModel` | Attack module UI state and logging |
| `RansomwareViewModel` | Subnet scan simulation |
| `ReportsViewModel` | Report listing |
| `ViewModelBase` | Shared base |

### Models (`Models/`)

Plain data objects serialized to JSON where needed.

| Model | Purpose |
|-------|---------|
| `ScanSession` | Target, timestamps, findings, enumeration metadata |
| `PortFinding` | Port, service, risk, recommendations |
| `DeviceProfile` | Host inventory presentation model |
| `AttackScenario` | AI-generated scenario structure |
| `HardeningSuggestion` | Remediation package with scripts |
| `AttackGraph` | Graph nodes for simulation |

### Core (`Core/`)

| Type | Purpose |
|------|---------|
| `ObservableObject` | `INotifyPropertyChanged` base |
| `RelayCommand` | Synchronous UI commands |
| `AsyncRelayCommand` | Async UI commands |

### Converters (`Converters/`)

XAML value converters for visibility, severity colors, formatting.

---

## Services Layer

| Service | Role |
|---------|------|
| `ScanStore` | ObservableCollection persistence (`history.json`) |
| `HistoryService` | Trend/history for port exposure over time |
| `ConfigService` | OpenAI API key in `%AppData%\CyberTool\config.json` |
| `SystemEnumerationService` | WMI, Nmap XML parsing, authenticated enumeration |
| `AttackService` | SMB credential testing helpers (authorized) |
| `OpenAIService` | Chat Completions API for analysis and scripts |
| `RemediationService` | AI or template-based fix generation |
| `ReportService` | Executive and technical report text |
| `SystemHardeningService` | Local registry/event log posture checks |
| `SimulationService` | Attack scenario packaging |
| `AttackGraphEngine` | Attack path graph construction |
| `NmapXmlImporter` | External scan import |
| `AuthService` | DEBUG-only demo authentication |

### AppServices

Static service locator for shared singletons:

```csharp
public static class AppServices
{
    public static ScanStore ScanStore { get; }
    public static AuthService AuthService { get; }
}
```

---

## Scanning Flow

```mermaid
sequenceDiagram
    participant User
    participant ScanPage
    participant ScanVM as ScanViewModel
    participant Enum as SystemEnumerationService
    participant Store as ScanStore
    participant Hist as HistoryService

    User->>ScanPage: Enter target, start scan
    ScanPage->>ScanVM: StartAsync()
    ScanVM->>ScanVM: TCP port probe per range
    ScanVM->>Enum: Optional WMI / Nmap enrichment
    Enum-->>ScanVM: Host metadata
    ScanVM->>ScanVM: Calculate risk & stats
    ScanVM->>Store: Add(ScanSession)
    ScanVM->>Hist: SaveScanAsync()
    ScanVM-->>ScanPage: Results + status
    ScanPage-->>User: Display findings
```

---

## OpenAI Integration Flow

```mermaid
sequenceDiagram
    participant User
    participant Settings
    participant Config as ConfigService
    participant ScanVM as ScanViewModel
    participant AI as OpenAIService
    participant API as OpenAI API

    User->>Settings: Enter API key
    Settings->>Config: Save to config.json
    User->>ScanVM: Request AI analysis
    ScanVM->>AI: AnalyzeScanResultsAsync()
    AI->>Config: Read API key
    alt Key present
        AI->>API: POST /v1/chat/completions
        API-->>AI: JSON scenarios
        AI-->>ScanVM: AttackScenario list
    else Key missing
        AI-->>ScanVM: Empty / fallback message
    end
```

**Privacy note:** Only user-initiated AI calls transmit data. Content is minimized (port/service summary). See [safety.md](safety.md).

---

## Data Storage

```mermaid
flowchart LR
    subgraph AppData["%AppData%\\CyberTool"]
        CFG[config.json]
        HIST[history.json]
    end

    subgraph LocalAppData["%LocalAppData%\\CyberTool"]
        ERR[errors.log]
        SCANH[History\\scan_history.json]
    end

    subgraph Desktop["User Desktop"]
        RPT[Executive_Report_*.txt]
        RPT2[Technical_Report_*.txt]
    end

    ConfigService --> CFG
    ScanStore --> HIST
    App --> ERR
    HistoryService --> SCANH
    ScanViewModel --> RPT
    ScanViewModel --> RPT2
```

All paths are gitignored. No cloud sync is built in.

---

## Navigation Architecture

```mermaid
flowchart TD
    MW[MainWindow]
    MW --> D[Dashboard]
    MW --> DP[Device Profile]
    MW --> S[Scan]
    MW --> R[Reports]
    MW --> A[Attack]
    MW --> RW[Ransomware]
    MW --> ST[Settings]
```

---

## Dependency Graph (Simplified)

```mermaid
graph TD
    ScanVM --> ScanStore
    ScanVM --> OpenAIService
    ScanVM --> RemediationService
    ScanVM --> SystemHardeningService
    ScanVM --> HistoryService
    OpenAIService --> ConfigService
    RemediationService --> OpenAIService
    DeviceVM --> ScanStore
    DeviceVM --> SystemEnumerationService
    AttackVM --> AttackService
    AttackVM --> SystemEnumerationService
    RansomwareVM --> RansomwareVM
    SimulationService --> AttackGraphEngine
```

---

## Future Plugin Architecture (Planned v1.2+)

```mermaid
flowchart TB
    subgraph Core["CyberTool Core"]
        HOST[Plugin Host]
        API[Plugin API Contract]
    end

    subgraph Plugins["Community Plugins"]
        P1[Custom Scan Check]
        P2[Compliance Rule Pack]
        P3[Report Exporter]
    end

    HOST --> API
    P1 --> API
    P2 --> API
    P3 --> API
```

Planned capabilities:

- `ICyberToolPlugin` interface for scan checks and report hooks
- Sandboxed execution with explicit permission declarations
- Offline rule packs without external API dependency

See [roadmap.md](roadmap.md).

---

## Technology Stack

| Component | Technology |
|-----------|------------|
| UI | WinUI 3, XAML |
| Runtime | .NET 8, Windows App SDK 1.8 |
| WMI | `System.Management` |
| HTTP | `HttpClient` (OpenAI) |
| Serialization | `System.Text.Json` |
| CI | GitHub Actions, `windows-latest` |

---

## Related Documents

- [usage.md](usage.md) — operator guide
- [safety.md](safety.md) — authorized use
- [roadmap.md](roadmap.md) — milestones
- [OPEN_SOURCE_HEALTH.md](OPEN_SOURCE_HEALTH.md) — project health assessment
