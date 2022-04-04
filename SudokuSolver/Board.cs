namespace SudokuSolver;

internal abstract class Board
{
    public int[,] CurrentBoard { get; set; }
    private List<Move> MoveHistory { get; set; }
    public int[,] Root { get; }

    public Board(int[,] board)
    {
        CurrentBoard = (int[,])board.Clone();
        MoveHistory = new List<Move> { };
        Root = (int[,])board.Clone();
    }

    protected abstract (int, int) GetStartingIndices(SectorId sectorId);
    protected abstract int SectorSize();
    public abstract Array Column(int columnIndex);
    public abstract Array Row(int rowIndex);

    public int UnfilledCount()
    {
        var count = 0;
        foreach (var number in CurrentBoard)
        {
            if (number == 0)
            {
                count++;
            }
        }
        return count;
    }

    public bool Filled()
    {
        return UnfilledCount() == 0;
    }

    public void Reset()
    {
        CurrentBoard = (int[,])Root.Clone();
    }

    public bool Valid()
    {
        return true;
    }

    public bool Win()
    {
        return Filled() && Valid();
    }

    public List<Move> ValidMoves()
    {
        return new List<Move> { };
    }

    private List<Move> PossibleMoves()
    {
        return new List<Move> { };
    }

    public bool MakeMove(Move move)
    {
        if (CurrentBoard[move.Row, move.Column] != 0) { return false; }

        MoveHistory.Add(move);
        CurrentBoard[move.Row, move.Column] = move.Number;
        return true;
    }

    public bool UnmakeLastMove()
    {
        if (MoveHistory.Count == 0) { return false; }

        var move = MoveHistory[MoveHistory.Count - 1];
        CurrentBoard[move.Row, move.Column] = 0;
        MoveHistory.Remove(move);
        return true;
    }

    public int[,] Sector(SectorId sectorId)
    {
        var size = SectorSize();
        var sector = new int[size, size];
        (var row, var column) = GetStartingIndices(sectorId);

        for (int i = row; i < row + size; i++)
        {
            for (int j = column; j < column + size; j++)
            {
                sector[i - size, j - size] = CurrentBoard[i, j];
            }
        }
        return sector;
    }
}