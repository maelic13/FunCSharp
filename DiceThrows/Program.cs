using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DiceThrows
{
    internal static class Program
    {
        public static void Main(string[] args)
        /*
         Calculate {iterations} sums of {attempts} number of rolls of {dice}-sided dice and return results.
         */
        {
            const int dice = 20;
            const int attempts = 100;
            const int iterations = 1000000;

            var watch = Stopwatch.StartNew();
            var result = GetThrowsMultiThreaded(dice, attempts, iterations, Environment.ProcessorCount);
            watch.Stop();
            
            // result.PrintToConsole();
            Console.WriteLine($"Execution took {watch.Elapsed}.");
        }
        
        private static Result GetThrows(int dice, int numThrows, int iterations)
        {
            var rnd = new Random();
            var result = new Result();
            
            for (var i = 0; i < iterations; i++)
            {
                var number = 0;
                for (var j = 0; j < numThrows; j++)
                {
                    number += rnd.Next(dice) + 1;
                }
                result.AddThrow(number);
            }

            return result;
        }

        private static Result GetThrowsMultiThreaded(int dice, int numThrows, int iterations, int cpus)
        {
            var interval = iterations / cpus;
            var inputList = new List<int>();
            var results = new List<Result>();
            
            for (var i = 0; i < cpus; i++) { inputList.Add(interval); }
            inputList[inputList.Count - 1] += iterations % cpus;
            Parallel.ForEach(inputList, sub => results.Add(GetThrows(dice, numThrows, sub)));
            return Result.Combine(results);
        }
    }
}