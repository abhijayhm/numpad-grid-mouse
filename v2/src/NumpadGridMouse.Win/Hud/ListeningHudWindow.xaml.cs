using System.Windows;
using NumpadGridMouse.Win.Grid;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Hud;

public partial class ListeningHudWindow : Window
{
    public void PositionBottomLeft(ScreenRegion monitor, int marginPhysical = 16)
    {
        Loaded += (_, _) =>
        {
            var (sx, sy) = MonitorDpi.GetScaleForRegion(monitor);
            var m = marginPhysical / sx;
            Left = monitor.X / sx + m;
            Top = (monitor.Y + monitor.Height) / sy - ActualHeight - m;
        };
    }
}
