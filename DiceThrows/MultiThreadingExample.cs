using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiceThrows
{
    public class MultiThreadingExample
    {
        public static void Example()
        {
            var testList = new List<int>();
            testList.AddRange(Enumerable.Range(1, 20));

            Parallel.ForEach(testList, x => Console.WriteLine(Test(x)));
        }

        private static string Test(int i)
        {
            Thread.Sleep(1000);
            return "P: " + Process.GetCurrentProcess().Id.ToString() + ", T: " + Thread.CurrentThread.ManagedThreadId.ToString();
        }
    }
}