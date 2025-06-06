using System.Runtime.InteropServices;

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
        var result = MathHelpers.Fibonacci(5);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Add_Direct_Test()
    {
        Assert.AreEqual(5, MathHelpers.Add(2, 3));
        Assert.AreEqual(0, MathHelpers.Add(0, 0));
        Assert.AreEqual(-1, MathHelpers.Add(-2, 1));
    }

    [TestMethod]
    public void Subtract_Direct_Test()
    {
        Assert.AreEqual(2, MathHelpers.Subtract(5, 3));
        Assert.AreEqual(0, MathHelpers.Subtract(0, 0));
        Assert.AreEqual(-3, MathHelpers.Subtract(-2, 1));
    }

    [TestMethod]
    public void Multiply_Direct_Test()
    {
        Assert.AreEqual(15, MathHelpers.Multiply(3, 5));
        Assert.AreEqual(0, MathHelpers.Multiply(0, 5));
        Assert.AreEqual(-6, MathHelpers.Multiply(-2, 3));
    }

    [TestMethod]
    public void Fibonacci_Edge_Cases()
    {
        // Test empty sequence
        CollectionAssert.AreEqual(new int[0], MathHelpers.Fibonacci(0));

        // Test single element
        CollectionAssert.AreEqual(new[] { 0 }, MathHelpers.Fibonacci(1));

        // Test two elements
        CollectionAssert.AreEqual(new[] { 0, 1 }, MathHelpers.Fibonacci(2));
    }

    [TestMethod]
    public void Factorial_Tests()
    {
        Assert.AreEqual(1, MathHelpers.Factorial(0)); // 0! = 1
        Assert.AreEqual(1, MathHelpers.Factorial(1)); // 1! = 1
        Assert.AreEqual(2, MathHelpers.Factorial(2)); // 2! = 2
        Assert.AreEqual(6, MathHelpers.Factorial(3)); // 3! = 6
        Assert.AreEqual(24, MathHelpers.Factorial(4)); // 4! = 24
        Assert.AreEqual(120, MathHelpers.Factorial(5)); // 5! = 120
    }

    [TestMethod]
    public void DivideInHalf_Tests()
    {
        Assert.AreEqual(5.0, MathHelpers.DivideInHalf(10));
        Assert.AreEqual(0.0, MathHelpers.DivideInHalf(0));
        Assert.AreEqual(-5.0, MathHelpers.DivideInHalf(-10));
        Assert.AreEqual(2, MathHelpers.DivideInHalf(5)); // Test odd number division
    }

    [TestMethod]
    public void MultiplyByTwo_Tests()
    {
        Assert.AreEqual(20.0, MathHelpers.MultiplyByTwo(10));
        Assert.AreEqual(0.0, MathHelpers.MultiplyByTwo(0));
        Assert.AreEqual(-20.0, MathHelpers.MultiplyByTwo(-10));
        Assert.AreEqual(2.0, MathHelpers.MultiplyByTwo(1));
    }
}