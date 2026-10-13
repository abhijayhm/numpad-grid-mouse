using System.Globalization;
using System.Windows;
using System.Windows.Media;
using NumpadGridMouse.Win.Config;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Grid;

public partial class GridOverlayWindow : Window
{
    private OverlayContentMode _mode = OverlayContentMode.Grid;
    private GridModel? _model;
    private ScreenRegion? _region;
    private AppConfig _config = new();
    private string _hudText = "";

    public GridOverlayWindow()
    {
        InitializeComponent();
    }

    public void ShowGrid(ScreenRegion region, GridModel model, AppConfig config, string hudText)
    {
        _mode = OverlayContentMode.Grid;
        _region = region;
        _model = model;
        _config = config;
        _hudText = hudText;
        DipWindowPlacement.Place(this, region);
        InvalidateVisual();
    }

    public void ShowHelp(ScreenRegion region, AppConfig config)
    {
        _mode = OverlayContentMode.Help;
        _region = region;
        _model = null;
        _config = config;
        _hudText = "";
        DipWindowPlacement.Place(this, region);
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);
        if (_region is null)
            return;

        if (_mode == OverlayContentMode.Help)
        {
            DrawHelp(dc);
            return;
        }

        DrawGrid(dc);
    }

    private void DrawGrid(DrawingContext dc)
    {
        var w = ActualWidth;
        var h = ActualHeight;
        if (w < 1 || h < 1 || _model is null)
            return;

        var border = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(_config.Grid.BorderColor)!;
        var textBrush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(_config.Grid.TextColor)!;
        var pen = new System.Windows.Media.Pen(border, Math.Max(1, _config.Grid.LineThickness));
        var dip = VisualTreeHelper.GetDpi(this).PixelsPerDip;

        for (var row = 0; row < 3; row++)
        for (var col = 0; col < 3; col++)
        {
            var cell = LocalCellRect(w, h, row, col);
            dc.DrawRectangle(null, pen, cell);
            var label = GridModel.CellLabel(row, col);
            var fontSize = Math.Max(10, Math.Min(_config.Grid.FontSize, Math.Min(cell.Width, cell.Height) * 0.38));
            var ft = new FormattedText(
                label.ToString(),
                CultureInfo.InvariantCulture,
                System.Windows.FlowDirection.LeftToRight,
                new Typeface("Segoe UI Semibold"),
                fontSize,
                textBrush,
                dip);
            dc.DrawText(
                ft,
                new System.Windows.Point(cell.X + cell.Width / 2 - ft.Width / 2, cell.Y + cell.Height / 2 - ft.Height / 2));
        }

        DrawHud(dc, dip);
    }

    private void DrawHelp(DrawingContext dc)
    {
        var w = ActualWidth;
        var h = ActualHeight;
        var dip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        var bg = new SolidColorBrush(System.Windows.Media.Color.FromArgb(230, 15, 23, 42));
        dc.DrawRectangle(bg, null, new Rect(0, 0, w, h));

        const string body = """
            Win + /          Grid on · Win + / again off
            Win + Shift + ?  This help (toggle)

            1–9              Aim pointer (zoom cell)
            Win + Enter      Left click · closes grid
            Win + Enter x2   Double-click · closes grid
            Win + Alt        Right click · closes grid
            Backspace        Up one level · Esc close

            Win + Shift + …  scroll ↑↓ · drag ; · path '
            """;
        var text = new FormattedText(
            body,
            CultureInfo.InvariantCulture,
            System.Windows.FlowDirection.LeftToRight,
            new Typeface("Consolas"),
            15,
            System.Windows.Media.Brushes.White,
            dip);
        dc.DrawText(text, new System.Windows.Point(24, 24));
    }

    private void DrawHud(DrawingContext dc, double dip)
    {
        if (!_config.Hud.Enabled || string.IsNullOrEmpty(_hudText))
            return;
        var hudBg = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(_config.Hud.BackgroundColor)!;
        var hudFg = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(_config.Hud.TextColor)!;
        var hud = new FormattedText(
            _hudText,
            CultureInfo.InvariantCulture,
            System.Windows.FlowDirection.LeftToRight,
            new Typeface("Segoe UI"),
            Math.Min(_config.Hud.FontSize, 14),
            hudFg,
            dip);
        var pad = 8;
        var bgRect = new Rect(pad, pad, hud.Width + pad * 2, hud.Height + pad * 2);
        dc.DrawRoundedRectangle(hudBg, null, bgRect, 6, 6);
        dc.DrawText(hud, new System.Windows.Point(pad * 2, pad * 2));
    }

    private static Rect LocalCellRect(double totalW, double totalH, int row, int col)
    {
        var baseW = Math.Floor(totalW / 3);
        var baseH = Math.Floor(totalH / 3);
        var cw = col == 2 ? totalW - 2 * baseW : baseW;
        var ch = row == 2 ? totalH - 2 * baseH : baseH;
        return new Rect(col * baseW, row * baseH, cw, ch);
    }
}
