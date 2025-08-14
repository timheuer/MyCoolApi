using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace MyCoolApi;

public class AlexaHelpers
{
    /// <summary>
    /// Creates a simple Alexa speech response
    /// </summary>
    /// <param name="speechText">The text for Alexa to speak</param>
    /// <param name="shouldEndSession">Whether to end the session after speaking</param>
    /// <returns>Alexa skill response</returns>
    public static SkillResponse CreateSpeechResponse(string speechText, bool shouldEndSession = true)
    {
        var speech = new PlainTextOutputSpeech { Text = speechText };
        
        return ResponseBuilder.Tell(speech);
    }

    /// <summary>
    /// Creates an Alexa response that asks a question and waits for user input
    /// </summary>
    /// <param name="speechText">The question to ask</param>
    /// <param name="repromptText">Text to use if user doesn't respond</param>
    /// <returns>Alexa skill response</returns>
    public static SkillResponse CreateAskResponse(string speechText, string repromptText)
    {
        var speech = new PlainTextOutputSpeech { Text = speechText };
        var reprompt = new PlainTextOutputSpeech { Text = repromptText };

        return ResponseBuilder.Ask(speech, new Reprompt { OutputSpeech = reprompt });
    }

    /// <summary>
    /// Processes a simple intent and returns appropriate response
    /// </summary>
    /// <param name="intentName">Name of the intent to process</param>
    /// <param name="userInput">Optional user input value</param>
    /// <returns>Response text for the intent</returns>
    public static string ProcessIntent(string intentName, string? userInput = null)
    {
        return intentName.ToLowerInvariant() switch
        {
            "hello" or "helloworld" => $"Hello there! Welcome to My Cool API{(userInput != null ? $", {userInput}" : "")}!",
            "mathhelp" => "I can help you with math operations like addition, multiplication, and Fibonacci sequences!",
            "goodbye" or "stop" or "cancel" => "Goodbye! Thanks for using My Cool API!",
            "help" => "You can ask me about math operations, say hello, or ask for help with various features.",
            _ => "I'm not sure how to help with that. Try saying 'help' to learn what I can do."
        };
    }
}