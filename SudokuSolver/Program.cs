using System.Diagnostics;

namespace SudokuSolver;

internal static class Program
{
    public static void Main()
    {
        var watch = new Stopwatch();
        var taskBoard = new Board(new[,]
        {
            {6, 0, 0, 0, 7, 1, 4, 0, 0},
            {1, 8, 5, 0, 0, 9, 2, 0, 0},
            {0, 4, 0, 2, 5, 0, 9, 0, 8},
            {5, 0, 8, 0, 3, 0, 0, 0, 4},
            {0, 7, 3, 0, 0, 0, 6, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0},
            {2, 3, 4, 9, 1, 0, 0, 7, 6},
            {8, 0, 0, 0, 0, 7, 1, 0, 9},
            {7, 1, 0, 6, 8, 3, 0, 0, 0}
        });
        
        var result = new Board(new[,]
        {
            {6, 9, 2, 8, 7, 1, 4, 5, 3},
            {1, 8, 5, 3, 4, 9, 2, 6, 7},
            {3, 4, 7, 2, 5, 6, 9, 1, 8},
            {5, 6, 8, 1, 3, 2, 7, 9, 4},
            {4, 7, 3, 5, 9, 8, 6, 2, 1},
            {9, 2, 1, 7, 6, 4, 3, 8, 5},
            {2, 3, 4, 9, 1, 5, 8, 7, 6},
            {8, 5, 6, 4, 2, 7, 1, 3, 9},
            {7, 1, 9, 6, 8, 3, 5, 4, 2}
        });
        
        var taskBoard2 = new Board(new[,]
        {
            {6, 9, 2, 8, 7, 1, 4, 5, 3},
            {1, 8, 0, 3, 4, 9, 2, 6, 7},
            {3, 4, 7, 2, 5, 6, 9, 1, 8},
            {5, 6, 8, 1, 3, 2, 7, 9, 4},
            {4, 7, 3, 5, 0, 8, 6, 2, 1},
            {9, 0, 1, 7, 6, 4, 3, 8, 5},
            {2, 3, 4, 9, 1, 5, 8, 7, 6},
            {8, 5, 0, 4, 2, 7, 1, 3, 9},
            {7, 1, 9, 6, 8, 3, 5, 4, 0}
        });
        
        watch.Start();
        var solution = SolveBoard(taskBoard);
        watch.Stop();
        
        Console.WriteLine($"Result: {solution.Win()}.");
        Console.WriteLine($"Execution time: {watch.ElapsedMilliseconds} ms.");
    }

    private static Board SolveBoard(Board board)
    {
        foreach (var move in board.ValidMoves())
        {
            board.MakeMove(move);
            board = SolveBoard(board);
            if (board.Win()) { return board; }
            board.UnmakeLastMove();
        }

        return board;
    }
}