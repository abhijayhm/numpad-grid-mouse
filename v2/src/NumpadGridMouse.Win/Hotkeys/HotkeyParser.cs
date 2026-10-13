using System.Text;
using System.Windows.Input;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Hotkeys;

internal readonly record struct ParsedHotkey(uint Modifiers, uint VirtualKey);

internal static class HotkeyParser
{
    public static ParsedHotkey Parse(string chord)
    {
        if (string.IsNullOrWhiteSpace(chord))
            throw new FormatException("Hotkey chord is empty.");

        uint mods = NativeMethods.ModNorepeat;
        uint vk = 0;

        foreach (var part in chord.Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var token = part.Trim();
            if (token.Length == 0)
                continue;

            switch (token.ToLowerInvariant())
            {
                case "ctrl":
                case "control":
                    mods |= NativeMethods.ModControl;
                    continue;
                case "shift":
                    mods |= NativeMethods.ModShift;
                    continue;
                case "alt":
                    mods |= NativeMethods.ModAlt;
                    continue;
                case "win":
                case "windows":
                    mods |= NativeMethods.ModWin;
                    continue;
            }

            vk = ResolveVirtualKey(token);
        }

        if (vk == 0)
            throw new FormatException($"No key in hotkey chord: {chord}");

        return new ParsedHotkey(mods, vk);
    }

    private static uint ResolveVirtualKey(string token)
    {
        if (token == "/" || token.Equals("oem2", StringComparison.OrdinalIgnoreCase))
            return (uint)KeyInterop.VirtualKeyFromKey(Key.OemQuestion);

        if (token == "?" || token.Equals("oemquestion", StringComparison.OrdinalIgnoreCase))
            return (uint)KeyInterop.VirtualKeyFromKey(Key.OemQuestion);

        if (token == "," || token.Equals("oemcomma", StringComparison.OrdinalIgnoreCase))
            return (uint)KeyInterop.VirtualKeyFromKey(Key.OemComma);

        if (token == "." || token.Equals("oemperiod", StringComparison.OrdinalIgnoreCase))
            return (uint)KeyInterop.VirtualKeyFromKey(Key.OemPeriod);

        if (token == ";" || token.Equals("oem1", StringComparison.OrdinalIgnoreCase))
            return (uint)KeyInterop.VirtualKeyFromKey(Key.OemSemicolon);

        if (token == "'" || token == "\"" || token.Equals("oem7", StringComparison.OrdinalIgnoreCase))
            return (uint)KeyInterop.VirtualKeyFromKey(Key.OemQuotes);

        if (token.Length == 1)
        {
            var c = char.ToUpperInvariant(token[0]);
            return c;
        }

        if (Enum.TryParse<Key>(token, ignoreCase: true, out var wpfKey))
            return (uint)KeyInterop.VirtualKeyFromKey(wpfKey);

        throw new FormatException($"Unknown key: {token}");
    }
}
