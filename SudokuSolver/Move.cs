namespace SudokuSolver;

internal class Move
{
    public int Row;
    public int Column;
    public int Number;

    public Move(int row, int column, int number)
    {
        Row = row;
        Column = column;
        Number = number;
    }

    public (int, int) Square()
    {
        return (Row, Column);
    }
}
