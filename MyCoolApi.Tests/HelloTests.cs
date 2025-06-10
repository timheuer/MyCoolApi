using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace MyCoolApi.Tests;

[TestClass]
public class HelloTests
{


    [TestMethod]
    public async Task Hello_To_Tim()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var hello = await client.GetStringAsync("/hello/tim");
        Assert.AreEqual("Hello Tim", hello);
    }

    [TestMethod]
    public async Task Goodbye_To_Felicia()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var hello = await client.GetStringAsync("/bye/felicia");
        Debug.WriteLine(hello);
        Assert.AreEqual("Bye Felicia!", hello);
    }

    // Unit tests for HelloBuilders methods
    [TestMethod]
    public void SayHello_Unit_Test()
    {
        Assert.AreEqual("Hello Tim", HelloBuilders.SayHello("tim"));
        Assert.AreEqual("Hello John", HelloBuilders.SayHello("john"));
        Assert.AreEqual("Hello MARY", HelloBuilders.SayHello("MARY"));
    }

    [TestMethod]
    public void SayGoodbye_Unit_Test()
    {
        Assert.AreEqual("Bye Tim!", HelloBuilders.SayGoodbye("tim"));
        Assert.AreEqual("Bye John!", HelloBuilders.SayGoodbye("john"));
        Assert.AreEqual("Bye MARY!", HelloBuilders.SayGoodbye("MARY"));
    }
    [TestMethod]
    public void SayHello_With_Mixed_Case()
    {
        Assert.AreEqual("Hello Alice", HelloBuilders.SayHello("aLiCe"));
        Assert.AreEqual("Hello BOB", HelloBuilders.SayHello("BOB")); // ToTitleCase keeps all-caps as-is
        Assert.AreEqual("Hello Charlie", HelloBuilders.SayHello("charlie"));
    }

    [TestMethod]
    public void SayGoodbye_With_Mixed_Case()
    {
        Assert.AreEqual("Bye Alice!", HelloBuilders.SayGoodbye("aLiCe"));
        Assert.AreEqual("Bye BOB!", HelloBuilders.SayGoodbye("BOB")); // ToTitleCase keeps all-caps as-is
        Assert.AreEqual("Bye Charlie!", HelloBuilders.SayGoodbye("charlie"));
    }

    [TestMethod]
    public async Task Hello_Endpoint_With_Special_Characters()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/hello/jane-doe");
        Assert.AreEqual("Hello Jane-Doe", result);
    }

    [TestMethod]
    public async Task Goodbye_Endpoint_With_Special_Characters()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/bye/jane-doe");
        Assert.AreEqual("Bye Jane-Doe!", result);
    }

    [TestMethod]
    public void SayHello_With_Single_Character()
    {
        Assert.AreEqual("Hello A", HelloBuilders.SayHello("a"));
        Assert.AreEqual("Hello Z", HelloBuilders.SayHello("z"));
    }

    [TestMethod]
    public void SayGoodbye_With_Single_Character()
    {
        Assert.AreEqual("Bye A!", HelloBuilders.SayGoodbye("a"));
        Assert.AreEqual("Bye Z!", HelloBuilders.SayGoodbye("z"));
    }
}
