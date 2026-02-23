namespace MyCoolApi;

public static class StringFormatHelpers
{
    public static string FormatWithSpacesAndExclamations(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder();
        
        for (int i = 0; i < input.Length; i++)
        {
            result.Append(input[i]);
            
            int position = i + 1;
            
            // Add exclamation point every second character
            if (position % 2 == 0 && position < input.Length)
            {
                result.Append('!');
            }
            
            // Add space every third character
            if (position % 3 == 0 && position < input.Length)
            {
                result.Append(' ');
            }
        }
        
        return result.ToString();
    }
}
