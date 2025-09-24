namespace MyCoolApi.Tests;

[TestClass]
public class StringCasingTests
{
    [TestMethod]
    public void ConvertCase_ToPascalCase_Success()
    {
        Assert.AreEqual("HelloWorld", StringCasingHelpers.KonvertiereSchreibweise("hello_world", "pascal"));
        Assert.AreEqual("HelloWorld", StringCasingHelpers.KonvertiereSchreibweise("hello-world", "pascalcase"));
        Assert.AreEqual("HelloWorld", StringCasingHelpers.KonvertiereSchreibweise("hello world", "pascal"));
    }

    [TestMethod]
    public void ConvertCase_ToCamelCase_Success()
    {
        Assert.AreEqual("helloWorld", StringCasingHelpers.KonvertiereSchreibweise("hello_world", "camel"));
        Assert.AreEqual("helloWorld", StringCasingHelpers.KonvertiereSchreibweise("hello-world", "camelcase"));
        Assert.AreEqual("helloWorld", StringCasingHelpers.KonvertiereSchreibweise("hello world", "camel"));
    }

    [TestMethod]
    public void ConvertCase_ToSnakeCase_Success()
    {
        Assert.AreEqual("hello_world", StringCasingHelpers.KonvertiereSchreibweise("HelloWorld", "snake"));
        Assert.AreEqual("hello_world", StringCasingHelpers.KonvertiereSchreibweise("helloWorld", "snake_case"));
        Assert.AreEqual("hello_world", StringCasingHelpers.KonvertiereSchreibweise("hello-world", "snake"));
    }

    [TestMethod]
    public void ConvertCase_ToKebabCase_Success()
    {
        Assert.AreEqual("hello-world", StringCasingHelpers.KonvertiereSchreibweise("hello_world", "kebab"));
        Assert.AreEqual("hello-world", StringCasingHelpers.KonvertiereSchreibweise("HelloWorld", "kebab-case"));
        Assert.AreEqual("hello-world", StringCasingHelpers.KonvertiereSchreibweise("hello world", "kebab"));
    }

    [TestMethod]
    public void ConvertCase_ToSentenceCase_Success()
    {
        Assert.AreEqual("hello world", StringCasingHelpers.KonvertiereSchreibweise("hello_world", "sentence"));
        Assert.AreEqual("Hello world", StringCasingHelpers.KonvertiereSchreibweise("HelloWorld", "sentencecase"));
        Assert.AreEqual("hello world", StringCasingHelpers.KonvertiereSchreibweise("hello-world", "sentence"));
    }

    [TestMethod]
    public void ConvertCase_ToTitleCase_Success()
    {
        Assert.AreEqual("Hello World", StringCasingHelpers.KonvertiereSchreibweise("hello_world", "title"));
        Assert.AreEqual("Hello World", StringCasingHelpers.KonvertiereSchreibweise("helloWorld", "titlecase"));
        Assert.AreEqual("Hello World", StringCasingHelpers.KonvertiereSchreibweise("hello-world", "title"));
    }

    [TestMethod]
    public void ConvertCase_ToUpperCase_Success()
    {
        Assert.AreEqual("HELLO_WORLD", StringCasingHelpers.KonvertiereSchreibweise("helloWorld", "upper"));
        Assert.AreEqual("HELLO_WORLD", StringCasingHelpers.KonvertiereSchreibweise("hello-world", "uppercase"));
        Assert.AreEqual("HELLO_WORLD", StringCasingHelpers.KonvertiereSchreibweise("hello world", "upper"));
    }

    [TestMethod]
    public void ConvertCase_ToLowerCase_Success()
    {
        Assert.AreEqual("helloworld", StringCasingHelpers.KonvertiereSchreibweise("HelloWorld", "lower"));
        Assert.AreEqual("hello-world", StringCasingHelpers.KonvertiereSchreibweise("Hello-World", "lowercase"));
    }

