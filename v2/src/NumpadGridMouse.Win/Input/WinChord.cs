using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Input;

internal static class WinChord
{
    private const int VkShift = 0x10;
    private const int VkLWin = 0x5B;
    private const int VkRWin = 0x5C;

    public const int VkReturn = 0x0D;
    public const int VkMenu = 0x12;
    public const int VkLMenu = 0xA4;
    public const int VkRMenu = 0xA5;
    public const int VkOemSemicolon = 0xBA;
    public const int VkOemQuote = 0xDE;
    public const int VkOemSlash = 0xBF;

    public static bool IsWinDown() =>
        (NativeMethods.GetAsyncKeyState(VkLWin) & 0x8000) != 0
        || (NativeMethods.GetAsyncKeyState(VkRWin) & 0x8000) != 0;

    public static bool IsShiftDown() => (NativeMethods.GetAsyncKeyState(VkShift) & 0x8000) != 0;

    public static bool IsWinShift() => IsWinDown() && IsShiftDown();

    public static bool IsAltKey(int vk) =>
        vk == VkMenu || vk == VkLMenu || vk == VkRMenu;
}
