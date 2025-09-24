namespace MyCoolApi.Tests;

[TestClass]
public class MathTests
{

    [TestMethod]
    public async Task One_Plus_One_Is_Two()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/addieren/1,1");
        Assert.AreEqual(2, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Two_Is_Three()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/addieren/1,2");
        Assert.AreEqual(3, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Three_Is_Four()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/addieren/1,3");
        Assert.AreEqual(4, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Four_Is_Five()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/addieren/1,4");
        Assert.AreEqual(5, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task Three_Times_Four_Is_Twelve()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var product = await client.GetStringAsync("/multiplizieren/3,4");
        Assert.AreEqual(12, Convert.ToInt32(product));
    }

    [TestMethod]
    public async Task Four_Times_Five_Is_Twenty()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var product = await client.GetStringAsync("/multiplizieren/4,5");
        Assert.AreEqual(20, Convert.ToInt32(product));
    }

    [TestMethod]
    public async Task Half_Of_Twelve()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var product = await client.GetStringAsync("/halbieren/12");
        Assert.AreEqual(6, Convert.ToDouble(product));
    }

    [TestMethod]
    public void Fibonacci_First_Five_Numbers()
    {
        // First 5 Fibonacci numbers should be [0, 1, 1, 2, 3]
        var expected = new[] { 0, 1, 1, 2, 3 };
        var result = MathHelfers.Fibonacci(5);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Add_Direct_Test()
    {
        Assert.AreEqual(5, MathHelfers.Addieren(2, 3));
        Assert.AreEqual(0, MathHelfers.Addieren(0, 0));
        Assert.AreEqual(-1, MathHelfers.Addieren(-2, 1));
    }

    [TestMethod]
    public void Subtract_Direct_Test()
    {
        Assert.AreEqual(2, MathHelfers.Subtrahieren(5, 3));
        Assert.AreEqual(0, MathHelfers.Subtrahieren(0, 0));
        Assert.AreEqual(-3, MathHelfers.Subtrahieren(-2, 1));
    }

    [TestMethod]
    public void Multiply_Direct_Test()
    {
        Assert.AreEqual(15, MathHelfers.Multiplizieren(3, 5));
        Assert.AreEqual(0, MathHelfers.Multiplizieren(0, 5));
        Assert.AreEqual(-6, MathHelfers.Multiplizieren(-2, 3));
    }

    [TestMethod]
    public void Fibonacci_Edge_Cases()
    {
        // Test empty sequence
        CollectionAssert.AreEqual(new int[0], MathHelfers.Fibonacci(0));

        // Test single element
        CollectionAssert.AreEqual(new[] { 0 }, MathHelfers.Fibonacci(1));

        // Test two elements
        CollectionAssert.AreEqual(new[] { 0, 1 }, MathHelfers.Fibonacci(2));
    }

    [TestMethod]
    public void Factorial_Tests()
    {
        Assert.AreEqual(1, MathHelfers.Fakultaet(0)); // 0! = 1
        Assert.AreEqual(1, MathHelfers.Fakultaet(1)); // 1! = 1
        Assert.AreEqual(2, MathHelfers.Fakultaet(2)); // 2! = 2
        Assert.AreEqual(6, MathHelfers.Fakultaet(3)); // 3! = 6
        Assert.AreEqual(24, MathHelfers.Fakultaet(4)); // 4! = 24
        Assert.AreEqual(120, MathHelfers.Fakultaet(5)); // 5! = 120
    }

    [TestMethod]
    public void DivideInHalf_Tests()
    {
        Assert.AreEqual(5.0, MathHelfers.Halbieren(10));
        Assert.AreEqual(0.0, MathHelfers.Halbieren(0));
        Assert.AreEqual(-5.0, MathHelfers.Halbieren(-10));
        Assert.AreEqual(2, MathHelfers.Halbieren(5)); // Test odd number division
    }

    [TestMethod]
    public void MultiplyByTwo_Tests()
    {
        Assert.AreEqual(20.0, MathHelfers.Verdoppeln(10));
        Assert.AreEqual(0.0, MathHelfers.Verdoppeln(0));
        Assert.AreEqual(-20.0, MathHelfers.Verdoppeln(-10));
        Assert.AreEqual(2.0, MathHelfers.Verdoppeln(1));
    }

    [TestMethod]
    public async Task Fibonacci_Endpoint_Returns_Correct_Sequence()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var response = await client.GetStringAsync("/fibonacci/5");
        var result = System.Text.Json.JsonSerializer.Deserialize<int[]>(response);

        var expected = new[] { 0, 1, 1, 2, 3 };
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task Fibonacci_Endpoint_Edge_Cases()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();
        // Test empty sequence
        var response0 = await client.GetStringAsync("/fibonacci/0");
        var result0 = System.Text.Json.JsonSerializer.Deserialize<int[]>(response0);
        Assert.IsNotNull(result0);
        Assert.AreEqual(0, result0.Length);

        // Test single element
        var response1 = await client.GetStringAsync("/fibonacci/1");
        var result1 = System.Text.Json.JsonSerializer.Deserialize<int[]>(response1);
        CollectionAssert.AreEqual(new[] { 0 }, result1);
    }

    [TestMethod]
    public async Task DoubleIt_Endpoint_Tests()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result1 = await client.GetStringAsync("/verdoppeln/5");
        Assert.AreEqual(10.0, Convert.ToDouble(result1));

        var result2 = await client.GetStringAsync("/verdoppeln/0");
        Assert.AreEqual(0.0, Convert.ToDouble(result2));

        var result3 = await client.GetStringAsync("/verdoppeln/-3");
        Assert.AreEqual(-6.0, Convert.ToDouble(result3));
    }

    [TestMethod]
    public async Task Add_Endpoint_Negative_Numbers()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/addieren/-5,-3");
        Assert.AreEqual(-8, Convert.ToInt32(result));
    }

    [TestMethod]
    public async Task Multiply_Endpoint_With_Zero()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/multiplizieren/0,999");
        Assert.AreEqual(0, Convert.ToInt32(result));
    }
    [TestMethod]
    public async Task Divide_Endpoint_Odd_Numbers()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/halbieren/7");
        Assert.AreEqual(3, Convert.ToDouble(result)); // Integer division: 7/2 = 3
    }

    [TestMethod]
    public async Task Divide_Endpoint_Even_Numbers()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/halbieren/10");
        Assert.AreEqual(5.0, Convert.ToDouble(result));
    }

    [TestMethod]
    public async Task Divide_Endpoint_Zero()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/halbieren/0");
        Assert.AreEqual(0.0, Convert.ToDouble(result));
    }

    [TestMethod]
    public async Task Divide_Endpoint_Negative_Numbers()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/halbieren/-8");
        Assert.AreEqual(-4.0, Convert.ToDouble(result));
    }
}