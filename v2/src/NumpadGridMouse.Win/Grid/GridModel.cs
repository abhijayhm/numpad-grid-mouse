namespace NumpadGridMouse.Win.Grid;

public sealed class GridModel
{
    public static readonly IReadOnlyDictionary<int, (int Row, int Col)> NumpadMap =
        new Dictionary<int, (int, int)>
        {
            [1] = (2, 0),
            [2] = (2, 1),
            [3] = (2, 2),
            [4] = (1, 0),
            [5] = (1, 1),
            [6] = (1, 2),
            [7] = (0, 0),
            [8] = (0, 1),
            [9] = (0, 2),
        };

    private readonly int _maxDepth;
    private readonly List<ScreenRegion> _stack;

    public GridModel(ScreenRegion screenRegion, int maxDepth = 0)
    {
        _maxDepth = maxDepth;
        _stack = [screenRegion];
        CurrentRegion = screenRegion;
    }

    public ScreenRegion CurrentRegion { get; private set; }

    public int Depth => _stack.Count - 1;

    public ScreenRegion? SelectRegion(int numpadKey)
    {
        if (!NumpadMap.TryGetValue(numpadKey, out var pos))
            return null;

        if (_maxDepth > 0 && _stack.Count > _maxDepth)
            return null;

        var newRegion = CurrentRegion.Subdivide(pos.Row, pos.Col);
        if (newRegion.Width < 6 || newRegion.Height < 6)
            return null;

        _stack.Add(newRegion);
        CurrentRegion = newRegion;
        return newRegion;
    }

    public ScreenRegion? GoUp()
    {
        if (_stack.Count <= 1)
            return null;
        _stack.RemoveAt(_stack.Count - 1);
        CurrentRegion = _stack[^1];
        return CurrentRegion;
    }

    public void Reset(ScreenRegion screenRegion)
    {
        _stack.Clear();
        _stack.Add(screenRegion);
        CurrentRegion = screenRegion;
    }

    public ScreenRegion Root => _stack[0];

    public IReadOnlyList<ScreenRegion> GetCellRegions()
    {
        var regions = new List<ScreenRegion>(9);
        for (var row = 0; row < 3; row++)
        for (var col = 0; col < 3; col++)
            regions.Add(CurrentRegion.Subdivide(row, col));
        return regions;
    }

    public static int CellLabel(int row, int col)
    {
        foreach (var (key, pos) in NumpadMap)
        {
            if (pos.Row == row && pos.Col == col)
                return key;
        }

        return 0;
    }
}
