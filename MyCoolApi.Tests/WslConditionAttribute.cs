using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyCoolApi.Tests;

/// <summary>
/// MSTest attribute that only runs the test when RuntimeInformation.OSDescription contains "WSL".
/// Otherwise the test is marked Inconclusive (skipped).
/// Apply this attribute in place of `[TestMethod]` on the test method.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class WslConditionAttribute : TestMethodAttribute
{
    public override TestResult[] Execute(ITestMethod testMethod)
    {
        // Guard: if no testMethod provided, fall back to base behavior
        if (testMethod is null) return base.Execute(testMethod!);

        var osDescription = RuntimeInformation.OSDescription ?? string.Empty;

        if (!osDescription.Contains("WSL", StringComparison.OrdinalIgnoreCase))
        {
            var tr = new TestResult
            {
                Outcome = UnitTestOutcome.Ignored,
                TestContextMessages = "Test is only supported on WSL"
            };

            return [tr];
        }

        return base.Execute(testMethod);
    }
}
