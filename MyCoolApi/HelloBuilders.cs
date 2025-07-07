namespace MyCoolApi;


public class HelloBuilders {
    public static string SayHello(string name)
        => $"Hello {System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}";
    
    public static string SayGoodbye(string name)
        => $"Bye {System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}!";
    
    public static string SayCaps(string text)
        => text.ToUpper();
}