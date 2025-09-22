using System.Text;

namespace MyCoolApi;

public class MathHelpers
{

    public static int Add(int num1, int num2)
        => num1 + num2;

    public static int Subtract(int num1, int num2)
        => num1 - num2;

    public static int Multiply(int num1, int num2)
        => num1 * num2; public static int[] Fibonacci(int num)
    {
        if (num <= 0) return [];
        if (num == 1) return [0];

        var result = new int[num];
        result[0] = 0;
        result[1] = 1;

        for (int i = 2; i < num; i++)
        {
            result[i] = result[i - 1] + result[i - 2];
        }

        return result;
    }

    public static int Factorial(int num)
        => num switch
        {
            0 => 1,
            _ => num * Factorial(num - 1)
        };

    public static double DivideInHalf(int number)
    {
        return number / 2;
    }

    public static double MultiplyByTwo(int number)
    {
        return number * 2;
    }
}
