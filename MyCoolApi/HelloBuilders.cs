namespace MyCoolApi;


public class HelloBuilders {
    public static string SagHallo(string name)
        => $"Hallo {System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}";
    
    public static string SagTschuess(string name)
        => $"Tschüss {System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}!";
}