using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCoolApi;
using System;
using System.Globalization;
namespace MyCoolApi.UnitTests;

[TestClass]
public class HelloBuildersTests
{
    /// <summary>
    /// Tests that SayHello returns the correct greeting with name converted to title case.
    /// </summary>
    /// <param name="name">Input name string.</param>
    /// <param name="expectedGreeting">Expected greeting result.</param>
    [DataTestMethod]
    [DataRow("tim", "Hello Tim")]
    [DataRow("TIM", "Hello Tim")]
    [DataRow("john doe", "Hello John Doe")]
    [DataRow("", "Hello ")]
    [DataRow("   ", "Hello    ")]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void SayHello_ValidName_ReturnsGreeting(string name, string expectedGreeting)
    {
        // Arrange

        // Act
        string result = HelloBuilders.SayHello(name);

        // Assert
        Assert.AreEqual(expectedGreeting, result);
    }

    /// <summary>
    /// Tests that SayHello throws an ArgumentNullException when a null name is passed.
    /// Passing null is not allowed for non-nullable parameters.
    /// </summary>
    [TestMethod]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void SayHello_NullName_ThrowsArgumentNullException()
    {
        // Arrange
        string? nullName = null;

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => HelloBuilders.SayHello(nullName!));
    }

    /// <summary>
    /// Tests the SayGoodbye method with various valid name inputs to ensure it returns the correctly formatted goodbye greeting.
    /// </summary>
    /// <param name="inputName">The input name to be converted to title case.</param>
    /// <param name="expectedGreeting">The expected formatted greeting.</param>
    [DataTestMethod]
    [DataRow("tim", "Bye Tim!")]
    [DataRow("TIM", "Bye Tim!")]
    [DataRow("tIm", "Bye Tim!")]
    [DataRow("Alice", "Bye Alice!")]
    [DataRow("", "Bye !")]
    [DataRow("   ", "Bye    !")]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void SayGoodbye_ValidInput_ReturnsCorrectGreeting(string inputName, string expectedGreeting)
    {
        // Arrange
        // Act
        string result = HelloBuilders.SayGoodbye(inputName);

        // Assert
        Assert.AreEqual(expectedGreeting, result);
    }

    /// <summary>
    /// Tests that the SayGoodbye method throws an ArgumentNullException when provided a null name.
    /// </summary>
    [TestMethod]
    [Owner("AI Testing Agent v0.1.0-alpha.25310.41+547a3a2")]
    [TestCategory("auto-generated")]
    public void SayGoodbye_NullName_ThrowsArgumentNullException()
    {
        // Arrange
        string? inputName = null;

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            HelloBuilders.SayGoodbye(inputName!);
        });
    }
}