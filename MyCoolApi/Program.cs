using System.Runtime.InteropServices;
using MyCoolApi;

var anwendungsErsteller = WebApplication.CreateBuilder(args);

anwendungsErsteller.Services.AddEndpointsApiExplorer();
anwendungsErsteller.Services.AddSwaggerGen();

var anwendung = anwendungsErsteller.Build();

// HTTP-Request-Pipeline konfigurieren
if (anwendung.Environment.IsDevelopment())
{
    anwendung.UseSwagger();
    anwendung.UseSwaggerUI();
}

anwendung.UseHttpsRedirection();

var zusammenfassungen = new[]
{
    "Frierend", "Kühl", "Kalt", "Angenehm", "Mild", "Warm", "Balmy", "Heiß", "Schwül", "Brennend"
};

anwendung.MapGet("/add/{zahl1},{zahl2}", (int zahl1, int zahl2) =>
{

    var ergebnis = MathematikHelfer.Addieren(zahl1, zahl2);
    return ergebnis;
});

anwendung.MapGet("/fibonacci/{zahl}", (int zahl) =>
{
    return MathematikHelfer.Fibonacci(zahl);
});

anwendung.MapGet("/multiply/{zahl1},{zahl2}", (int zahl1, int zahl2) =>
{
    return MathematikHelfer.Multiplizieren(zahl1, zahl2);
});

anwendung.MapGet("/doubleit/{zahl1}", (int zahl1) =>
{
    return MathematikHelfer.VerdoppelnVon(zahl1);
});

anwendung.MapGet("/hello/{name}", (string name) =>
{
    return HalloErsteller.SageHallo(name);
});

anwendung.MapGet("/bye/{name}", (string name) =>
{
    return HalloErsteller.SageTschuess(name);
});

anwendung.MapGet("/env", () =>
{

    return RuntimeInformation.OSDescription;
});

anwendung.MapGet("/divide/{zahl1}", (int zahl1) =>
{
    return MathematikHelfer.HalbierenVon(zahl1);
});

anwendung.MapGet("/casing/{eingabe}/{formatTyp}", (string eingabe, string formatTyp) =>
{
    try
    {
        var ergebnis = ZeichenkettenHelfer.KonvertiereGroessKleinschreibung(eingabe, formatTyp);
        return Results.Ok(ergebnis);
    }
    catch (ArgumentException ausnahme)
    {
        return Results.BadRequest(ausnahme.Message);
    }
});

anwendung.MapGet("/weatherforecast", () =>
{
    var vorhersage = Enumerable.Range(1, 5).Select(index =>
        new WetterVorhersage
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            zusammenfassungen[Random.Shared.Next(zusammenfassungen.Length)]
        ))
        .ToArray();
    return vorhersage;
})
.WithName("GetWeatherForecast");

anwendung.Run();

public record WetterVorhersage(DateOnly Datum, int TemperaturC, string? Zusammenfassung)
{
    public int TemperaturF => 32 + (int)(TemperaturC / 0.5556);
}

public partial class Program { }