using System.Runtime.InteropServices;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Input;

internal sealed class LowLevelKeyboardHook : IDisposable
{
    private readonly NativeMethods.LowLevelKeyboardProc _proc;
    private nint _hookId;

    public KeyboardChordState Chords { get; } = new();

    public event Func<int, bool>? KeyDown;

    public LowLevelKeyboardHook()
    {
        _proc = HookCallback;
    }

    public void Install()
    {
        if (_hookId != nint.Zero)
            return;
        using var curProcess = System.Diagnostics.Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule!;
        _hookId = NativeMethods.SetWindowsHookEx(
            NativeMethods.WhKeyboardLl,
            _proc,
            NativeMethods.GetModuleHandle(curModule.ModuleName),
            0);
    }

    public void Uninstall()
    {
        if (_hookId == nint.Zero)
            return;
        NativeMethods.UnhookWindowsHookEx(_hookId);
        _hookId = nint.Zero;
    }

    private nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode >= 0)
        {
            var vk = Marshal.ReadInt32(lParam);
            var isDown = wParam is NativeMethods.WmKeydown or NativeMethods.WmSyskeydown;
            var isUp = wParam is NativeMethods.WmKeyup or NativeMethods.WmSyskeyup;

            if (isDown || isUp)
                Chords.Apply(vk, isDown);

            if (isDown)
            {
                var suppress = KeyDown?.Invoke(vk) == true;
                if (suppress)
                    return 1;
            }
        }

        return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    public void Dispose() => Uninstall();
}
