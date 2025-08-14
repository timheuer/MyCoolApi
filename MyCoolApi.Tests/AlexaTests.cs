using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;

namespace MyCoolApi.Tests;

[TestClass]
public class AlexaTests
{
    // Unit tests for AlexaHelpers methods
    [TestMethod]
    public void CreateSpeechResponse_ReturnsValidResponse()
    {
        var response = AlexaHelpers.CreateSpeechResponse("Hello World");
        
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Response);
        Assert.IsNotNull(response.Response.OutputSpeech);
        Assert.IsInstanceOfType(response.Response.OutputSpeech, typeof(PlainTextOutputSpeech));
        
        var speech = response.Response.OutputSpeech as PlainTextOutputSpeech;
        Assert.AreEqual("Hello World", speech?.Text);
        Assert.IsTrue(response.Response.ShouldEndSession);
    }

    [TestMethod]
    public void CreateAskResponse_ReturnsValidResponse()
    {
        var response = AlexaHelpers.CreateAskResponse("What's your name?", "Please tell me your name.");
        
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Response);
        Assert.IsNotNull(response.Response.OutputSpeech);
        Assert.IsNotNull(response.Response.Reprompt);
        Assert.IsFalse(response.Response.ShouldEndSession);
        
        var speech = response.Response.OutputSpeech as PlainTextOutputSpeech;
        Assert.AreEqual("What's your name?", speech?.Text);
    }

    [TestMethod]
    public void ProcessIntent_HelloIntent_ReturnsGreeting()
    {
        var result = AlexaHelpers.ProcessIntent("hello");
        Assert.AreEqual("Hello there! Welcome to My Cool API!", result);
    }

    [TestMethod]
    public void ProcessIntent_HelloWithInput_ReturnsPersonalizedGreeting()
    {
        var result = AlexaHelpers.ProcessIntent("hello", "Alice");
        Assert.AreEqual("Hello there! Welcome to My Cool API, Alice!", result);
    }

    [TestMethod]
    public void ProcessIntent_MathHelp_ReturnsHelpText()
    {
        var result = AlexaHelpers.ProcessIntent("mathhelp");
        Assert.AreEqual("I can help you with math operations like addition, multiplication, and Fibonacci sequences!", result);
    }

    [TestMethod]
    public void ProcessIntent_Goodbye_ReturnsGoodbye()
    {
        var result = AlexaHelpers.ProcessIntent("goodbye");
        Assert.AreEqual("Goodbye! Thanks for using My Cool API!", result);
    }

    [TestMethod]
    public void ProcessIntent_Help_ReturnsHelp()
    {
        var result = AlexaHelpers.ProcessIntent("help");
        Assert.AreEqual("You can ask me about math operations, say hello, or ask for help with various features.", result);
    }

    [TestMethod]
    public void ProcessIntent_UnknownIntent_ReturnsDefaultResponse()
    {
        var result = AlexaHelpers.ProcessIntent("unknown");
        Assert.AreEqual("I'm not sure how to help with that. Try saying 'help' to learn what I can do.", result);
    }

    // Integration tests for API endpoints
    [TestMethod]
    public async Task AlexaIntent_HelloEndpoint_ReturnsValidResponse()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/alexa/intent/hello");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains("Hello there! Welcome to My Cool API!"));
    }

    [TestMethod]
    public async Task AlexaIntent_MathHelpEndpoint_ReturnsValidResponse()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/alexa/intent/mathhelp");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains("math operations"));
    }

    [TestMethod]
    public async Task AlexaIntent_WithUserInput_ReturnsPersonalizedResponse()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/alexa/intent/hello?userInput=Tim");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains("Tim"));
    }

    [TestMethod]
    public async Task AlexaSkill_InvalidRequest_ReturnsBadRequest()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.PostAsync("/alexa/skill", new StringContent("", Encoding.UTF8, "application/json"));
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}