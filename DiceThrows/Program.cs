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
            var numbers = GetThrowsMultiThreaded(dice, attempts, iterations);
            watch.Stop();


            var grp = numbers.GroupBy(i => i);
            grp = grp.OrderBy(i => i.Key);
            
            // uncomment to write results to console
            // foreach (var x in grp)
            // {
            //     Console.WriteLine($"{x.Key}: {x.Count()}");
            // }
            Console.WriteLine($"Execution took {watch.Elapsed}.");
        }
        
        private static List<int> GetThrows(int dice, int numThrows, int iterations)
        {
            var rnd = new Random();
            var numbers = new List<int>();
            
            for (var i = 0; i < iterations; i++)
            {
                var number = 0;
                for (var j = 0; j < numThrows; j++)
                {
                    number += rnd.Next(1, dice + 1);
                }
                numbers.Add(number);
            }

            return numbers;
        }

        // 32 cpus is usually maximum for personal computers. C# multithreading will handle if you have less.
        private static List<int> GetThrowsMultiThreaded(int dice, int numThrows, int iterations, int cpus = 32)
        {
            if (iterations < 32000)
            {
                return GetThrows(dice, numThrows, iterations);
            }

            var interval = iterations / cpus;
            var inputList = new List<int>();
            var subResults = new List<List<int>>();
            var numbers = new List<int>();
            
            for (var i = 0; i < cpus; i++) { inputList.Add(interval); }
            inputList[inputList.Count - 1] += iterations % cpus;
            Parallel.ForEach(inputList, sub => subResults.Add(GetThrows(dice, numThrows, sub)));
            subResults.ForEach(subResult => subResult.ForEach(number => numbers.Add(number)));
            return numbers;
        }
    }
}