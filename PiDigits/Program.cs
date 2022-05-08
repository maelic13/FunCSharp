using System.Diagnostics;

namespace PiDigits;

internal static class Program
{
    public static void Main()
    {
        var watch = Stopwatch.StartNew();
        var pi = GetPiDigits(1);
        watch.Stop();

        var watch_mt = Stopwatch.StartNew();
        var pi_mc_mt = GetPiMonteCarloMultithreaded(4);
        watch_mt.Stop();

        Console.WriteLine($"{pi}, {watch.ElapsedMilliseconds} ms.");
        Console.WriteLine($"{pi_mc_mt}, {watch_mt.ElapsedMilliseconds} ms.");
    }

    private static double GetPiDigits(int digits)
    {
        double sum = 0;
        for (var i = 0; i < digits; i++)
        {
            sum += CalculateSum(i);
        }

        return (426880 * Math.Sqrt(10005)) / sum;
    }

    private static double CalculateSum(int q)
    {
        var upper = Factorial(6 * q) * (545140134 * q + 13591409);
        var lower = Factorial(3 * q) * Math.Pow(Factorial(q), 3) * Math.Pow(-262537412640768000, q);
        return upper / lower;
    }

    private static int Factorial(int number)
    {
        var result = 1;
        for (var i = 2; i <= number; i++)
        {
            result *= i;
        }

        return result;
    }

    private static double GetPiMonteCarloMultithreaded(int digits)
    {
        var cpus = Environment.ProcessorCount;
        long pointsCount = (long)Math.Pow(10, 2 * digits);

        var interval = pointsCount / cpus;
        var inputList = new List<long>();
        var results = new List<long>();
        
        for (var i = 0; i < cpus; i++) 
            inputList.Add(interval);
        inputList[^1] += pointsCount % cpus;
        
        Parallel.ForEach(inputList, sub => results.Add(GetPoints(sub)));

        return 4.0 * results.Sum() / pointsCount;
    }

    private static double GetPiMonteCarlo(int digits)
    {
        long pointsCount = (long)Math.Pow(10, 2 * digits);
        var inside = GetPoints(pointsCount);

        return 4.0 * inside / pointsCount;
    }

    private static long GetPoints(long pointsCount)
    {
        var inside = 0;
        var rnd = new Random();

        for (var i = 0; i < pointsCount; i++)
        {
            var x = rnd.NextDouble();
            var y = rnd.NextDouble();

            if (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) <= 1) { inside += 1; }
        }

        return inside;
    }
}
