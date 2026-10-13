using System.Diagnostics;
using System.Runtime.InteropServices;
using NumpadGridMouse.Win.Config;
using NumpadGridMouse.Win.Native;

namespace NumpadGridMouse.Win.Hotkeys;

internal sealed class HotkeyService : IDisposable
{
    private readonly HotkeySinkWindow _sink = new();
    private readonly List<int> _registered = new();

    public event Action? ToggleGrid;
    public event Action? ToggleArrowMode;
    public event Action? ToggleHelp;

    public void RegisterFromConfig(AppConfig config)
    {
        _sink.HotkeyPressed += OnHotkeyPressed;
        _sink.Show();

        RegisterOne(HotkeyIds.ToggleGrid, config.Hotkeys.ToggleGrid);
        RegisterOne(HotkeyIds.ToggleHelp, config.Hotkeys.ToggleHelp);
    }

    private void RegisterOne(int id, string chord)
    {
        var parsed = HotkeyParser.Parse(chord);
        if (!NativeMethods.RegisterHotKey(_sink.Handle, id, parsed.Modifiers, parsed.VirtualKey))
        {
            var err = Marshal.GetLastWin32Error();
            Trace.TraceWarning("RegisterHotKey failed id={0} chord={1} win32={2}", id, chord, err);
            return;
        }

        _registered.Add(id);
        Trace.TraceInformation("Registered hotkey id={0} chord={1}", id, chord);
    }

    private void OnHotkeyPressed(int id)
    {
        switch (id)
        {
            case HotkeyIds.ToggleGrid:
                Trace.TraceInformation("Hotkey: ToggleGrid");
                ToggleGrid?.Invoke();
                break;
            case HotkeyIds.ToggleArrowMode:
                Trace.TraceInformation("Hotkey: ToggleArrowMode");
                ToggleArrowMode?.Invoke();
                break;
            case HotkeyIds.ToggleHelp:
                Trace.TraceInformation("Hotkey: ToggleHelp");
                ToggleHelp?.Invoke();
                break;
        }
    }

    public void Dispose()
    {
        _sink.HotkeyPressed -= OnHotkeyPressed;
        foreach (var id in _registered)
            NativeMethods.UnregisterHotKey(_sink.Handle, id);
        _registered.Clear();
        _sink.Close();
    }
}
