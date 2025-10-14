using System.Runtime.InteropServices;
using MyCoolApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var anwendung = builder.Build();

// HTTP-Request-Pipeline konfigurieren.
if (anwendung.Environment.IsDevelopment())
{
    anwendung.UseSwagger();
    anwendung.UseSwaggerUI();
}

anwendung.UseHttpsRedirection();

var wetterZusammenfassungen = new[]
{
    "Eiskalt", "Frisch", "Kühl", "Mild", "Angenehm", "Warm", "Heiss", "Sehr heiss", "Schwül", "Glühend"
};

anwendung.MapGet("/addieren/{zahl1},{zahl2}", (int zahl1, int zahl2) =>
{

    var ergebnis = MathHelfers.Addieren(zahl1, zahl2);
    return ergebnis;
});

anwendung.MapGet("/fibonacci/{zahl}", (int zahl) =>
{
    return MathHelfers.Fibonacci(zahl);
});

anwendung.MapGet("/multiplizieren/{zahl1},{zahl2}", (int zahl1, int zahl2) =>
{
    return MathHelfers.Multiplizieren(zahl1, zahl2);
});

anwendung.MapGet("/verdoppeln/{zahl1}", (int zahl1) =>
{
    return MathHelfers.Verdoppeln(zahl1);
});

anwendung.MapGet("/hallo/{name}", (string name) =>
{
    return HelloBuilders.SagHallo(name);
});

anwendung.MapGet("/tschuess/{name}", (string name) =>
{
    return HelloBuilders.SagTschuess(name);
});

anwendung.MapGet("/umgebung", () =>
{

    return RuntimeInformation.OSDescription;
});

anwendung.MapGet("/halbieren/{zahl1}", (int zahl1) =>
{
    return MathHelfers.Halbieren(zahl1);
});

anwendung.MapGet("/schreibweise/{eingabe}/{schreibweiseTyp}", (string eingabe, string schreibweiseTyp) =>
{
    try
    {
        var ergebnis = StringCasingHelpers.KonvertiereSchreibweise(eingabe, schreibweiseTyp);
        return Results.Ok(ergebnis);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

anwendung.MapGet("/wettervorhersage", () =>
{
    var vorhersage = Enumerable.Range(1, 5).Select(index =>
        new Wettervorhersage
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            wetterZusammenfassungen[Random.Shared.Next(wetterZusammenfassungen.Length)]
        ))
        .ToArray();
    return vorhersage;
})
.WithName("HoleWettervorhersage");

anwendung.Run();

public record Wettervorhersage(DateOnly Datum, int TemperaturC, string? Zusammenfassung)
{
    public int TemperaturF => 32 + (int)(TemperaturC / 0.5556);
}

public partial class Program { }