using System.Runtime.InteropServices;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Pointer;

internal sealed class PointerService
{
    public void MoveTo(int x, int y)
    {
        NativeMethods.SetCursorPos(x, y);
    }

    public (int X, int Y) GetPosition()
    {
        if (!NativeMethods.GetCursorPos(out var pt))
            return (0, 0);
        return (pt.X, pt.Y);
    }

    public void LeftClick()
    {
        SendMouse(NativeMethods.MouseeventfLeftdown);
        SendMouse(NativeMethods.MouseeventfLeftup);
    }

    public void RightClick()
    {
        SendMouse(NativeMethods.MouseeventfRightdown);
        SendMouse(NativeMethods.MouseeventfRightup);
    }

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
