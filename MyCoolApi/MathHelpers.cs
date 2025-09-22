using System.Text;

namespace MyCoolApi;

public class MathematikHelfer
{

    public static int Addieren(int zahl1, int zahl2)
        => zahl1 + zahl2;

    public static int Subtrahieren(int zahl1, int zahl2)
        => zahl1 - zahl2;

    public static int Multiplizieren(int zahl1, int zahl2)
        => zahl1 * zahl2; public static int[] Fibonacci(int zahl)
    {
        if (zahl <= 0) return [];
        if (zahl == 1) return [0];

        var ergebnis = new int[zahl];
        ergebnis[0] = 0;
        ergebnis[1] = 1;

        for (int i = 2; i < zahl; i++)
        {
            ergebnis[i] = ergebnis[i - 1] + ergebnis[i - 2];
        }

        return ergebnis;
    }

    public static int Fakultaet(int zahl)
        => zahl switch
        {
            0 => 1,
            _ => zahl * Fakultaet(zahl - 1)
        };

    public static double HalbierenVon(int zahl)
    {
        return zahl / 2;
    }

    public static double VerdoppelnVon(int zahl)
    {
        return zahl * 2;
    }
}
