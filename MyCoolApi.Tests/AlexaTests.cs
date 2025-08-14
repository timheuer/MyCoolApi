using System.Text;
using System.Text.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

namespace MyCoolApi.Tests;

[TestClass]
public class AlexaTests
{
    [TestMethod]
    public async Task Alexa_Endpoint_Handles_Launch_Request()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var launchRequest = new SkillRequest
        {
            Version = "1.0",
            Request = new LaunchRequest(),
            Session = new Session
            {
                SessionId = "test-session",
                Application = new Application { ApplicationId = "test-app" },
                New = true
            }
        };

        var json = JsonSerializer.Serialize(launchRequest, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/alexa", content);

        Assert.IsTrue(response.IsSuccessStatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var skillResponse = JsonSerializer.Deserialize<SkillResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(skillResponse);
        Assert.IsNotNull(skillResponse.Response);
        Assert.IsNotNull(skillResponse.Response.OutputSpeech);
    }

    [TestMethod]
    public async Task Alexa_Endpoint_Handles_Add_Numbers_Intent()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var intentRequest = new SkillRequest
        {
            Version = "1.0",
            Request = new IntentRequest
            {
                Intent = new Intent
                {
                    Name = "AddNumbersIntent",
                    Slots = new Dictionary<string, Slot>
                    {
                        ["FirstNumber"] = new Slot { Name = "FirstNumber", Value = "5" },
                        ["SecondNumber"] = new Slot { Name = "SecondNumber", Value = "3" }
                    }
                }
            },
            Session = new Session
            {
                SessionId = "test-session",
                Application = new Application { ApplicationId = "test-app" },
                New = false
            }
        };

        var json = JsonSerializer.Serialize(intentRequest, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/alexa", content);

        Assert.IsTrue(response.IsSuccessStatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var skillResponse = JsonSerializer.Deserialize<SkillResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(skillResponse);
        Assert.IsNotNull(skillResponse.Response);
        Assert.IsNotNull(skillResponse.Response.OutputSpeech);
        
        // Verify the response contains the expected calculation
        if (skillResponse.Response.OutputSpeech is PlainTextOutputSpeech speech)
        {
            Assert.IsTrue(speech.Text.Contains("8"));
        }
    }

    [TestMethod]
    public async Task Alexa_Endpoint_Handles_Say_Hello_Intent()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var intentRequest = new SkillRequest
        {
            Version = "1.0",
            Request = new IntentRequest
            {
                Intent = new Intent
                {
                    Name = "SayHelloIntent",
                    Slots = new Dictionary<string, Slot>
                    {
                        ["Name"] = new Slot { Name = "Name", Value = "Tim" }
                    }
                }
            },
            Session = new Session
            {
                SessionId = "test-session",
                Application = new Application { ApplicationId = "test-app" },
                New = false
            }
        };

        var json = JsonSerializer.Serialize(intentRequest, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/alexa", content);

        Assert.IsTrue(response.IsSuccessStatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var skillResponse = JsonSerializer.Deserialize<SkillResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(skillResponse);
        Assert.IsNotNull(skillResponse.Response);
        Assert.IsNotNull(skillResponse.Response.OutputSpeech);
        
        // Verify the response contains the expected greeting
        if (skillResponse.Response.OutputSpeech is PlainTextOutputSpeech speech)
        {
            Assert.IsTrue(speech.Text.Contains("Hello Tim"));
        }
    }

    [TestMethod]
    public async Task Alexa_Endpoint_Handles_Invalid_Request()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var invalidJson = "{ invalid json }";
        var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/alexa", content);

        Assert.IsFalse(response.IsSuccessStatusCode);
    }

    [TestMethod]
    public void AlexaHelpers_HandleLaunchRequest_ReturnsWelcomeMessage()
    {
        var request = new SkillRequest
        {
            Version = "1.0",
            Request = new LaunchRequest(),
            Session = new Session
            {
                SessionId = "test-session",
                Application = new Application { ApplicationId = "test-app" },
                New = true
            }
        };

        var response = AlexaHelpers.HandleAlexaRequest(request);

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Response);
        Assert.IsNotNull(response.Response.OutputSpeech);
        Assert.IsTrue(response.Response.ShouldEndSession == false); // Should keep session open
    }

    [TestMethod]
    public void AlexaHelpers_HandleStopIntent_EndsSession()
    {
        var request = new SkillRequest
        {
            Version = "1.0",
            Request = new IntentRequest
            {
                Intent = new Intent { Name = "AMAZON.StopIntent" }
            },
            Session = new Session
            {
                SessionId = "test-session",
                Application = new Application { ApplicationId = "test-app" },
                New = false
            }
        };

        var response = AlexaHelpers.HandleAlexaRequest(request);

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Response);
        Assert.IsNotNull(response.Response.OutputSpeech);
        Assert.IsTrue(response.Response.ShouldEndSession == true); // Should end session
    }
}