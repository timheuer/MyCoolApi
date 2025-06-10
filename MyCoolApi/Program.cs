using System.Runtime.InteropServices;
using MyCoolApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/add/{num1},{num2}", (int num1, int num2) =>
{

    var result = MathHelpers.Add(num1, num2);
    return result;
});

app.MapGet("/fibonacci/{num}", (int num) =>
{
    return MathHelpers.Fibonacci(num);
});

app.MapGet("/multiply/{num1},{num2}", (int num1, int num2) =>
{
    return MathHelpers.Multiply(num1, num2);
});

app.MapGet("/doubleit/{num1}", (int num1) =>
{
    return MathHelpers.MultiplyByTwo(num1);
});

app.MapGet("/hello/{name}", (string name) =>
{
    return HelloBuilders.SayHello(name);
});

app.MapGet("/bye/{name}", (string name) =>
{
    return HelloBuilders.SayGoodbye(name);
});

app.MapGet("/env", () =>
{

    return RuntimeInformation.OSDescription;
});

app.MapGet("/divide/{num1}", (int num1) =>
{
    return MathHelpers.DivideInHalf(num1);
});

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }