using System.Runtime.InteropServices;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Pointer;

internal sealed class PointerService
{
    public void MoveTo(int x, int y) => NativeMethods.SetCursorPos(x, y);

    public (int X, int Y) GetPosition()
    {
        if (!NativeMethods.GetCursorPos(out var pt))
            return (0, 0);
        return (pt.X, pt.Y);
    }

    public void LeftClick() => ClickAtCursor(
        NativeMethods.MouseeventfLeftdown,
        NativeMethods.MouseeventfLeftup);

    public void RightClick() => ClickAtCursor(
        NativeMethods.MouseeventfRightdown,
        NativeMethods.MouseeventfRightup);

    public void DoubleClick()
    {
        LeftClick();
        LeftClick();
    }

    public void LeftButtonDown() => SendMouse(NativeMethods.MouseeventfLeftdown);

    public void LeftButtonUp() => SendMouse(NativeMethods.MouseeventfLeftup);

    public void RightButtonDown() => SendMouse(NativeMethods.MouseeventfRightdown);

    public void RightButtonUp() => SendMouse(NativeMethods.MouseeventfRightup);

    public void Scroll(int horizontalDelta, int verticalDelta)
    {
        if (verticalDelta != 0)
        {
            var clicks = verticalDelta / NativeMethods.WheelDelta;
            if (clicks == 0)
                clicks = verticalDelta > 0 ? 1 : -1;
            SendWheel((uint)(clicks * NativeMethods.WheelDelta), vertical: true);
        }

        if (horizontalDelta != 0)
        {
            var clicks = horizontalDelta / NativeMethods.WheelDelta;
            if (clicks == 0)
                clicks = horizontalDelta > 0 ? 1 : -1;
            SendWheel((uint)(clicks * NativeMethods.WheelDelta), vertical: false);
        }
    }

    private void ClickAtCursor(uint downFlag, uint upFlag)
    {
        if (!NativeMethods.GetCursorPos(out var pt))
            return;

        var vx = NativeMethods.GetSystemMetrics(NativeMethods.SmXvirtualscreen);
        var vy = NativeMethods.GetSystemMetrics(NativeMethods.SmYvirtualscreen);
        var vw = NativeMethods.GetSystemMetrics(NativeMethods.SmCxvirtualscreen);
        var vh = NativeMethods.GetSystemMetrics(NativeMethods.SmCyvirtualscreen);

        var absX = (int)(((pt.X - vx) * 65535.0) / Math.Max(1, vw - 1));
        var absY = (int)(((pt.Y - vy) * 65535.0) / Math.Max(1, vh - 1));

        var move = new INPUT
        {
            Type = NativeMethods.InputMouse,
            U = new InputUnion
            {
                Mouse = new MOUSEINPUT
                {
                    Dx = absX,
                    Dy = absY,
                    Flags = NativeMethods.MouseeventfMove
                        | NativeMethods.MouseeventfAbsolute
                        | NativeMethods.MouseeventfVirtualdesk,
                },
            },
        };

        var down = new INPUT
        {
            Type = NativeMethods.InputMouse,
            U = new InputUnion { Mouse = new MOUSEINPUT { Flags = downFlag } },
        };

        var up = new INPUT
        {
            Type = NativeMethods.InputMouse,
            U = new InputUnion { Mouse = new MOUSEINPUT { Flags = upFlag } },
        };

        NativeMethods.SendInput(3, [move, down, up], Marshal.SizeOf<INPUT>());
    }

    private static void SendMouse(uint flag)
    {
        var input = new INPUT
        {
            Type = NativeMethods.InputMouse,
            U = new InputUnion
            {
                Mouse = new MOUSEINPUT { Flags = flag },
            },
        };
        NativeMethods.SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }

    private static void SendWheel(uint data, bool vertical)
    {
        var input = new INPUT
        {
            Type = NativeMethods.InputMouse,
            U = new InputUnion
            {
                Mouse = new MOUSEINPUT
                {
                    Flags = vertical ? NativeMethods.MouseeventfWheel : NativeMethods.MouseeventfHwheel,
                    MouseData = data,
                },
            },
        };
        NativeMethods.SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }
}
