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

    // Unit tests for SayCaps method
    [TestMethod]
    public void SayCaps_Unit_Test()
    {
        Assert.AreEqual("HELLO", HelloBuilders.SayCaps("hello"));
        Assert.AreEqual("WORLD", HelloBuilders.SayCaps("world"));
        Assert.AreEqual("TESTING", HelloBuilders.SayCaps("TESTING"));
    }

    [TestMethod]
    public void SayCaps_With_Mixed_Case()
    {
        Assert.AreEqual("HELLO WORLD", HelloBuilders.SayCaps("Hello World"));
        Assert.AreEqual("MIXED CASE TEXT", HelloBuilders.SayCaps("MiXeD cAsE tExT"));
        Assert.AreEqual("ALREADY CAPS", HelloBuilders.SayCaps("ALREADY CAPS"));
    }

    [TestMethod]
    public void SayCaps_With_Special_Characters()
    {
        Assert.AreEqual("HELLO-WORLD!", HelloBuilders.SayCaps("hello-world!"));
        Assert.AreEqual("TEST@EMAIL.COM", HelloBuilders.SayCaps("test@email.com"));
        Assert.AreEqual("123 NUMBER TEST", HelloBuilders.SayCaps("123 number test"));
        Assert.AreEqual("SPECIAL #$%& CHARS", HelloBuilders.SayCaps("special #$%& chars"));
    }

    [TestMethod]
    public void SayCaps_With_Single_Character()
    {
        Assert.AreEqual("A", HelloBuilders.SayCaps("a"));
        Assert.AreEqual("Z", HelloBuilders.SayCaps("z"));
        Assert.AreEqual("5", HelloBuilders.SayCaps("5"));
    }

    [TestMethod]
    public void SayCaps_With_Empty_And_Whitespace()
    {
        Assert.AreEqual("", HelloBuilders.SayCaps(""));
        Assert.AreEqual(" ", HelloBuilders.SayCaps(" "));
        Assert.AreEqual("   ", HelloBuilders.SayCaps("   "));
    }

    // Integration tests for /caps endpoint
    [TestMethod]
    public async Task Caps_Endpoint_Lowercase_Text()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/caps/hello");
        Assert.AreEqual("HELLO", result);
    }

    [TestMethod]
    public async Task Caps_Endpoint_Mixed_Case_Text()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/caps/Hello%20World");
        Assert.AreEqual("HELLO WORLD", result);
    }

    [TestMethod]
    public async Task Caps_Endpoint_With_Special_Characters()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/caps/test-123");
        Assert.AreEqual("TEST-123", result);
    }

    [TestMethod]
    public async Task Caps_Endpoint_Already_Uppercase()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/caps/ALREADY%20CAPS");
        Assert.AreEqual("ALREADY CAPS", result);
    }
}
