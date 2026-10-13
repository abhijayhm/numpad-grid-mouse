using System.Runtime.InteropServices;
using NumpadGridMouse.Win.Grid;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Monitors;

internal sealed class MonitorService
{
    private readonly List<ScreenRegion> _enumBuffer = new();

    public ScreenRegion GetMonitorUnderCursor()
    {
        if (!NativeMethods.GetCursorPos(out var pt))
            return GetPrimaryMonitor();
        return GetMonitorWorkAreaAt(pt.X, pt.Y);
    }

    public ScreenRegion GetPrimaryMonitor()
    {
        var list = GetAllWorkAreas();
        foreach (var m in list)
        {
            if (m.X == 0 && m.Y == 0)
                return m;
        }

        return list.Count > 0 ? list[0] : new ScreenRegion(0, 0, 1920, 1080);
    }

    public IReadOnlyList<ScreenRegion> GetAllWorkAreas()
    {
        _enumBuffer.Clear();
        var proc = new NativeMethods.MonitorEnumProc(EnumCallback);
        NativeMethods.EnumDisplayMonitors(nint.Zero, nint.Zero, proc, nint.Zero);
        return _enumBuffer.ToList();
    }

    private bool EnumCallback(nint hMonitor, nint hdcMonitor, ref RECT lprcMonitor, nint dwData)
    {
        var info = new MONITORINFO { CbSize = (uint)Marshal.SizeOf<MONITORINFO>() };
        if (NativeMethods.GetMonitorInfo(hMonitor, ref info))
        {
            var w = info.RcWork;
            _enumBuffer.Add(new ScreenRegion(w.Left, w.Top, w.Right - w.Left, w.Bottom - w.Top));
        }

        return true;
    }

    private ScreenRegion GetMonitorWorkAreaAt(int x, int y)
    {
        var pt = new POINT { X = x, Y = y };
        var hMon = NativeMethods.MonitorFromPoint(pt, NativeMethods.MonitorDefaulttonearest);
        var info = new MONITORINFO { CbSize = (uint)Marshal.SizeOf<MONITORINFO>() };
        if (!NativeMethods.GetMonitorInfo(hMon, ref info))
            return new ScreenRegion(0, 0, 1920, 1080);
        var w = info.RcWork;
        return new ScreenRegion(w.Left, w.Top, w.Right - w.Left, w.Bottom - w.Top);
    }
}
