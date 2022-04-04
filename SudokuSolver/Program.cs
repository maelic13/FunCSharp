namespace SudokuSolver;

internal static class Program
{
    public static void Main()
    {
        var board = new Board9X9();
        Console.WriteLine(board.Row(4));
        Console.WriteLine(board.Column(4));
        Console.WriteLine(board.Sector(SectorId.Center));
    }
}