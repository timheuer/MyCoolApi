namespace MyCoolApi.Tests;

[TestClass]
public class StringCasingTests
{
    [TestMethod]
    public void ConvertCase_ToPascalCase_Success()
    {
        Assert.AreEqual("HelloWorld", StringCasingHelpers.ConvertCase("hello_world", "pascal"));
        Assert.AreEqual("HelloWorld", StringCasingHelpers.ConvertCase("hello-world", "pascalcase"));
        Assert.AreEqual("HelloWorld", StringCasingHelpers.ConvertCase("hello world", "pascal"));
    }

    [TestMethod]
    public void ConvertCase_ToCamelCase_Success()
    {
        Assert.AreEqual("helloWorld", StringCasingHelpers.ConvertCase("hello_world", "camel"));
        Assert.AreEqual("helloWorld", StringCasingHelpers.ConvertCase("hello-world", "camelcase"));
        Assert.AreEqual("helloWorld", StringCasingHelpers.ConvertCase("hello world", "camel"));
    }

    [TestMethod]
    public void ConvertCase_ToSnakeCase_Success()
    {
        Assert.AreEqual("hello_world", StringCasingHelpers.ConvertCase("HelloWorld", "snake"));
        Assert.AreEqual("hello_world", StringCasingHelpers.ConvertCase("helloWorld", "snake_case"));
        Assert.AreEqual("hello_world", StringCasingHelpers.ConvertCase("hello-world", "snake"));
    }

    [TestMethod]
    public void ConvertCase_ToKebabCase_Success()
    {
        Assert.AreEqual("hello-world", StringCasingHelpers.ConvertCase("hello_world", "kebab"));
        Assert.AreEqual("hello-world", StringCasingHelpers.ConvertCase("HelloWorld", "kebab-case"));
        Assert.AreEqual("hello-world", StringCasingHelpers.ConvertCase("hello world", "kebab"));
    }

    [TestMethod]
    public void ConvertCase_ToSentenceCase_Success()
    {
        Assert.AreEqual("hello world", StringCasingHelpers.ConvertCase("hello_world", "sentence"));
        Assert.AreEqual("Hello world", StringCasingHelpers.ConvertCase("HelloWorld", "sentencecase"));
        Assert.AreEqual("hello world", StringCasingHelpers.ConvertCase("hello-world", "sentence"));
    }

    [TestMethod]
    public void ConvertCase_ToTitleCase_Success()
    {
        Assert.AreEqual("Hello World", StringCasingHelpers.ConvertCase("hello_world", "title"));
        Assert.AreEqual("Hello World", StringCasingHelpers.ConvertCase("helloWorld", "titlecase"));
        Assert.AreEqual("Hello World", StringCasingHelpers.ConvertCase("hello-world", "title"));
    }

    [TestMethod]
    public void ConvertCase_ToUpperCase_Success()
    {
        Assert.AreEqual("HELLO_WORLD", StringCasingHelpers.ConvertCase("helloWorld", "upper"));
        Assert.AreEqual("HELLO_WORLD", StringCasingHelpers.ConvertCase("hello-world", "uppercase"));
        Assert.AreEqual("HELLO_WORLD", StringCasingHelpers.ConvertCase("hello world", "upper"));
    }

    [TestMethod]
    public void ConvertCase_ToLowerCase_Success()
    {
        Assert.AreEqual("helloworld", StringCasingHelpers.ConvertCase("HelloWorld", "lower"));
        Assert.AreEqual("hello-world", StringCasingHelpers.ConvertCase("Hello-World", "lowercase"));
    }

    [TestMethod]
    public void ConvertCase_InvalidCaseType_ThrowsException()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() =>
            StringCasingHelpers.ConvertCase("hello", "invalid"));
        Assert.IsTrue(ex.Message.Contains("Unsupported case type"));
    }

    [TestMethod]
    public void ConvertCase_EmptyString_ReturnsEmpty()
    {
        Assert.AreEqual("", StringCasingHelpers.ConvertCase("", "pascal"));
        Assert.AreEqual("", StringCasingHelpers.ConvertCase("", "camel"));
    }

    [TestMethod]
    public void ConvertCase_NullString_ReturnsNull()
    {
        Assert.IsNull(StringCasingHelpers.ConvertCase(null!, "pascal"));
    }

    [TestMethod]
    public void ConvertCase_WhitespaceOnly_ReturnsWhitespace()
    {
        Assert.AreEqual("   ", StringCasingHelpers.ConvertCase("   ", "pascal"));
    }

    // Individual method tests
    [TestMethod]
    public void ToPascalCase_Success()
    {
        Assert.AreEqual("HelloWorld", StringCasingHelpers.ToPascalCase("hello_world"));
        Assert.AreEqual("ApiResponse", StringCasingHelpers.ToPascalCase("api_response"));
    }

    [TestMethod]
    public void ToCamelCase_Success()
    {
        Assert.AreEqual("helloWorld", StringCasingHelpers.ToCamelCase("hello_world"));
        Assert.AreEqual("apiResponse", StringCasingHelpers.ToCamelCase("api_response"));
    }

    [TestMethod]
    public void ToSnakeCase_Success()
    {
        Assert.AreEqual("hello_world", StringCasingHelpers.ToSnakeCase("HelloWorld"));
        Assert.AreEqual("api_response", StringCasingHelpers.ToSnakeCase("ApiResponse"));
    }

    [TestMethod]
    public void ToKebabCase_Success()
    {
        Assert.AreEqual("hello-world", StringCasingHelpers.ToKebabCase("hello_world"));
        Assert.AreEqual("hello-world", StringCasingHelpers.ToKebabCase("HelloWorld"));
    }

    [TestMethod]
    public void ToSentenceCase_Success()
    {
        Assert.AreEqual("hello world", StringCasingHelpers.ToSentenceCase("hello_world"));
        Assert.AreEqual("Api response", StringCasingHelpers.ToSentenceCase("ApiResponse"));
    }

    [TestMethod]
    public void ToTitleCase_Success()
    {
        Assert.AreEqual("Hello World", StringCasingHelpers.ToTitleCase("hello_world"));
        Assert.AreEqual("Api Response", StringCasingHelpers.ToTitleCase("ApiResponse"));
    }

    [TestMethod]
    public void ToUpperCase_Success()
    {
        Assert.AreEqual("HELLO_WORLD", StringCasingHelpers.ToUpperCase("HelloWorld"));
        Assert.AreEqual("API_RESPONSE", StringCasingHelpers.ToUpperCase("ApiResponse"));
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