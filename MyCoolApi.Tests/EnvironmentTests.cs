using System.Runtime.InteropServices;

namespace MyCoolApi.Tests;

[TestClass]
public class EnvironmentTests
{
    [TestMethod]
    public async Task Environment_Endpoint_Returns_OS_Description()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var osDescription = await client.GetStringAsync("/env");

        Assert.IsFalse(string.IsNullOrWhiteSpace(osDescription));
        Assert.IsTrue(osDescription.Length > 0);
    }

    [TestMethod]
    public async Task Environment_Endpoint_Matches_Runtime_OS()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var endpointResult = await client.GetStringAsync("/env");
        var runtimeResult = RuntimeInformation.OSDescription;

        Assert.AreEqual(runtimeResult, endpointResult);
    }

    [TestMethod]
    public async Task Environment_Endpoint_Contains_OS_Information()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var osDescription = await client.GetStringAsync("/env");

        // Should contain some indication of OS type
        Assert.IsTrue(
            osDescription.Contains("Windows") ||
            osDescription.Contains("Linux") ||
            osDescription.Contains("Darwin") ||
            osDescription.Contains("macOS") ||
            osDescription.Contains("Unix") ||
            osDescription.Contains("Ubuntu"),
            $"OS Description '{osDescription}' doesn't contain recognized OS name");
    }
}
