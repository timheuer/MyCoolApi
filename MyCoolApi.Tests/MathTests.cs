using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace MyCoolApi.Tests;

[TestClass]
public class MathTests {

    [TestMethod]
    public async Task One_Plus_One_Is_Two() {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/add/1,1");
        Assert.AreEqual(2, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Two_Is_Three() {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/add/1,2");
        Assert.AreEqual(3, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Three_Is_Four() {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/add/1,3");
        Assert.AreEqual(4, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Four_Is_Five() {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/add/1,4");
        Assert.AreEqual(6, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task Three_Times_Four_Is_Twelve() {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var product = await client.GetStringAsync("/multiply/3,4");
        Assert.AreEqual(12, Convert.ToInt32(product));
    }

    [TestMethod]
    public async Task Hello_To_Tim() {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var hello = await client.GetStringAsync("/hello/tim");
        Assert.AreEqual("Hello Tim", hello);
    }

    [TestMethod]
    public async Task Goodbye_To_Felicia() {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var hello = await client.GetStringAsync("/bye/felicia");
        Assert.AreEqual("Bye Felicia!", hello);
    }
}


class MyCoolApiApp : WebApplicationFactory<Program> {
    protected override IHost CreateHost(IHostBuilder builder) {
        return base.CreateHost(builder);
    }
}