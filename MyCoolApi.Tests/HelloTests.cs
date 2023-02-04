using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace MyCoolApi.Tests;

[TestClass]
public class HelloTests {


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
