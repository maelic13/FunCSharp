using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DiceThrows
{
    internal static class Program
    {
        private static IEnumerable<int> GetThrows(int dice, int numThrows, int iterations) 
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

        public static void Main(string[] args) 
        {
            const int dice = 20;
            const int numAttempts = 100;
            const int iterations = 1000000;

            var watch = Stopwatch.StartNew();
            var numbers = GetThrows(dice, numAttempts, iterations);
            watch.Stop();

            var grp = numbers.GroupBy(i => i);
            grp = grp.OrderBy(i => i.Key);
            foreach (var x in grp)
            {
                Console.WriteLine($"{x.Key}: {x.Count()}");
            }
            Console.WriteLine($"Execution took {watch.Elapsed}.");
            Console.ReadLine();
        }
    }
}