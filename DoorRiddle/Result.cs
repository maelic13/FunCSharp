using System.Collections.Generic;

namespace DoorRiddle
{
    public class Result
    {
        public int Successful { get; private set; }
        public int Total { get; private set; }
        public int Failed => Total - Successful;
        public double SuccessRate => (double)Successful / Total;
        
        public Result()
        {
            Successful = 0;
            Total = 0;
        }

        public Result(int successful, int total)
        {
            Successful = successful;
            Total = total;
        }

        public static Result Combine(List<Result> results)
        {
            var successful = 0;
            var total = 0;
            
            foreach (var result in results)
            {
                successful += result.Successful;
                total += result.Total;
            }
            
            return new Result(successful, total);
        }
        
        public new string ToString() => $"{Successful} successful tries, {Total} total. " +
                                        $"Success rate {SuccessRate * 100} %.";

        public void AddTry(bool result)
        {
            Total += 1;
            if (result) { Successful += 1; }
        }
    }
}