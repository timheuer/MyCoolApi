namespace MyCoolApi;

public class MathHelpers {
    
    public static int Add(int num1, int num2)
        => num1 + num2;

    public static int Subtract(int num1, int num2)
        => num1 - num2;

    public static int Multiply(int num1, int num2)
        => num1 * num2;

    public static int[] Fibonacci(int num)
        => Enumerable.Range(0, num).Select(i => i switch {
            0 => 0,
            1 => 1,
            _ => Fibonacci(i - 1).Last() + Fibonacci(i - 2).Last()
        }).ToArray();
}
