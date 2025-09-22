namespace MyCoolApi.Tests;

[TestClass]
public class MathTests
{

    [TestMethod]
    public async Task One_Plus_One_Is_Two()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/add/1,1");
        Assert.AreEqual(2, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Two_Is_Three()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/add/1,2");
        Assert.AreEqual(3, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Three_Is_Four()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/add/1,3");
        Assert.AreEqual(4, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task One_Plus_Four_Is_Five()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var sum = await client.GetStringAsync("/add/1,4");
        Assert.AreEqual(5, Convert.ToInt32(sum));
    }

    [TestMethod]
    public async Task Three_Times_Four_Is_Twelve()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var product = await client.GetStringAsync("/multiply/3,4");
        Assert.AreEqual(12, Convert.ToInt32(product));
    }

    [TestMethod]
    public async Task Four_Times_Five_Is_Twenty()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var product = await client.GetStringAsync("/multiply/4,5");
        Assert.AreEqual(20, Convert.ToInt32(product));
    }

    [TestMethod]
    public async Task Half_Of_Twelve()
    {
        await using var application = new MyCoolApiApp();

        var client = application.CreateClient();
        var product = await client.GetStringAsync("/divide/12");
        Assert.AreEqual(6, Convert.ToDouble(product));
    }

    [TestMethod]
    public void Fibonacci_First_Five_Numbers()
    {
        // First 5 Fibonacci numbers should be [0, 1, 1, 2, 3]
        var expected = new[] { 0, 1, 1, 2, 3 };
        var result = MathematikHelfer.Fibonacci(5);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Add_Direct_Test()
    {
        Assert.AreEqual(5, MathematikHelfer.Addieren(2, 3));
        Assert.AreEqual(0, MathematikHelfer.Addieren(0, 0));
        Assert.AreEqual(-1, MathematikHelfer.Addieren(-2, 1));
    }

    [TestMethod]
    public void Subtract_Direct_Test()
    {
        Assert.AreEqual(2, MathematikHelfer.Subtrahieren(5, 3));
        Assert.AreEqual(0, MathematikHelfer.Subtrahieren(0, 0));
        Assert.AreEqual(-3, MathematikHelfer.Subtrahieren(-2, 1));
    }

    [TestMethod]
    public void Multiply_Direct_Test()
    {
        Assert.AreEqual(15, MathematikHelfer.Multiplizieren(3, 5));
        Assert.AreEqual(0, MathematikHelfer.Multiplizieren(0, 5));
        Assert.AreEqual(-6, MathematikHelfer.Multiplizieren(-2, 3));
    }

    [TestMethod]
    public void Fibonacci_Edge_Cases()
    {
        // Test empty sequence
        CollectionAssert.AreEqual(new int[0], MathematikHelfer.Fibonacci(0));

        // Test single element
        CollectionAssert.AreEqual(new[] { 0 }, MathematikHelfer.Fibonacci(1));

        // Test two elements
        CollectionAssert.AreEqual(new[] { 0, 1 }, MathematikHelfer.Fibonacci(2));
    }

    [TestMethod]
    public void Factorial_Tests()
    {
        Assert.AreEqual(1, MathematikHelfer.Fakultaet(0)); // 0! = 1
        Assert.AreEqual(1, MathematikHelfer.Fakultaet(1)); // 1! = 1
        Assert.AreEqual(2, MathematikHelfer.Fakultaet(2)); // 2! = 2
        Assert.AreEqual(6, MathematikHelfer.Fakultaet(3)); // 3! = 6
        Assert.AreEqual(24, MathematikHelfer.Fakultaet(4)); // 4! = 24
        Assert.AreEqual(120, MathematikHelfer.Fakultaet(5)); // 5! = 120
    }

    [TestMethod]
    public void DivideInHalf_Tests()
    {
        Assert.AreEqual(5.0, MathematikHelfer.HalbierenVon(10));
        Assert.AreEqual(0.0, MathematikHelfer.HalbierenVon(0));
        Assert.AreEqual(-5.0, MathematikHelfer.HalbierenVon(-10));
        Assert.AreEqual(2, MathematikHelfer.HalbierenVon(5)); // Test odd number division
    }

    [TestMethod]
    public void MultiplyByTwo_Tests()
    {
        Assert.AreEqual(20.0, MathematikHelfer.VerdoppelnVon(10));
        Assert.AreEqual(0.0, MathematikHelfer.VerdoppelnVon(0));
        Assert.AreEqual(-20.0, MathematikHelfer.VerdoppelnVon(-10));
        Assert.AreEqual(2.0, MathematikHelfer.VerdoppelnVon(1));
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

        var result1 = await client.GetStringAsync("/doubleit/5");
        Assert.AreEqual(10.0, Convert.ToDouble(result1));

        var result2 = await client.GetStringAsync("/doubleit/0");
        Assert.AreEqual(0.0, Convert.ToDouble(result2));

        var result3 = await client.GetStringAsync("/doubleit/-3");
        Assert.AreEqual(-6.0, Convert.ToDouble(result3));
    }

    [TestMethod]
    public async Task Add_Endpoint_Negative_Numbers()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/add/-5,-3");
        Assert.AreEqual(-8, Convert.ToInt32(result));
    }

    [TestMethod]
    public async Task Multiply_Endpoint_With_Zero()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/multiply/0,999");
        Assert.AreEqual(0, Convert.ToInt32(result));
    }
    [TestMethod]
    public async Task Divide_Endpoint_Odd_Numbers()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/divide/7");
        Assert.AreEqual(3, Convert.ToDouble(result)); // Integer division: 7/2 = 3
    }

    [TestMethod]
    public async Task Divide_Endpoint_Even_Numbers()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/divide/10");
        Assert.AreEqual(5.0, Convert.ToDouble(result));
    }

    [TestMethod]
    public async Task Divide_Endpoint_Zero()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/divide/0");
        Assert.AreEqual(0.0, Convert.ToDouble(result));
    }

    [TestMethod]
    public async Task Divide_Endpoint_Negative_Numbers()
    {
        await using var application = new MyCoolApiApp();
        var client = application.CreateClient();

        var result = await client.GetStringAsync("/divide/-8");
        Assert.AreEqual(-4.0, Convert.ToDouble(result));
    }
}