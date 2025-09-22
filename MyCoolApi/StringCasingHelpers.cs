using Humanizer;

namespace MyCoolApi;

public class StringCasingHelpers
{
    public static string ConvertCase(string input, string caseType)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return caseType.ToLowerInvariant() switch
        {
            "pascal" or "pascalcase" => ToPascalCase(input),
            "camel" or "camelcase" => ToCamelCase(input),
            "snake" or "snake_case" => input.Underscore(),
            "kebab" or "kebab-case" => ToKebabCase(input),
            "sentence" or "sentencecase" => input.Humanize(),
            "title" or "titlecase" => input.Titleize(),
            "upper" or "uppercase" => input.Underscore().ToUpperInvariant(),
            "lower" or "lowercase" => input.ToLowerInvariant(),
            _ => throw new ArgumentException($"Unsupported case type: {caseType}. Supported types: pascal, camel, snake, kebab, sentence, title, upper, lower")
        };
    }

    public static string ToPascalCase(string input)
    {
        // First normalize hyphens to underscores, then use Pascalize
        var normalized = input.Replace("-", "_");
        return normalized.Pascalize();
    }

    public static string ToCamelCase(string input)
    {
        // First normalize hyphens to underscores, then use Camelize
        var normalized = input.Replace("-", "_");
        return normalized.Camelize();
    }

    public static string ToSnakeCase(string input) => input.Underscore();

    public static string ToKebabCase(string input)
    {
        // Convert to snake_case first, then replace underscores with hyphens
        return input.Underscore().Replace("_", "-");
    }

    public static string ToSentenceCase(string input) => input.Humanize();
    public static string ToTitleCase(string input) => input.Titleize();
    public static string ToUpperCase(string input) => input.Underscore().ToUpperInvariant();
}