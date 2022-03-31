namespace DoorRiddle;

public class Result
{
    public long Failed { get; private set; }
    public long Successful { get; private set; }
    public long Total => Failed + Successful;
    public double SuccessRate => (double)Successful / Total;
        
    public Result()
    {
        Failed = 0;
        Successful = 0;
    }

    public Result(long failed, long successful)
    {
        Failed = failed;
        Successful = successful;
    }

    public static Result Combine(List<Result> results)
    {
        long failed = 0;
        long successful = 0;
            
        foreach (var result in results)
        {
            successful += result.Successful;
            failed += result.Failed;
        }
            
        return new Result(failed, successful);
    }
        
    public new string ToString() => $"{Successful} successful tries, {Total} total. " +
                                    $"Success rate {(SuccessRate * 100):0.0000} %.";

    public void AddTry(bool result)
    {
        if (result)
        {
            Successful += 1;
        }
        else
        {
            Failed += 1; 
        }
    }
}