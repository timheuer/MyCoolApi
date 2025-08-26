using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MyCoolApi.Tests;

[TestClass]
public class OSTests
{

    [TestMethod]
    [OSCondition(ConditionMode.Include, OperatingSystems.Linux)]
    public void Running_On_Linux()
    {
        Debug.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Linux));
    }

    [TestMethod]
    [WslCondition]
    public void Running_In_WSL()
    {
        Debug.WriteLine(RuntimeInformation.OSDescription);

        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) &&
                      RuntimeInformation.OSDescription.Contains("WSL", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    [OSCondition(ConditionMode.Include, OperatingSystems.Windows)]
    public void Running_On_Windows()
    {
        Debug.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
    }

    [TestMethod]
    [OSCondition(ConditionMode.Include, OperatingSystems.OSX)]
    public void Running_On_MacOS()
    {
        Debug.WriteLine(RuntimeInformation.OSDescription);
        Assert.IsTrue(RuntimeInformation.IsOSPlatform(OSPlatform.OSX));
    }

    [TestMethod]
    public void Check_OS_Architecture()
    {
        var architecture = RuntimeInformation.ProcessArchitecture;
        Debug.WriteLine($"Process Architecture: {architecture}");
        Assert.IsTrue(
            architecture == Architecture.X64 ||
            architecture == Architecture.X86 ||
            architecture == Architecture.Arm ||
            architecture == Architecture.Arm64);
    }

    [TestMethod]
    public void Check_Framework_Description()
    {
        var framework = RuntimeInformation.FrameworkDescription;
        Debug.WriteLine($"Framework: {framework}");
        Assert.IsTrue(framework.StartsWith(".NET"));
    }

    [TestMethod]
    public void Check_OS_Description_Not_Empty()
    {
        var osDescription = RuntimeInformation.OSDescription;
        Debug.WriteLine($"OS Description: {osDescription}");
        Assert.IsFalse(string.IsNullOrWhiteSpace(osDescription));
    }

    [TestMethod]
    public void Check_Runtime_Identifier()
    {
        var rid = RuntimeInformation.RuntimeIdentifier;
        Debug.WriteLine($"Runtime Identifier: {rid}");
        Assert.IsFalse(string.IsNullOrWhiteSpace(rid));

        // RID should follow the pattern: [os].[version]-[architecture]
        // Examples: win10-x64, linux-x64, osx.12-arm64
        Assert.IsTrue(
            rid.Contains("win") ||
            rid.Contains("linux") ||
            rid.Contains("osx") ||
            rid.Contains("android") ||
            rid.Contains("ios"));
    }

    [TestMethod]
    public void Check_Platform_Specific_Path_Separator()
    {
        var separator = Path.DirectorySeparatorChar;
        Debug.WriteLine($"Directory Separator: {separator}");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.AreEqual('\\', separator);
        }
        else
        {
            Assert.AreEqual('/', separator);
        }
    }

    [TestMethod]
    public void Check_Platform_Not_FreeBSD()
    {
        var freebsd = OSPlatform.Create("FREEBSD");
        Assert.IsFalse(RuntimeInformation.IsOSPlatform(freebsd), "Should not be running on FreeBSD");
    }
}
