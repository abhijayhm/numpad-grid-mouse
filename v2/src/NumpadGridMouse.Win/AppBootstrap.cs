using System.Diagnostics;
using NumpadGridMouse.Win.Config;
using NumpadGridMouse.Win.Hotkeys;
using NumpadGridMouse.Win.Tray;

namespace NumpadGridMouse.Win;

internal sealed class AppBootstrap : IDisposable
{
    private static Mutex? _singleInstanceMutex;

    private readonly ConfigStore _configStore = new();
    private ApplicationCoordinator? _coordinator;
    private HotkeyService? _hotkeyService;
    private TrayApplicationContext? _tray;

    public bool TryAcquireSingleInstance()
    {
        const string mutexName = "NumpadGridMouse.Win.SingleInstance";
        try
        {
            _singleInstanceMutex = new Mutex(true, mutexName, out var created);
            return created;
        }
        catch
        {
            return true;
        }
    }

    public void Start()
    {
        Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        Trace.AutoFlush = true;

        _coordinator = new ApplicationCoordinator(_configStore);
        _coordinator.Initialize();

        _hotkeyService = new HotkeyService();
        _hotkeyService.ToggleGrid += () => _coordinator.ToggleGrid();
        _hotkeyService.ToggleHelp += () => _coordinator.ToggleHelp();
        _hotkeyService.RegisterFromConfig(_coordinator.Config);

        _tray = new TrayApplicationContext(_coordinator.Config, _coordinator.IsRunAtLogonEnabled());
        _tray.ShowGridRequested += () => _coordinator.ToggleGrid();
        _tray.ToggleHelpRequested += () => _coordinator.ToggleHelp();
        _tray.RunAtLogonChanged += enabled => _coordinator.SetRunAtLogon(enabled);
        _tray.ExitRequested += () => System.Windows.Application.Current.Shutdown();
    }

    public void Dispose()
    {
        _hotkeyService?.Dispose();
        _tray?.Dispose();
        _coordinator?.Dispose();
        _singleInstanceMutex?.ReleaseMutex();
        _singleInstanceMutex?.Dispose();
    }
}
