# MyCoolApi

Eine .NET 9.0 ASP.NET Core Web API Anwendung, die mathematische Operationen, Grüße, Zeichenfolgen-Formatierung, Wettervorhersage und Betriebssystem-Informationen bereitstellt.

## Funktionen

### Mathematische Operationen
- **Addition**: `GET /add/{zahl1},{zahl2}` - Addiert zwei Zahlen
- **Multiplikation**: `GET /multiply/{zahl1},{zahl2}` - Multipliziert zwei Zahlen  
- **Division**: `GET /divide/{zahl1}` - Teilt eine Zahl durch 2
- **Verdopplung**: `GET /doubleit/{zahl1}` - Multipliziert eine Zahl mit 2
- **Fibonacci**: `GET /fibonacci/{zahl}` - Berechnet die Fibonacci-Folge

### Grußfunktionen
- **Hallo**: `GET /hello/{name}` - Sagt Hallo mit korrekter Groß-/Kleinschreibung
- **Tschüss**: `GET /bye/{name}` - Sagt Tschüss mit korrekter Groß-/Kleinschreibung

### Zeichenfolgen-Formatierung
- **Groß-/Kleinschreibung**: `GET /casing/{eingabe}/{formatTyp}` - Konvertiert Zeichenfolgen-Formatierung
  - Unterstützte Formate: pascal, camel, snake, kebab, sentence, title, upper, lower

### Systeminformationen
- **Umgebung**: `GET /env` - Gibt die Betriebssystem-Beschreibung zurück
- **Wettervorhersage**: `GET /weatherforecast` - Gibt Beispiel-Wetterdaten zurück

## Verwendung

### Lokale Entwicklung
```bash
# Abhängigkeiten wiederherstellen
dotnet restore

# Anwendung erstellen
dotnet build --configuration Release --no-restore

# Tests ausführen
dotnet test --configuration Release --no-build

# Anwendung starten (HTTP)
dotnet run --project MyCoolApi --launch-profile http
```

Die Anwendung läuft auf:
- HTTP: http://localhost:5250
- HTTPS: https://localhost:7271

### Swagger UI
Die API-Dokumentation ist verfügbar unter: `http://localhost:5250/swagger`

## Beispiele

```bash
# Mathematische Operationen
curl http://localhost:5250/add/5,3          # Gibt 8 zurück
curl http://localhost:5250/multiply/4,5     # Gibt 20 zurück
curl http://localhost:5250/fibonacci/5      # Gibt [0,1,1,2,3] zurück

# Grüße
curl http://localhost:5250/hello/peter      # Gibt "Hallo Peter" zurück
curl http://localhost:5250/bye/anna         # Gibt "Tschüss Anna!" zurück

# Zeichenfolgen-Formatierung
curl http://localhost:5250/casing/hallo_welt/pascal  # Gibt "HalloWelt" zurück
curl http://localhost:5250/casing/HalloWelt/snake    # Gibt "hallo_welt" zurück
```

## Technologie-Stack

- .NET 9.0
- ASP.NET Core Web API
- Humanizer Library für Zeichenfolgen-Manipulation
- Swagger/OpenAPI für API-Dokumentation
- Microsoft Testing Platform für Tests

## Projektstruktur

```
MyCoolApi/                  # Haupt-Web-API-Projekt
├── Program.cs             # Anwendungs-Einstiegspunkt mit API-Endpunkten
├── MathematikHelfer.cs    # Mathematische Operationen
├── HalloErsteller.cs      # Grußfunktionalität
└── ZeichenkettenHelfer.cs # Zeichenfolgen-Formatierung

MyCoolApi.Tests/           # Testprojekt
├── MathematikTests.cs     # Tests für mathematische Operationen
├── HalloTests.cs          # Tests für Grußendpunkte
└── ZeichenkettenTests.cs  # Tests für Zeichenfolgen-Formatierung

infra/                     # Azure-Bereitstellungs-Infrastruktur
docs/                      # Zusätzliche Dokumentation
```

## Entwicklung

### Build und Test
Die Anwendung verwendet das Microsoft Testing Platform (MTP) für Tests und umfasst sowohl Unit- als auch Integrationstests.

### CI/CD
GitHub Actions Workflow verfügbar in `.github/workflows/MyCoolApi.yaml` für automatische Builds und Tests.

### Docker-Unterstützung
Die Anwendung unterstützt Docker-Containerisierung für einfache Bereitstellung.

## Lizenz

MIT License - siehe LICENSE.txt für Details.