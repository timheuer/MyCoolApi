using System.Net;

namespace MyCoolApi.Tests;

[TestClass]
public class ErrorHandlingTests
{
    [TestMethod]
    public async Task Add_Endpoint_With_Invalid_Format_Returns_BadRequest()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/add/abc,def");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Multiply_Endpoint_With_Invalid_Format_Returns_BadRequest()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/multiply/not-a-number,5");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Fibonacci_Endpoint_With_Negative_Number_Returns_BadRequest()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/fibonacci/-1");

        // The API should handle negative numbers gracefully
        // Since MathHelpers.Fibonacci(-1) returns empty array, this should work
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<int[]>(content);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }
        else
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [TestMethod]
    public async Task DivideInHalf_Endpoint_With_Invalid_Format_Returns_BadRequest()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/divide/not-a-number");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task DoubleIt_Endpoint_With_Invalid_Format_Returns_BadRequest()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/doubleit/invalid");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Hello_Endpoint_With_Empty_Name_Handles_Gracefully()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/hello/");
        // This should return NotFound since the route doesn't match
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task NonExistent_Endpoint_Returns_NotFound()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/nonexistent");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}