    [TestMethod]
    public void ConvertCase_InvalidCaseType_ThrowsException()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() =>
            StringCasingHelpers.KonvertiereSchreibweise("hello", "invalid"));
        Assert.IsTrue(ex.Message.Contains("Nicht unterstützter Schreibweise-Typ"));
    }

    [TestMethod]
    public void ConvertCase_EmptyString_ReturnsEmpty()
    {
        Assert.AreEqual("", StringCasingHelpers.KonvertiereSchreibweise("", "pascal"));
        Assert.AreEqual("", StringCasingHelpers.KonvertiereSchreibweise("", "camel"));
    }

    [TestMethod]
    public void ConvertCase_NullString_ReturnsNull()
    {
        Assert.IsNull(StringCasingHelpers.KonvertiereSchreibweise(null!, "pascal"));
    }

    [TestMethod]
    public void ConvertCase_WhitespaceOnly_ReturnsWhitespace()
    {
        Assert.AreEqual("   ", StringCasingHelpers.KonvertiereSchreibweise("   ", "pascal"));
    }

    // Individual method tests
    [TestMethod]
    public void ToPascalCase_Success()
    {
        Assert.AreEqual("HelloWorld", StringCasingHelpers.ZuPascalCase("hello_world"));
        Assert.AreEqual("ApiResponse", StringCasingHelpers.ZuPascalCase("api_response"));
    }

    [TestMethod]
    public void ToCamelCase_Success()
    {
        Assert.AreEqual("helloWorld", StringCasingHelpers.ZuCamelCase("hello_world"));
        Assert.AreEqual("apiResponse", StringCasingHelpers.ZuCamelCase("api_response"));
    }

    [TestMethod]
    public void ToSnakeCase_Success()
    {
        Assert.AreEqual("hello_world", StringCasingHelpers.ZuSnakeCase("HelloWorld"));
        Assert.AreEqual("api_response", StringCasingHelpers.ZuSnakeCase("ApiResponse"));
    }

    [TestMethod]
    public void ToKebabCase_Success()
    {
        Assert.AreEqual("hello-world", StringCasingHelpers.ZuKebabCase("hello_world"));
        Assert.AreEqual("hello-world", StringCasingHelpers.ZuKebabCase("HelloWorld"));
    }

    [TestMethod]
    public void ToSentenceCase_Success()
    {
        Assert.AreEqual("hello world", StringCasingHelpers.ZuSentenceCase("hello_world"));
        Assert.AreEqual("Api response", StringCasingHelpers.ZuSentenceCase("ApiResponse"));
    }

    [TestMethod]
    public void ToTitleCase_Success()
    {
        Assert.AreEqual("Hello World", StringCasingHelpers.ZuTitleCase("hello_world"));
        Assert.AreEqual("Api Response", StringCasingHelpers.ZuTitleCase("ApiResponse"));
    }

    [TestMethod]
    public void ToUpperCase_Success()
    {
        Assert.AreEqual("HELLO_WORLD", StringCasingHelpers.ZuUpperCase("HelloWorld"));
        Assert.AreEqual("API_RESPONSE", StringCasingHelpers.ZuUpperCase("ApiResponse"));
    }

    // Integration tests for API endpoints
    [TestMethod]
    public async Task CasingApi_PascalCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/schreibweise/hello_world/pascal");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"HelloWorld\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_CamelCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/schreibweise/hello_world/camel");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"helloWorld\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_SnakeCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/schreibweise/HelloWorld/snake");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"hello_world\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_KebabCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/schreibweise/hello_world/kebab");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"hello-world\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_SentenceCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/schreibweise/hello_world/sentence");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"hello world\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_TitleCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/schreibweise/hello_world/title");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"Hello World\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_UpperCase_Success()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/schreibweise/helloWorld/upper");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("\"HELLO_WORLD\"", content); // JSON string response
    }

    [TestMethod]
    public async Task CasingApi_InvalidCaseType_ReturnsBadRequest()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetAsync("/schreibweise/hello/invalid");
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}