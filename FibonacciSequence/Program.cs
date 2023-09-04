namespace FibonacciSequence;

internal static class Program
{
    public static void Main(string[] args)
    {
        var sequence = FibonacciByMaximum(400);
        Console.WriteLine(string.Join(", ", sequence));

        Console.WriteLine(FibonacciClosestToValue(355));
    }

    private static List<int> FibonacciByLength(int length)
    {
        var sequence = new List<int> {0, 1};

        for (var i = 2; i < length; i++) sequence.Add(sequence[i - 2] + sequence[i - 1]);

        return sequence;
    }

    private static List<int> FibonacciByMaximum(int maximum)
    {
        var sequence = new List<int> {0, 1};

        var i = 2;
        while (sequence[i - 2] + sequence[i - 1] < maximum)
        {
            sequence.Add(sequence[i - 2] + sequence[i - 1]);
            i++;
        }

        return sequence;
    }

    private static int FibonacciClosestToValue(double value)
    {
        var sequence = FibonacciByMaximum(2 * (int) value);

        var minErr = double.MaxValue;
        var chosenNumber = int.MaxValue;
        foreach (var number in sequence)
            if (Math.Abs(value - number) < minErr)
            {
                minErr = Math.Abs(value - number);
                chosenNumber = number;
            }

        return chosenNumber;
    }
}