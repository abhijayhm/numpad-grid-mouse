using System.Windows;
using NumpadGridMouse.Win.Grid;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Hud;

public partial class HelpOverlayWindow : Window
{
    public void CenterOnMonitor(ScreenRegion monitor)
    {
        Loaded += (_, _) =>
        {
            var (sx, sy) = MonitorDpi.GetScaleForRegion(monitor);
            Left = monitor.X / sx + (monitor.Width / sx - ActualWidth) / 2;
            Top = monitor.Y / sy + (monitor.Height / sy - ActualHeight) / 2;
        };
    }
}
