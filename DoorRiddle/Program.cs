﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DoorRiddle
{
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
            const int cycles = 1000000000;

            var watch1 = Stopwatch.StartNew();
            var resultWithChange = PlayGameMultiThreaded(cycles, true);
            watch1.Stop();
            Console.WriteLine($"Change = True. Time elapsed {watch1.Elapsed}");
            Console.WriteLine(resultWithChange.ToString());
            Console.WriteLine($"Speed = {cycles / (watch1.ElapsedMilliseconds / 1000) / 1000000} Miter/s.");

            Console.WriteLine();
            
            var watch2 = Stopwatch.StartNew();
            var resultNoChange = PlayGameMultiThreaded(cycles, false);
            watch2.Stop();
            Console.WriteLine($"Change = False. Time elapsed {watch2.Elapsed}");
            Console.WriteLine(resultNoChange.ToString());
            Console.WriteLine($"Speed = {cycles / (watch2.ElapsedMilliseconds / 1000) / 1000000} Miter/s.");
        }
        
        private static Result PlayGame(int cycles, bool changeChoice = false)
        {
            var doors = new bool[3];
            doors[0] = false;
            doors[1] = true;
            doors[2] = false;
            var rnd = new Random();
            var result = new Result();

            for (var i = 0; i < cycles; i++)
            {
                result.AddTry(changeChoice != doors[rnd.Next(3)]);
            }
            
            return result;
        }
        
        // 32 cpus is usually maximum for personal computers. C# multithreading will handle if you have less.
        private static Result PlayGameMultiThreaded(int cycles, bool changeChoice = false, int cpus = 32)
        {
            if (cycles < 1000000)
            {
                return PlayGame(cycles, changeChoice);
            }
            
            var interval = cycles / cpus;
            var inputList = new List<int>();
            var results = new List<Result>();
            
            for (var i = 0; i < cpus; i++) { inputList.Add(interval); }
            inputList[inputList.Count - 1] += cycles % cpus;
            
            Parallel.ForEach(inputList, sub => results.Add(PlayGame(sub, changeChoice)));
            return Result.Combine(results);
        }
    }
}