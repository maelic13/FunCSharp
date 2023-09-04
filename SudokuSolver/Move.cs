namespace SudokuSolver;

internal class Move
{
    public int Column;
    public int Number;
    public int Row;

    public Move(int row, int column, int number)
    {
        Row = row;
        Column = column;
        Number = number;
    }

    public (int, int) Square => (Row, Column);
}