namespace MyCoolApi;

/// <summary>
/// Generates PowerBall lottery numbers
/// </summary>
public class PowerBallHelpers
{
    /// <summary>
    /// Generates a set of PowerBall numbers
    /// </summary>
    /// <returns>PowerBallResult containing 5 white balls (1-69) and 1 PowerBall (1-26)</returns>
    public static PowerBallResult GenerateNumbers()
    {
        // Generate 5 unique white balls from 1-69
        var whiteBalls = new HashSet<int>();
        while (whiteBalls.Count < 5)
        {
            whiteBalls.Add(Random.Shared.Next(1, 70)); // 1-69 inclusive
        }

        // Sort white balls for consistent display
        var sortedWhiteBalls = whiteBalls.OrderBy(n => n).ToArray();

        // Generate PowerBall from 1-26
        var powerBall = Random.Shared.Next(1, 27); // 1-26 inclusive

        return new PowerBallResult(sortedWhiteBalls, powerBall);
    }
}

/// <summary>
/// Represents a PowerBall lottery result
/// </summary>
/// <param name="WhiteBalls">Array of 5 unique numbers from 1-69, sorted</param>
/// <param name="PowerBall">Single number from 1-26</param>
public record PowerBallResult(int[] WhiteBalls, int PowerBall);
