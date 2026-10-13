namespace NumpadGridMouse.Win.Grid;

public sealed class ScreenRegion
{
    public ScreenRegion(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    public (int X, int Y) Center() => (X + Width / 2, Y + Height / 2);

    public ScreenRegion Subdivide(int row, int col)
    {
        var cellWidth = Width / 3;
        var cellHeight = Height / 3;
        var x = X + col * cellWidth;
        var y = Y + row * cellHeight;
        if (col == 2)
            cellWidth = Width - 2 * (Width / 3);
        if (row == 2)
            cellHeight = Height - 2 * (Height / 3);
        return new ScreenRegion(x, y, cellWidth, cellHeight);
    }
}
