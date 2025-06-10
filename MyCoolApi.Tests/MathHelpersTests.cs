using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCoolApi;
using System;
using System.Collections.Generic;
namespace MyCoolApi.UnitTests;

[TestClass]
public class MathHelpersTests
{
    /// <summary>
    /// Tests the Add method of MathHelpers with various integer inputs including edge cases such as zero,
    /// positive numbers, negative numbers, and boundary values using parameterized DataRow.
    /// </summary>
    /// <param name="num1">First integer operand.</param>
    /// <param name="num2">Second integer operand.</param>
    /// <param name="expected">The expected result of the addition.</param>
    [DataTestMethod]
    [DataRow(2, 3, 5)]
    [DataRow(0, 0, 0)]
    [DataRow(-2, 1, -1)]
    [DataRow(int.MaxValue, 0, int.MaxValue)]
    [DataRow(int.MinValue, 0, int.MinValue)]
    [DataRow(int.MaxValue, 1, unchecked(int.MaxValue + 1))]
    [DataRow(int.MinValue, -1, unchecked(int.MinValue - 1))]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void Add_VariousValues_ReturnsExpectedSum(int num1, int num2, int expected)
    {
        // Arrange

        // Act
        int result = MathHelpers.Add(num1, num2);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests MathHelpers.Subtract with various integer inputs including boundaries.
    /// Condition: Testing with typical values, zeroes, negatives, and extreme int values.
    /// Expected outcome: Correct subtraction result.
    /// </summary>
    /// <param name="num1">The minuend.</param>
    /// <param name="num2">The subtrahend.</param>
    /// <param name="expected">The expected result of subtraction.</param>
    [DataTestMethod]
    [DataRow(5, 3, 2)]
    [DataRow(0, 0, 0)]
    [DataRow(-2, 1, -3)]
    [DataRow(int.MaxValue, 1, int.MaxValue - 1)]
    [DataRow(int.MinValue, -1, int.MinValue + 1)]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void Subtract_ValidInputs_ReturnsCorrectResult(int num1, int num2, int expected)
    {
        // Arrange

        // Act
        int result = MathHelpers.Subtract(num1, num2);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests the Multiply method with various integer inputs including edge cases such as zero, negative values, and boundary numbers.
    /// Expected result is the correct product of the two input integers.
    /// </summary>
    [DataTestMethod]
    [DataRow(3, 5, 15)]
    [DataRow(0, 5, 0)]
    [DataRow(5, 0, 0)]
    [DataRow(-2, 3, -6)]
    [DataRow(-2, -3, 6)]
    [DataRow(int.MaxValue, 1, int.MaxValue)]
    [DataRow(1, int.MinValue, int.MinValue)]
    [DataRow(0, int.MinValue, 0)]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void Multiply_ValidInputs_CorrectResult(int num1, int num2, int expected)
    {
        // Arrange

        // Act
        int result = MathHelpers.Multiply(num1, num2);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Provides test cases for the Fibonacci method.
    /// Each test case includes the input number and the expected Fibonacci sequence.
    /// </summary>
    public static IEnumerable<object[]> FibonacciTestCases
    {
        get
        {
            // Test: negative value should return empty array.
            yield return new object[] { -1, Array.Empty<int>() };
            // Test: zero should return empty array.
            yield return new object[] { 0, Array.Empty<int>() };
            // Test: one should return [0].
            yield return new object[] { 1, new int[] { 0 } };
            // Test: two should return [0, 1].
            yield return new object[] { 2, new int[] { 0, 1 } };
            // Test: five should return [0, 1, 1, 2, 3].
            yield return new object[] { 5, new int[] { 0, 1, 1, 2, 3 } };
            // Test: ten should return the first ten Fibonacci numbers.
            yield return new object[] { 10, new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 } };
        }
    }

    /// <summary>
    /// Tests the Fibonacci method with various input scenarios including edge cases.
    /// The method is expected to return the correct Fibonacci sequence for a given input.
    /// </summary>
    /// <param name="input">The number of Fibonacci elements to generate.</param>
    /// <param name="expected">The expected Fibonacci sequence as an integer array.</param>
    [DataTestMethod]
    [DynamicData(nameof(FibonacciTestCases))]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void Fibonacci_VariousInputs_CorrectSequence(int input, int[] expected)
    {
        // Arrange

        // Act
        int[] result = MathHelpers.Fibonacci(input);

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests the Factorial method with valid inputs verifying correct factorials.
    /// </summary>
    /// <param name="input">The input number to compute the factorial.</param>
    /// <param name="expected">The expected result of the factorial computation.</param>
    [DataTestMethod]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow(2, 2)]
    [DataRow(3, 6)]
    [DataRow(4, 24)]
    [DataRow(5, 120)]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void Factorial_ValidInput_ReturnsExpectedResult(int input, int expected)
    {
        // Act
        int actual = MathHelpers.Factorial(input);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Tests the Factorial method with a negative input.
    /// This test is marked inconclusive because negative inputs are not supported and may lead to stack overflow.
    /// </summary>
    [TestMethod]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void Factorial_NegativeInput_BehaviorUndefined()
    {
        // Arrange
        int negativeInput = -1;

        // Act & Assert
        Assert.Inconclusive("Factorial method behavior for negative inputs is undefined and may cause a stack overflow.");
    }

    /// <summary>
    /// Tests the DivideInHalf method with various integer inputs to verify correct integer division and conversion to double.
    /// </summary>
    /// <param name="input">The integer value to be divided in half.</param>
    /// <param name="expectedResult">The expected result considering integer division.</param>
    [DataTestMethod]
    [DataRow(10, 5.0)]
    [DataRow(0, 0.0)]
    [DataRow(-10, -5.0)]
    [DataRow(5, 2.0)] // odd number division, integer division truncates
    [DataRow(int.MaxValue, (double)(int.MaxValue / 2))]
    [DataRow(int.MinValue, (double)(int.MinValue / 2))]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void DivideInHalf_Input_ReturnsExpectedResult(int input, double expectedResult)
    {
        // Arrange

        // Act
        double result = MathHelpers.DivideInHalf(input);

        // Assert
        Assert.AreEqual(expectedResult, result, $"DivideInHalf({input}) should return {expectedResult}.");
    }

    /// <summary>
    /// Tests the MultiplyByTwo method with various integer inputs to ensure it returns the expected doubled values.
    /// Tests boundaries and typical values including int.MaxValue and int.MinValue.
    /// </summary>
    /// <param name="input">The integer to be multiplied by two.</param>
    /// <param name="expected">The expected result of input multiplied by two.</param>
    [DataTestMethod]
    [DataRow(10, 20.0)]
    [DataRow(0, 0.0)]
    [DataRow(-10, -20.0)]
    [DataRow(1, 2.0)]
    [DataRow(int.MaxValue, (double)int.MaxValue * 2)]
    [DataRow(int.MinValue, (double)int.MinValue * 2)]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void MultiplyByTwo_InputVariousValues_ReturnsDoubledValue(int input, double expected)
    {
        // Arrange

        // Act
        double result = MathHelpers.MultiplyByTwo(input);

        // Assert
        Assert.AreEqual(expected, result);
    }
}