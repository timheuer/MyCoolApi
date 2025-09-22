namespace MyCoolApi.Tests;

[TestClass]
public class StringCasingTests
{
    [TestMethod]
    public void ConvertCase_ToPascalCase_Success()
    {
        Assert.AreEqual("HelloWorld", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello_world", "pascal"));
        Assert.AreEqual("HelloWorld", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello-world", "pascalcase"));
        Assert.AreEqual("HelloWorld", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello world", "pascal"));
    }

    [TestMethod]
    public void ConvertCase_ToCamelCase_Success()
    {
        Assert.AreEqual("helloWorld", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello_world", "camel"));
        Assert.AreEqual("helloWorld", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello-world", "camelcase"));
        Assert.AreEqual("helloWorld", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello world", "camel"));
    }

    [TestMethod]
    public void ConvertCase_ToSnakeCase_Success()
    {
        Assert.AreEqual("hello_world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("HelloWorld", "snake"));
        Assert.AreEqual("hello_world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("helloWorld", "snake_case"));
        Assert.AreEqual("hello_world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello-world", "snake"));
    }

    [TestMethod]
    public void ConvertCase_ToKebabCase_Success()
    {
        Assert.AreEqual("hello-world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello_world", "kebab"));
        Assert.AreEqual("hello-world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("HelloWorld", "kebab-case"));
        Assert.AreEqual("hello-world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello world", "kebab"));
    }

    [TestMethod]
    public void ConvertCase_ToSentenceCase_Success()
    {
        Assert.AreEqual("hello world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello_world", "sentence"));
        Assert.AreEqual("Hello world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("HelloWorld", "sentencecase"));
        Assert.AreEqual("hello world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello-world", "sentence"));
    }

    [TestMethod]
    public void ConvertCase_ToTitleCase_Success()
    {
        Assert.AreEqual("Hello World", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello_world", "title"));
        Assert.AreEqual("Hello World", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("helloWorld", "titlecase"));
        Assert.AreEqual("Hello World", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello-world", "title"));
    }

    [TestMethod]
    public void ConvertCase_ToUpperCase_Success()
    {
        Assert.AreEqual("HELLO_WORLD", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("helloWorld", "upper"));
        Assert.AreEqual("HELLO_WORLD", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello-world", "uppercase"));
        Assert.AreEqual("HELLO_WORLD", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello world", "upper"));
    }

    [TestMethod]
    public void ConvertCase_ToLowerCase_Success()
    {
        Assert.AreEqual("helloworld", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("HelloWorld", "lower"));
        Assert.AreEqual("hello-world", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("Hello-World", "lowercase"));
    }

    [TestMethod]
    public void ConvertCase_InvalidCaseType_ThrowsException()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() =>
            ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("hello", "invalid"));
        Assert.IsTrue(ex.Message.Contains("Nicht unterstützter Format-Typ"));
    }

    [TestMethod]
    public void ConvertCase_EmptyString_ReturnsEmpty()
    {
        Assert.AreEqual("", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("", "pascal"));
        Assert.AreEqual("", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("", "camel"));
    }

    [TestMethod]
    public void ConvertCase_NullString_ReturnsNull()
    {
        Assert.IsNull(ZeichenkettenHelfer.KonvertiereGroessKleinschreibung(null!, "pascal"));
    }

    [TestMethod]
    public void ConvertCase_WhitespaceOnly_ReturnsWhitespace()
    {
        Assert.AreEqual("   ", ZeichenkettenHelfer.KonvertiereGroessKleinschreibung("   ", "pascal"));
    }

    // Individual method tests
    [TestMethod]
    public void ToPascalCase_Success()
    {
        Assert.AreEqual("HelloWorld", ZeichenkettenHelfer.ZuPascalFormat("hello_world"));
        Assert.AreEqual("ApiResponse", ZeichenkettenHelfer.ZuPascalFormat("api_response"));
    }

    [TestMethod]
    public void ToCamelCase_Success()
    {
        Assert.AreEqual("helloWorld", ZeichenkettenHelfer.ZuCamelFormat("hello_world"));
        Assert.AreEqual("apiResponse", ZeichenkettenHelfer.ZuCamelFormat("api_response"));
    }

    [TestMethod]
    public void ToSnakeCase_Success()
    {
        Assert.AreEqual("hello_world", ZeichenkettenHelfer.ZuSchlangen_Format("HelloWorld"));
        Assert.AreEqual("api_response", ZeichenkettenHelfer.ZuSchlangen_Format("ApiResponse"));
    }

    [TestMethod]
    public void ToKebabCase_Success()
    {
        Assert.AreEqual("hello-world", ZeichenkettenHelfer.ZuKebabFormat("hello_world"));
        Assert.AreEqual("hello-world", ZeichenkettenHelfer.ZuKebabFormat("HelloWorld"));
    }

    [TestMethod]
    public void ToSentenceCase_Success()
    {
        Assert.AreEqual("hello world", ZeichenkettenHelfer.ZuSatzFormat("hello_world"));
        Assert.AreEqual("Api response", ZeichenkettenHelfer.ZuSatzFormat("ApiResponse"));
    }

    [TestMethod]
    public void ToTitleCase_Success()
    {
        Assert.AreEqual("Hello World", ZeichenkettenHelfer.ZuTitelFormat("hello_world"));
        Assert.AreEqual("Api Response", ZeichenkettenHelfer.ZuTitelFormat("ApiResponse"));
    }

    [TestMethod]
    public void ToUpperCase_Success()
    {
        Assert.AreEqual("HELLO_WORLD", ZeichenkettenHelfer.ZuGrossschreibung("HelloWorld"));
        Assert.AreEqual("API_RESPONSE", ZeichenkettenHelfer.ZuGrossschreibung("ApiResponse"));
    }

    // Integration tests for API endpoints
    [TestMethod]
    public async Task CasingApi_PascalCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/casing/hello_world/pascal");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"HelloWorld\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_CamelCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/casing/hello_world/camel");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"helloWorld\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_SnakeCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/casing/HelloWorld/snake");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"hello_world\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_KebabCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/casing/hello_world/kebab");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"hello-world\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_SentenceCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/casing/hello_world/sentence");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"hello world\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_TitleCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/casing/hello_world/title");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"Hello World\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_UpperCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/casing/helloWorld/upper");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"HELLO_WORLD\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_InvalidCaseType_ReturnsBadRequest()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/casing/hello/invalid");
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}