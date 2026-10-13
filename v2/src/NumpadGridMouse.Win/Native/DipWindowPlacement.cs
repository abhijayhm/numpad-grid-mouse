using System.Windows;
using NumpadGridMouse.Win.Grid;

namespace NumpadGridMouse.Win.Native;

internal static class DipWindowPlacement
{
    public static void Place(Window window, ScreenRegion region)
    {
        void Apply()
        {
            var (sx, sy) = MonitorDpi.GetScaleForRegion(region);
            window.Left = region.X / sx;
            window.Top = region.Y / sy;
            window.Width = Math.Max(1, region.Width / sx);
            window.Height = Math.Max(1, region.Height / sy);
        }

        if (window.IsLoaded)
            Apply();
        else
            window.SourceInitialized += (_, _) => Apply();
    }
}
