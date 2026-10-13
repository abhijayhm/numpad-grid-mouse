using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Hotkeys;

internal sealed class HotkeySinkWindow : Window
{
    public event Action<int>? HotkeyPressed;

    public HotkeySinkWindow()
    {
        Width = 0;
        Height = 0;
        WindowStyle = WindowStyle.None;
        ShowInTaskbar = false;
        ShowActivated = false;
        Visibility = Visibility.Hidden;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        if (PresentationSource.FromVisual(this) is HwndSource source)
            source.AddHook(WndProc);
    }

    private nint WndProc(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
    {
        if (msg == NativeMethods.WmHotkey)
        {
            var id = wParam.ToInt32();
            HotkeyPressed?.Invoke(id);
            handled = true;
        }

        return nint.Zero;
    }

    public nint Handle => new WindowInteropHelper(this).EnsureHandle();
}
