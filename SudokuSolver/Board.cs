namespace SudokuSolver;

internal class Board
{
    public int[,] CurrentBoard { get; set; }
    private List<Move> MoveHistory { get; }
    public int[,] Root { get; }
    private int BoardSizeIndex;
    private int BoardSize => (int)Math.Pow(BoardSizeIndex, 2);

    public Board(int[,] board)
    {
        CurrentBoard = (int[,])board.Clone();
        MoveHistory = new List<Move> { };
        Root = (int[,])board.Clone();
        BoardSizeIndex = (int) Math.Pow(board.Length, 1.0 / 4);

        if ((int)Math.Pow(BoardSizeIndex, 4) != board.Length)
        {
            throw new Exception("Board of wrong size provided.");
        }
    }

    public Board(int boardSizeIndex = 3) : this(EmptyBoard(boardSizeIndex)) { }

    private static int[,] EmptyBoard(int boardSizeIndex)
    {
        var boardSize = (int)Math.Pow(boardSizeIndex, 2);
        return new int[boardSize, boardSize];
    }

    public bool Equals(Board other)
    {
        return true;
    }

    public int UnfilledCount()
    {
        return CurrentBoard.Cast<int>().Count(number => number == 0);
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
        return AllColumnsValid() && AllColumnsValid() && AllSectorsValid();
    }

    public bool Win()
    {
        return Filled() && Valid();
    }

    public IOrderedEnumerable<Move> ValidMoves()
    {
        var validMoves = new List<Move>();

        foreach (var move in PossibleMoves())
        {
            MakeMove(move);
            if (Valid()) { validMoves.Add(move); }
            UnmakeLastMove();
        }
        
        return SortMoves(validMoves);
    }

    private IOrderedEnumerable<Move> SortMoves(List<Move> moves)
    {
        var counter = new Dictionary<(int, int), int>();

        foreach (var move in moves)
        {
            if (!counter.ContainsKey(move.Square))
            {
                counter.Add(move.Square, 1);
                continue;
            }
            counter[move.Square]++;
        }

        return moves.OrderBy(move => counter[move.Square]);
    }

    private List<Move> PossibleMoves()
    {
        var possibleMoves = new List<Move>();
        for (var rowIndex = 0; rowIndex < BoardSize; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < BoardSize; columnIndex++)
            {
                if (CurrentBoard[rowIndex, columnIndex] != 0) { continue; }

                for (var number = 0; number < BoardSize; number++)
                {
                    possibleMoves.Add(new Move(rowIndex, columnIndex, number + 1));
                }
            }
        }
        return possibleMoves;
    }

    public bool MakeMove(Move move)
    {
        if (CurrentBoard[move.Row, move.Column] != 0) { return false; }

        MoveHistory.Add(move);
        CurrentBoard[move.Row, move.Column] = move.Number;
        return true;
    }

    private List<(int, int)> GetAllSectorStartingIndices()
    {
        var startingIndices = new List<(int, int)>();

        for (var i = 0; i < BoardSize; i += BoardSizeIndex)
        {
            for (var j = 0; j < BoardSize; j += BoardSizeIndex)
            {
                startingIndices.Add((i, j));
            }
        }
        
        return startingIndices;
    }

    public bool UnmakeLastMove()
    {
        if (MoveHistory.Count == 0) { return false; }

        var move = MoveHistory[^1];
        CurrentBoard[move.Row, move.Column] = 0;
        MoveHistory.Remove(move);
        return true;
    }

    public IEnumerable<int> Column(int columnIndex)
    {
        var column = new int[BoardSize];

        for (var i = 0; i < BoardSize; i++)
        {
            column[i] = CurrentBoard[i, columnIndex];
        }
        return column;
    }

    public IEnumerable<int> Row(int rowIndex)
    {
        var row = new int[BoardSize];

        for (var i = 0; i < BoardSize; i++)
        {
            row[i] = CurrentBoard[rowIndex, i];
        }
        return row;
    }

    public int[,] Sector(int startRow, int startColumn)
    {
        var sector = new int[BoardSizeIndex, BoardSizeIndex];

        for (var i = startRow; i < startRow + BoardSizeIndex; i++)
        {
            for (var j = startColumn; j < startColumn + BoardSizeIndex; j++)
            {
                sector[i - startRow, j - startColumn] = CurrentBoard[i, j];
            }
        }
        return sector;
    }

    private bool AllColumnsValid()
    {
        var allValid = true;

        for (var i = 0; i < BoardSize; i++)
        {
            allValid = allValid && ColumnValid(i);
        }

        return allValid;
    }

    private bool AllRowsValid()
    {
        var allValid = true;

        for (var i = 0; i < BoardSize; i++)
        {
            allValid = allValid && RowValid(i);
        }

        return allValid;
    }

    private bool AllSectorsValid()
    {
        var allSectors = GetAllSectorStartingIndices();
        var allValid = true;
        
        foreach (var sectorIndices in allSectors)
        {
            var (rowIndex, columnIndex) = sectorIndices;
            allValid = allValid && SectorValid(Sector(rowIndex, columnIndex));
        }

        return allValid;
    }

    private bool ColumnValid(int columnIndex)
    {
        var column = Column(columnIndex);
        var hashSet = new HashSet<double>();

        return column.Where(number => number != 0).Aggregate(true, (current, number) 
            => current && hashSet.Add(number));
    }

    private bool RowValid(int rowIndex)
    {
        var row = Row(rowIndex);
        var hashSet = new HashSet<double>();
        
        return row.Where(number => number != 0).Aggregate(true, (current, number) 
            => current && hashSet.Add(number));
    }

    private bool SectorValid(int[,] sector)
    {
        var hashSet = new HashSet<double>();

        for (var i = 0; i < BoardSizeIndex; i++)
        {
            for (var j = 0; j < BoardSizeIndex; j++)
            {
                if (sector[i, j] != 0 && !hashSet.Add(sector[i, j]))
                {
                    return false;
                }
            }
        }
        return true;
    } 
}