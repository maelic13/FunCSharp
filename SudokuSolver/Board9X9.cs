namespace SudokuSolver;

internal class Board9X9 : Board
{
    public Board9X9() : base(new int[9, 9]) {}
    public Board9X9(int[,] board) : base(board) {}

    protected override (int, int) GetStartingIndices(SectorId sectorId)
    {
        return sectorId switch
        {
            SectorId.TopLeft => (0, 0),
            SectorId.Top => (0, 3),
            SectorId.TopRight => (0, 6),
            SectorId.CenterLeft => (3, 0),
            SectorId.Center => (3, 3),
            SectorId.CenterRight => (3, 6),
            SectorId.BottomLeft => (6, 0),
            SectorId.Bottom => (6, 3),
            SectorId.BottomRight => (6, 6),
            _ => throw new ArgumentOutOfRangeException(nameof(sectorId), sectorId, null),
        };
    }

    protected override int SectorSize() { return 3; }

    public override Array Column(int columnIndex)
    {
        var column = new int[9];
        for (int i = 0; i < 9; i++)
        {
            column[i] = CurrentBoard[i, columnIndex];
        }
        return column;
    }

    public override Array Row(int rowIndex)
    {
        var row = new int[9];
        for (int i = 0; i < 9; i++)
        {
            row[i] = CurrentBoard[rowIndex, i];
        }
        return row;
    }
}