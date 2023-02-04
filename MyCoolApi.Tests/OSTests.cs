using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace MyCoolApi.Tests;

[TestClass]
public class OSTests {

    [TestMethod]
    public void Running_On_Linux() {
        Console.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Linux));
    }

    [TestMethod]
    public void Running_In_WSL() {
        Console.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.OSDescription.Contains("WSL"));
    }

    [TestMethod]
    public void Running_On_Windows() {
        Console.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
    }
}
