# MyCoolApi - Deutsche Projektanleitung

Diese Anleitung hilft Ihnen beim Verständnis und der Verwendung der MyCoolApi, die vollständig in deutscher Sprache implementiert ist.

## Projektübersicht

MyCoolApi ist eine .NET 9.0 ASP.NET Core Web API, die verschiedene mathematische Operationen, Begrüßungsfunktionen, Zeichenketten-Umwandlungen, Wettervorhersagen und Betriebssysteminformationen bereitstellt.

## Projektstruktur

```
MyCoolApi/                  # Haupt ASP.NET Core Web API (.NET 9.0)
├── Program.cs             # Haupteinstiegspunkt mit allen Endpunkt-Definitionen
├── MathHelfers.cs         # Mathematische Operationen (Addieren, Subtrahieren, etc.)
├── HelloBuilders.cs       # Begrüßungsfunktionalität
├── StringCasingHelpers.cs # Zeichenketten-Umwandlung
└── Properties/            # Konfigurationsdateien

MyCoolApi.Tests/           # Testprojekt mit Microsoft Testing Platform (.NET 9.0)
├── MathTests.cs          # Tests für mathematische Operationen  
├── HelloTests.cs         # Tests für Begrüßungsendpunkte
├── StringCasingTests.cs  # Tests für Zeichenketten-Umwandlung
└── ...                   # Weitere Testdateien
```

## Verfügbare API-Endpunkte

### Mathematische Operationen
- `GET /addieren/{zahl1},{zahl2}` - Addiert zwei Zahlen
- `GET /multiplizieren/{zahl1},{zahl2}` - Multipliziert zwei Zahlen  
- `GET /halbieren/{zahl1}` - Teilt Zahl durch 2
- `GET /verdoppeln/{zahl1}` - Multipliziert Zahl mit 2
- `GET /fibonacci/{zahl}` - Berechnet Fibonacci-Folge
- `GET /fakultaet/{zahl}` - Berechnet Fakultät einer Zahl

### Begrüßungen
- `GET /hallo/{name}` - Sagt Hallo mit korrekter Namensschreibung
- `GET /tschuess/{name}` - Sagt Tschüss mit korrekter Namensschreibung

### System-Information
- `GET /umgebung` - Gibt Betriebssystem-Beschreibung zurück
- `GET /wettervorhersage` - Gibt Beispiel-Wetterdaten zurück

### Zeichenketten-Umwandlung
- `GET /schreibweise/{eingabe}/{typ}` - Wandelt Zeichenketten-Schreibweise um
  - Unterstützte Typen: pascal, camel, snake, kebab, sentence, title, upper, lower

## Entwicklung

### Voraussetzungen
- .NET 10.x preview SDK (für Entwicklung)
- Das Projekt zielt auf .NET 9.0 Runtime ab

### Befehle zum Erstellen und Testen
- **Wiederherstellen**: `dotnet restore`
- **Erstellen**: `dotnet build --configuration Release --no-restore`
- **Testen**: `dotnet test --configuration Release --no-build`
- **Ausführen**: `dotnet run --project MyCoolApi --launch-profile http`

### Anwendung ausführen
- Entwicklung HTTP: `dotnet run --project MyCoolApi --launch-profile http`
- Entwicklung HTTPS: `dotnet run --project MyCoolApi --launch-profile https`
- Anwendung läuft auf http://localhost:5250 (HTTP) und https://localhost:7271 (HTTPS)
- Swagger UI verfügbar unter `/swagger` Endpunkt

## Beispiel-Anfragen

```http
# Mathematische Operationen
GET http://localhost:5250/addieren/5,3          # → 8
GET http://localhost:5250/multiplizieren/4,5    # → 20
GET http://localhost:5250/halbieren/12          # → 6.0
GET http://localhost:5250/fibonacci/5           # → [0,1,1,2,3]

# Begrüßungen
GET http://localhost:5250/hallo/johann          # → "Hallo Johann"
GET http://localhost:5250/tschuess/maria        # → "Tschüss Maria!"

# Zeichenketten-Umwandlung
GET http://localhost:5250/schreibweise/hello_world/pascal    # → "HelloWorld"
GET http://localhost:5250/schreibweise/HelloWorld/snake      # → "hello_world"

# System-Information
GET http://localhost:5250/umgebung              # → OS-Beschreibung
GET http://localhost:5250/wettervorhersage      # → Wetter-Array
```

## Wichtige Klassen und Methoden

### MathHelfers (Mathematische Hilfsfunktionen)
- `Addieren(zahl1, zahl2)` - Addiert zwei Zahlen
- `Subtrahieren(zahl1, zahl2)` - Subtrahiert zwei Zahlen
- `Multiplizieren(zahl1, zahl2)` - Multipliziert zwei Zahlen
- `Fibonacci(zahl)` - Berechnet Fibonacci-Folge
- `Fakultaet(zahl)` - Berechnet Fakultät
- `Halbieren(zahl)` - Teilt durch 2
- `Verdoppeln(zahl)` - Multipliziert mit 2

### HelloBuilders (Begrüßungsklasse)
- `SagHallo(name)` - Erstellt Hallo-Begrüßung
- `SagTschuess(name)` - Erstellt Tschüss-Begrüßung

### StringCasingHelpers (Zeichenketten-Hilfsfunktionen)
- `KonvertiereSchreibweise(eingabe, typ)` - Hauptmethode für Umwandlung
- `ZuPascalCase(eingabe)` - Wandelt zu PascalCase um
- `ZuCamelCase(eingabe)` - Wandelt zu camelCase um
- `ZuSnakeCase(eingabe)` - Wandelt zu snake_case um
- `ZuKebabCase(eingabe)` - Wandelt zu kebab-case um

## Fehlerbehebung

### Häufige Probleme
- **NuGet SSL-Fehler**: Bekanntes Problem in bestimmten Umgebungen
- **Port-Konflikte**: Verwenden Sie `dotnet run --urls "http://localhost:5251"`
- **Fehlende Abhängigkeiten**: Überprüfen Sie PackageReference-Einträge

### Tests
- Vollständige Test-Suite: `dotnet test`
- Spezifische Test-Klasse: `dotnet test --filter MathTests`
- Tests verwenden Microsoft Testing Platform (MTP)

## Lizenz

Dieses Projekt steht unter der MIT-Lizenz. Siehe LICENSE.txt für Details.