using System.Runtime.InteropServices;
using NumpadGridMouse.Win.Grid;

namespace NumpadGridMouse.Win.Native;

internal static class MonitorDpi
{
    private const int MdtEffectiveDpi = 0;

    [DllImport("Shcore.dll")]
    private static extern int GetDpiForMonitor(nint hMonitor, int dpiType, out uint dpiX, out uint dpiY);

    public static (double ScaleX, double ScaleY) GetScaleForRegion(ScreenRegion region)
    {
        var (cx, cy) = region.Center();
        var pt = new POINT { X = cx, Y = cy };
        var hMon = NativeMethods.MonitorFromPoint(pt, NativeMethods.MonitorDefaulttonearest);
        if (GetDpiForMonitor(hMon, MdtEffectiveDpi, out var dpiX, out var dpiY) == 0 && dpiX > 0 && dpiY > 0)
            return (dpiX / 96.0, dpiY / 96.0);

        return (1.0, 1.0);
    }
}
