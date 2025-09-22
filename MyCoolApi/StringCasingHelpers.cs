using Humanizer;

namespace MyCoolApi;

public class ZeichenkettenHelfer
{
    public static string KonvertiereGroessKleinschreibung(string eingabe, string formatTyp)
    {
        if (string.IsNullOrWhiteSpace(eingabe))
            return eingabe;

        return formatTyp.ToLowerInvariant() switch
        {
            "pascal" or "pascalcase" => ZuPascalFormat(eingabe),
            "camel" or "camelcase" => ZuCamelFormat(eingabe),
            "snake" or "snake_case" => eingabe.Underscore(),
            "kebab" or "kebab-case" => ZuKebabFormat(eingabe),
            "sentence" or "sentencecase" => eingabe.Humanize(),
            "title" or "titlecase" => eingabe.Titleize(),
            "upper" or "uppercase" => eingabe.Underscore().ToUpperInvariant(),
            "lower" or "lowercase" => eingabe.ToLowerInvariant(),
            _ => throw new ArgumentException($"Nicht unterstützter Format-Typ: {formatTyp}. Unterstützte Typen: pascal, camel, snake, kebab, sentence, title, upper, lower")
        };
    }

    public static string ZuPascalFormat(string eingabe)
    {
        // Erst Bindestriche zu Unterstrichen normalisieren, dann Pascalize verwenden
        var normalisiert = eingabe.Replace("-", "_");
        return normalisiert.Pascalize();
    }

    public static string ZuCamelFormat(string eingabe)
    {
        // Erst Bindestriche zu Unterstrichen normalisieren, dann Camelize verwenden
        var normalisiert = eingabe.Replace("-", "_");
        return normalisiert.Camelize();
    }

    public static string ZuSchlangen_Format(string eingabe) => eingabe.Underscore();

    public static string ZuKebabFormat(string eingabe)
    {
        // Erst zu snake_case konvertieren, dann Unterstriche durch Bindestriche ersetzen
        return eingabe.Underscore().Replace("_", "-");
    }

    public static string ZuSatzFormat(string eingabe) => eingabe.Humanize();
    public static string ZuTitelFormat(string eingabe) => eingabe.Titleize();
    public static string ZuGrossschreibung(string eingabe) => eingabe.Underscore().ToUpperInvariant();
}