namespace NumpadGridMouse.Win.Input;

internal sealed class KeyboardChordState
{
    private const int VkLWin = 0x5B;
    private const int VkRWin = 0x5C;
    private const int VkShift = 0x10;
    private const int VkLShift = 0xA0;
    private const int VkRShift = 0xA1;
    private const int VkMenu = 0x12;
    private const int VkLMenu = 0xA4;
    private const int VkRMenu = 0xA5;

    private bool _winHeld;
    private bool _shiftHeld;

    public void Apply(int vk, bool isKeyDown)
    {
        if (IsWinVk(vk))
            _winHeld = isKeyDown;
        else if (IsShiftVk(vk))
            _shiftHeld = isKeyDown;
    }

    public void ReconcileWithAsync()
    {
        if (!WinChord.IsWinDown())
            _winHeld = false;
        if (!WinChord.IsShiftDown())
            _shiftHeld = false;
    }

    public bool WinHeld => _winHeld || WinChord.IsWinDown();

    public bool WinWithShift => WinHeld && (_shiftHeld || WinChord.IsShiftDown());

    /// <summary>Win held without shift — Win+Enter / Win+Alt pointer chords.</summary>
    public bool WinAloneForPointerChord => WinHeld && !(_shiftHeld || WinChord.IsShiftDown());

    public static bool IsAltVk(int vk) => vk is VkMenu or VkLMenu or VkRMenu;

    private static bool IsWinVk(int vk) => vk is VkLWin or VkRWin;
    private static bool IsShiftVk(int vk) => vk is VkShift or VkLShift or VkRShift;
}
