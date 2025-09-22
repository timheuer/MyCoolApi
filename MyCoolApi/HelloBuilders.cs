namespace MyCoolApi;


public class HalloErsteller {
    public static string SageHallo(string name)
        => $"Hallo {System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}";
    
    public static string SageTschuess(string name)
        => $"Tschüss {System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}!";
}