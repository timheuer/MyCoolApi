using System.Text.Json;
using MyCoolApi;

namespace MyCoolApi.Tests;

[TestClass]
public class PowerBallTests
{
    [TestMethod]
    public async Task PowerBall_Endpoint_Returns_Valid_Result()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/powerball");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PowerBallResult>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.WhiteBalls);
        Assert.AreEqual(5, result.WhiteBalls.Length);
        Assert.IsTrue(result.PowerBall >= 1 && result.PowerBall <= 26);
    }

    [TestMethod]
    public void GenerateNumbers_Returns_Five_WhiteBalls()
    {
        var result = PowerBallHelpers.GenerateNumbers();

        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.WhiteBalls.Length);
    }

    [TestMethod]
    public void GenerateNumbers_WhiteBalls_Are_In_Valid_Range()
    {
        var result = PowerBallHelpers.GenerateNumbers();

        foreach (var ball in result.WhiteBalls)
        {
            Assert.IsTrue(ball >= 1 && ball <= 69, $"White ball {ball} is out of range (1-69)");
        }
    }

    [TestMethod]
    public void GenerateNumbers_WhiteBalls_Are_Unique()
    {
        var result = PowerBallHelpers.GenerateNumbers();

        var uniqueBalls = result.WhiteBalls.Distinct().Count();
        Assert.AreEqual(5, uniqueBalls, "White balls should all be unique");
    }

    [TestMethod]
    public void GenerateNumbers_WhiteBalls_Are_Sorted()
    {
        var result = PowerBallHelpers.GenerateNumbers();

        var sortedBalls = result.WhiteBalls.OrderBy(n => n).ToArray();
        CollectionAssert.AreEqual(sortedBalls, result.WhiteBalls, "White balls should be sorted");
    }

    [TestMethod]
    public void GenerateNumbers_PowerBall_Is_In_Valid_Range()
    {
        var result = PowerBallHelpers.GenerateNumbers();

        Assert.IsTrue(result.PowerBall >= 1 && result.PowerBall <= 26, 
            $"PowerBall {result.PowerBall} is out of range (1-26)");
    }

    [TestMethod]
    public void GenerateNumbers_Multiple_Calls_Produce_Different_Results()
    {
        var results = new List<PowerBallResult>();
        
        // Generate 10 results
        for (int i = 0; i < 10; i++)
        {
            results.Add(PowerBallHelpers.GenerateNumbers());
        }

        // At least some results should be different (extremely unlikely to be all the same)
        var uniqueResults = results.Select(r => string.Join(",", r.WhiteBalls) + ":" + r.PowerBall).Distinct().Count();
        Assert.IsTrue(uniqueResults > 1, "Multiple calls should produce different results");
    }

    [TestMethod]
    public async Task PowerBall_Endpoint_Returns_JSON()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/powerball");
        response.EnsureSuccessStatusCode();

        var contentType = response.Content.Headers.ContentType?.MediaType;
        Assert.AreEqual("application/json", contentType);
    }
}
