using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MyCoolApi.Tests;

[TestClass]
public class OSTests {

    [TestMethod]
    public void Running_On_Linux() {
        Debug.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Linux));
    }

    [TestMethod]
    public void Running_In_WSL() {
        Debug.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.OSDescription.Contains("WSL"));
    }

    [TestMethod]
    public void Running_On_Windows() {
        Debug.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
    }
}
