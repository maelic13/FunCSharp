using System.Diagnostics;

namespace DoorRiddle;

internal static class Program
{
    public static void Main(string[] args)
        /*
         Experimental solution to following statistical riddle.
         You are given 3 doors with treasure behind 1 of them. You choose 1 door, and then the host opens 1
         of the remaining doors (and always chooses the one behind which the treasure is not).
         You are then given a choice - do you open the door you chose at the beginning, or do you change your choice
         and open the remaining closed door instead? Which option maximizes your chances of finding treasure?
         */
    {
        const long cycles = 1000000000;
        var cpus = Environment.ProcessorCount;

        var watch = Stopwatch.StartNew();
        var resultWithChange = PlayGameMultiThreaded(cycles, true, cpus);
        watch.Stop();
        PrintResultsToConsole(resultWithChange, watch, cycles, true);

        watch.Restart();
        var resultNoChange = PlayGameMultiThreaded(cycles, false, cpus);
        watch.Stop();
        PrintResultsToConsole(resultNoChange, watch, cycles, false);
    }

    private static Result PlayGame(long cycles, bool changeChoice)
    {
        var doors = new List<bool> {false, true, false};
        var rnd = new Random();
        var result = new Result();

        for (var i = 0; i < cycles; i++)
            result.AddTry(changeChoice != doors[rnd.Next(3)]);
        return result;
    }

    private static Result PlayGameMultiThreaded(long cycles, bool changeChoice, int cpus)
    {
        if (cpus == 1)
            return PlayGame(cycles, changeChoice);

        var interval = cycles / cpus;
        var inputList = new List<long>();
        var results = new List<Result>();

        for (var i = 0; i < cpus; i++)
            inputList.Add(interval);
        inputList[^1] += cycles % cpus;

        Parallel.ForEach(inputList, sub => results.Add(PlayGame(sub, changeChoice)));
        return Result.Combine(results);
    }

    private static void PrintResultsToConsole(Result result, Stopwatch watch, long cycles, bool changeChoice)
    {
        Console.WriteLine();
        Console.WriteLine($"Change = {changeChoice}. Time elapsed {watch.ElapsedMilliseconds} ms.");
        Console.WriteLine(result.ToString());
        Console.WriteLine($"Speed = {cycles / watch.ElapsedMilliseconds / 1000} Miter/s.");
    }
}