using System.Runtime.InteropServices;

namespace MyCoolApi.Tests;

/// <summary>
/// MSTest attribute that only runs the test when RuntimeInformation.OSDescription contains "WSL".
/// Otherwise the test is marked Inconclusive (skipped).
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class WslConditionAttribute : TestMethodAttribute
{
    public override TestResult[] Execute(ITestMethod testMethod)
    {
        var osDescription = RuntimeInformation.OSDescription ?? string.Empty;

        if (!osDescription.Contains("WSL", StringComparison.OrdinalIgnoreCase))
        {
            var tr = new TestResult
            {
                Outcome = UnitTestOutcome.Ignored
            };

            return [tr];
        }

        return base.Execute(testMethod);
    }
}
