using NumpadGridMouse.Win.Grid;

namespace NumpadGridMouse.Win.Tests;

public class GridModelTests
{
    [Fact]
    public void Region_Center()
    {
        var region = new ScreenRegion(10, 20, 100, 200);
        var (x, y) = region.Center();
        Assert.Equal(60, x);
        Assert.Equal(120, y);
    }

    [Fact]
    public void SelectRegion_And_GoUp()
    {
        var model = new GridModel(new ScreenRegion(0, 0, 900, 900));
        var r7 = model.SelectRegion(7);
        Assert.NotNull(r7);
        Assert.Equal(0, r7!.X);
        Assert.Equal(300, r7.Width);

        model.SelectRegion(5);
        Assert.Equal(2, model.Depth);
        model.GoUp();
        Assert.Equal(1, model.Depth);
    }

    [Fact]
    public void SelectRegion_StopsWhenCellTooSmall()
    {
        var model = new GridModel(new ScreenRegion(0, 0, 100, 100));
        for (var i = 0; i < 8; i++)
            model.SelectRegion(5);
        var before = model.Depth;
        Assert.Null(model.SelectRegion(5));
        Assert.Equal(before, model.Depth);
    }

    [Fact]
    public void MaxDepth_BlocksDeeper()
    {
        var model = new GridModel(new ScreenRegion(0, 0, 900, 900), maxDepth: 2);
        model.SelectRegion(5);
        model.SelectRegion(5);
        Assert.Null(model.SelectRegion(5));
        Assert.Equal(2, model.Depth);
    }
}
