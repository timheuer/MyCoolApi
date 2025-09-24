using System.Diagnostics;

namespace MyCoolApi.Tests;

[TestClass]
public class HelloTests
{


    [TestMethod]
    public async Task Hello_To_Tim()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var hello = await client.GetStringAsync("/hallo/tim");
        Assert.AreEqual("Hallo Tim", hello);
    }

    [TestMethod]
    public async Task Goodbye_To_Felicia()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var hello = await client.GetStringAsync("/tschuess/felicia");
        Debug.WriteLine(hello);
        Assert.AreEqual("Tschüss Felicia!", hello);
    }

    // Unit tests for HelloBuilders methods
    [TestMethod]
    public void SayHello_Unit_Test()
    {
        Assert.AreEqual("Hallo Tim", HelloBuilders.SagHallo("tim"));
        Assert.AreEqual("Hallo John", HelloBuilders.SagHallo("john"));
        Assert.AreEqual("Hallo MARY", HelloBuilders.SagHallo("MARY"));
    }

    [TestMethod]
    public void SayGoodbye_Unit_Test()
    {
        Assert.AreEqual("Tschüss Tim!", HelloBuilders.SagTschuess("tim"));
        Assert.AreEqual("Tschüss John!", HelloBuilders.SagTschuess("john"));
        Assert.AreEqual("Tschüss MARY!", HelloBuilders.SagTschuess("MARY"));
    }
    [TestMethod]
    public void SayHello_With_Mixed_Case()
    {
        Assert.AreEqual("Hallo Alice", HelloBuilders.SagHallo("aLiCe"));
        Assert.AreEqual("Hallo BOB", HelloBuilders.SagHallo("BOB")); // ToTitleCase keeps all-caps as-is
        Assert.AreEqual("Hallo Charlie", HelloBuilders.SagHallo("charlie"));
    }

    [TestMethod]
    public void SayGoodbye_With_Mixed_Case()
    {
        Assert.AreEqual("Tschüss Alice!", HelloBuilders.SagTschuess("aLiCe"));
        Assert.AreEqual("Tschüss BOB!", HelloBuilders.SagTschuess("BOB")); // ToTitleCase keeps all-caps as-is
        Assert.AreEqual("Tschüss Charlie!", HelloBuilders.SagTschuess("charlie"));
    }

    [TestMethod]
    public async Task Hello_Endpoint_With_Special_Characters()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/hallo/jane-doe");
        Assert.AreEqual("Hallo Jane-Doe", result);
    }

    [TestMethod]
    public async Task Goodbye_Endpoint_With_Special_Characters()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/tschuess/jane-doe");
        Assert.AreEqual("Tschüss Jane-Doe!", result);
    }

    [TestMethod]
    public void SayHello_With_Single_Character()
    {
        Assert.AreEqual("Hallo A", HelloBuilders.SagHallo("a"));
        Assert.AreEqual("Hallo Z", HelloBuilders.SagHallo("z"));
    }

    [TestMethod]
    public void SayGoodbye_With_Single_Character()
    {
        Assert.AreEqual("Tschüss A!", HelloBuilders.SagTschuess("a"));
        Assert.AreEqual("Tschüss Z!", HelloBuilders.SagTschuess("z"));
    }
}
