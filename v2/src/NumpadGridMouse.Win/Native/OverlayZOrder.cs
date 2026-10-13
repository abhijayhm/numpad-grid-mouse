using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace NumpadGridMouse.Win.Native;

/// <summary>
/// Pushes the overlay as high as Win32 allows (HWND_TOPMOST). Cannot beat UAC, lock screen,
/// or some shell surfaces (e.g. certain notification layers) without UIAccess.
/// </summary>
internal static class OverlayZOrder
{
    private const int GwlExstyle = -20;

    private const int WsExTopmost = 0x00000008;
    private const int WsExToolwindow = 0x00000080;
    private const int WsExNoactivate = 0x08000000;

    private static readonly nint HwndTopmost = -1;

    private const uint SwpNomove = 0x0002;
    private const uint SwpNosize = 0x0001;
    private const uint SwpNoactivate = 0x0010;
    private const uint SwpShowwindow = 0x0040;
    private const uint SwpNoownerzorder = 0x0200;

    public static void Wire(Window window)
    {
        window.ShowActivated = false;
        window.Topmost = true;

        void Apply()
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == nint.Zero)
                return;

            var ex = GetWindowLongPtr(hwnd, GwlExstyle);
            var next = ex.ToInt64() | WsExTopmost | WsExToolwindow | WsExNoactivate;
            SetWindowLongPtr(hwnd, GwlExstyle, new nint(next));

            SetWindowPos(
                hwnd,
                HwndTopmost,
                0,
                0,
                0,
                0,
                SwpNomove | SwpNosize | SwpNoactivate | SwpShowwindow | SwpNoownerzorder);
        }

        if (window.IsLoaded)
            Apply();
        else
            window.SourceInitialized += (_, _) => Apply();

        window.Loaded += (_, _) => Raise(window);
    }

    public static void Raise(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        if (hwnd == nint.Zero)
            return;

        SetWindowPos(
            hwnd,
            HwndTopmost,
            0,
            0,
            0,
            0,
            SwpNomove | SwpNosize | SwpNoactivate | SwpShowwindow | SwpNoownerzorder);
        window.Topmost = true;
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern nint GetWindowLongPtr64(nint hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern nint GetWindowLongPtr32(nint hWnd, int nIndex);

    private static nint GetWindowLongPtr(nint hWnd, int nIndex) =>
        nint.Size == 8 ? GetWindowLongPtr64(hWnd, nIndex) : GetWindowLongPtr32(hWnd, nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern nint SetWindowLongPtr64(nint hWnd, int nIndex, nint dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern nint SetWindowLongPtr32(nint hWnd, int nIndex, nint dwNewLong);

    private static nint SetWindowLongPtr(nint hWnd, int nIndex, nint dwNewLong) =>
        nint.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : SetWindowLongPtr32(hWnd, nIndex, dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(
        nint hWnd,
        nint hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        uint uFlags);
}
