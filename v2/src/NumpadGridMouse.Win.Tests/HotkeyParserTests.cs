using NumpadGridMouse.Win.Hotkeys;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Tests;

public class HotkeyParserTests
{
    [Fact]
    public void Parse_WinSlash()
    {
        var parsed = HotkeyParser.Parse("Win+/");
        Assert.Equal(NativeMethods.ModWin | NativeMethods.ModNorepeat, parsed.Modifiers & ~NativeMethods.ModNorepeat | NativeMethods.ModNorepeat);
        Assert.True((parsed.Modifiers & NativeMethods.ModWin) != 0);
    }

    [Fact]
    public void Parse_WinShiftSlash()
    {
        var parsed = HotkeyParser.Parse("Win+Shift+/");
        Assert.True((parsed.Modifiers & NativeMethods.ModWin) != 0);
        Assert.True((parsed.Modifiers & NativeMethods.ModShift) != 0);
    }
}
