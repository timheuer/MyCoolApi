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
        var hello = await client.GetStringAsync("/hello/tim");
        Assert.AreEqual("Hallo Tim", hello);
    }

    [TestMethod]
    public async Task Goodbye_To_Felicia()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var hello = await client.GetStringAsync("/bye/felicia");
        Debug.WriteLine(hello);
        Assert.AreEqual("Tschüss Felicia!", hello);
    }

    // Unit tests for HelloBuilders methods
    [TestMethod]
    public void SayHello_Unit_Test()
    {
        Assert.AreEqual("Hallo Tim", HalloErsteller.SageHallo("tim"));
        Assert.AreEqual("Hallo John", HalloErsteller.SageHallo("john"));
        Assert.AreEqual("Hallo MARY", HalloErsteller.SageHallo("MARY"));
    }

    [TestMethod]
    public void SayGoodbye_Unit_Test()
    {
        Assert.AreEqual("Tschüss Tim!", HalloErsteller.SageTschuess("tim"));
        Assert.AreEqual("Tschüss John!", HalloErsteller.SageTschuess("john"));
        Assert.AreEqual("Tschüss MARY!", HalloErsteller.SageTschuess("MARY"));
    }
    [TestMethod]
    public void SayHello_With_Mixed_Case()
    {
        Assert.AreEqual("Hallo Alice", HalloErsteller.SageHallo("aLiCe"));
        Assert.AreEqual("Hallo BOB", HalloErsteller.SageHallo("BOB")); // ToTitleCase keeps all-caps as-is
        Assert.AreEqual("Hallo Charlie", HalloErsteller.SageHallo("charlie"));
    }

    [TestMethod]
    public void SayGoodbye_With_Mixed_Case()
    {
        Assert.AreEqual("Tschüss Alice!", HalloErsteller.SageTschuess("aLiCe"));
        Assert.AreEqual("Tschüss BOB!", HalloErsteller.SageTschuess("BOB")); // ToTitleCase keeps all-caps as-is
        Assert.AreEqual("Tschüss Charlie!", HalloErsteller.SageTschuess("charlie"));
    }

    [TestMethod]
    public async Task Hello_Endpoint_With_Special_Characters()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/hello/jane-doe");
        Assert.AreEqual("Hallo Jane-Doe", result);
    }

    [TestMethod]
    public async Task Goodbye_Endpoint_With_Special_Characters()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/bye/jane-doe");
        Assert.AreEqual("Tschüss Jane-Doe!", result);
    }

    [TestMethod]
    public void SayHello_With_Single_Character()
    {
        Assert.AreEqual("Hallo A", HalloErsteller.SageHallo("a"));
        Assert.AreEqual("Hallo Z", HalloErsteller.SageHallo("z"));
    }

    [TestMethod]
    public void SayGoodbye_With_Single_Character()
    {
        Assert.AreEqual("Tschüss A!", HalloErsteller.SageTschuess("a"));
        Assert.AreEqual("Tschüss Z!", HalloErsteller.SageTschuess("z"));
    }
}
