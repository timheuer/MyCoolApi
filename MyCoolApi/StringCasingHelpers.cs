using Humanizer;

namespace MyCoolApi;

public class StringCasingHelpers
{
    public static string KonvertiereSchreibweise(string eingabe, string schreibweiseTyp)
    {
        if (string.IsNullOrWhiteSpace(eingabe))
            return eingabe;

        return schreibweiseTyp.ToLowerInvariant() switch
        {
            "pascal" or "pascalcase" => ZuPascalCase(eingabe),
            "camel" or "camelcase" => ZuCamelCase(eingabe),
            "snake" or "snake_case" => eingabe.Underscore(),
            "kebab" or "kebab-case" => ZuKebabCase(eingabe),
            "sentence" or "sentencecase" => eingabe.Humanize(),
            "title" or "titlecase" => eingabe.Titleize(),
            "upper" or "uppercase" => eingabe.Underscore().ToUpperInvariant(),
            "lower" or "lowercase" => eingabe.ToLowerInvariant(),
            _ => throw new ArgumentException($"Nicht unterstützter Schreibweise-Typ: {schreibweiseTyp}. Unterstützte Typen: pascal, camel, snake, kebab, sentence, title, upper, lower")
        };
    }

    public static string ZuPascalCase(string eingabe)
    {
        // Zuerst Bindestriche zu Unterstrichen normalisieren, dann Pascalize verwenden
        var normalisiert = eingabe.Replace("-", "_");
        return normalisiert.Pascalize();
    }

    public static string ZuCamelCase(string eingabe)
    {
        // Zuerst Bindestriche zu Unterstrichen normalisieren, dann Camelize verwenden
        var normalisiert = eingabe.Replace("-", "_");
        return normalisiert.Camelize();
    }

    public static string ZuSnakeCase(string eingabe) => eingabe.Underscore();

    public static string ZuKebabCase(string eingabe)
    {
        // Zuerst zu snake_case konvertieren, dann Unterstriche mit Bindestrichen ersetzen
        return eingabe.Underscore().Replace("_", "-");
    }

    public static string ZuSentenceCase(string eingabe) => eingabe.Humanize();
    public static string ZuTitleCase(string eingabe) => eingabe.Titleize();
    public static string ZuUpperCase(string eingabe) => eingabe.Underscore().ToUpperInvariant();
}