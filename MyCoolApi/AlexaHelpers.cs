using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace MyCoolApi;

public class AlexaHelpers
{
    public static SkillResponse HandleAlexaRequest(SkillRequest request)
    {
        var requestType = request.GetRequestType();
        
        switch (requestType)
        {
            case typeof(LaunchRequest):
                return HandleLaunchRequest(request);
            case typeof(IntentRequest):
                return HandleIntentRequest(request as IntentRequest);
            case typeof(SessionEndedRequest):
                return HandleSessionEndedRequest();
            default:
                return ResponseBuilder.Tell("I don't understand that request.");
        }
    }

    private static SkillResponse HandleLaunchRequest(SkillRequest request)
    {
        var speech = new PlainTextOutputSpeech
        {
            Text = "Welcome to My Cool API! You can ask me to do math, say hello, or get the weather forecast."
        };

        var reprompt = new PlainTextOutputSpeech
        {
            Text = "What would you like me to help you with?"
        };

        return ResponseBuilder.Ask(speech, new Reprompt { OutputSpeech = reprompt });
    }

    private static SkillResponse HandleIntentRequest(IntentRequest intentRequest)
    {
        switch (intentRequest.Intent.Name)
        {
            case "AddNumbersIntent":
                return HandleAddNumbersIntent(intentRequest);
            case "MultiplyNumbersIntent":
                return HandleMultiplyNumbersIntent(intentRequest);
            case "SayHelloIntent":
                return HandleSayHelloIntent(intentRequest);
            case "GetWeatherIntent":
                return HandleGetWeatherIntent();
            case "AMAZON.HelpIntent":
                return HandleHelpIntent();
            case "AMAZON.StopIntent":
            case "AMAZON.CancelIntent":
                return HandleStopIntent();
            default:
                return ResponseBuilder.Tell("I don't know how to handle that intent.");
        }
    }

    private static SkillResponse HandleAddNumbersIntent(IntentRequest intentRequest)
    {
        try
        {
            var firstNumber = int.Parse(intentRequest.Intent.Slots["FirstNumber"].Value);
            var secondNumber = int.Parse(intentRequest.Intent.Slots["SecondNumber"].Value);
            var result = MathHelpers.Add(firstNumber, secondNumber);
            
            var speech = $"The sum of {firstNumber} and {secondNumber} is {result}";
            return ResponseBuilder.Tell(speech);
        }
        catch
        {
            return ResponseBuilder.Tell("I couldn't understand the numbers. Please try again.");
        }
    }

    private static SkillResponse HandleMultiplyNumbersIntent(IntentRequest intentRequest)
    {
        try
        {
            var firstNumber = int.Parse(intentRequest.Intent.Slots["FirstNumber"].Value);
            var secondNumber = int.Parse(intentRequest.Intent.Slots["SecondNumber"].Value);
            var result = MathHelpers.Multiply(firstNumber, secondNumber);
            
            var speech = $"{firstNumber} times {secondNumber} equals {result}";
            return ResponseBuilder.Tell(speech);
        }
        catch
        {
            return ResponseBuilder.Tell("I couldn't understand the numbers. Please try again.");
        }
    }

    private static SkillResponse HandleSayHelloIntent(IntentRequest intentRequest)
    {
        var name = intentRequest.Intent.Slots?["Name"]?.Value ?? "there";
        var greeting = HelloBuilders.SayHello(name);
        return ResponseBuilder.Tell(greeting);
    }

    private static SkillResponse HandleGetWeatherIntent()
    {
        var speech = "I can provide weather forecasts through the API endpoints, but I don't have current weather data in this skill.";
        return ResponseBuilder.Tell(speech);
    }

    private static SkillResponse HandleHelpIntent()
    {
        var speech = "You can ask me to add or multiply numbers, say hello to someone, or get help. What would you like to do?";
        var reprompt = "What can I help you with?";
        return ResponseBuilder.Ask(speech, new Reprompt { OutputSpeech = new PlainTextOutputSpeech { Text = reprompt } });
    }

    private static SkillResponse HandleStopIntent()
    {
        return ResponseBuilder.Tell("Goodbye!");
    }

    private static SkillResponse HandleSessionEndedRequest()
    {
        return ResponseBuilder.Empty();
    }
}