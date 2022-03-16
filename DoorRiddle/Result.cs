using System.Collections.Generic;

namespace DoorRiddle
{
    public class Result
    {
        public long Successful { get; private set; }
        public long Total { get; private set; }
        public long Failed => Total - Successful;
        public double SuccessRate => (double)Successful / Total;
        
        public Result()
        {
            Successful = 0;
            Total = 0;
        }

        public Result(long successful, long total)
        {
            Successful = successful;
            Total = total;
        }

        public static Result Combine(List<Result> results)
        {
            long successful = 0;
            long total = 0;
            
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