namespace DiceThrows;

public class Result
{
    public Result()
    {
        Throws = new SortedDictionary<int, int>();
    }

    public Result(Dictionary<int, int> throws)
    {
        Throws = new SortedDictionary<int, int>(throws);
    }

    public SortedDictionary<int, int> Throws { get; }

    public void AddThrow(int number)
    {
        if (Throws.TryGetValue(number, out _))
            Throws[number]++;
        else
            Throws.Add(number, 1);
    }

    public static Result Combine(List<Result> results)
    {
        var throws = new Dictionary<int, int>();
        foreach (var result in results)
            throws = throws.Union(result.Throws)
                .GroupBy(g => g.Key)
                .ToDictionary(pair => pair.Key, pair => pair.First().Value);

        return new Result(throws);
    }

    public void PrintToConsole()
    {
        foreach (var diceThrow in Throws) Console.WriteLine($"{diceThrow.Key}: {diceThrow.Value}");
    }
}