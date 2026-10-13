using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NumpadGridMouse.Win.Config;

namespace NumpadGridMouse.Win.Tray;

internal sealed class TrayApplicationContext : IDisposable
{
    private readonly NotifyIcon _notifyIcon;

    public event Action? ExitRequested;
    public event Action? ShowGridRequested;
    public event Action? ToggleHelpRequested;
    public event Action<bool>? RunAtLogonChanged;

    public TrayApplicationContext(AppConfig config, bool runAtLogonEnabled)
    {
        _ = config;
        _notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Visible = true,
            Text = "Numpad Grid Mouse — Win+/ grid · Win+Shift+? help",
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add("Grid (Win+/)", null, (_, _) => ShowGridRequested?.Invoke());
        menu.Items.Add("Help (Win+Shift+?)", null, (_, _) => ToggleHelpRequested?.Invoke());
        menu.Items.Add(new ToolStripSeparator());
        var logon = new ToolStripMenuItem("Run at Windows logon") { CheckOnClick = true, Checked = runAtLogonEnabled };
        logon.CheckedChanged += (_, _) => RunAtLogonChanged?.Invoke(logon.Checked);
        menu.Items.Add(logon);
        menu.Items.Add("Open config folder", null, (_, _) => OpenConfigFolder());
        menu.Items.Add("Exit", null, (_, _) => ExitRequested?.Invoke());

        _notifyIcon.ContextMenuStrip = menu;
        _notifyIcon.DoubleClick += (_, _) => ShowGridRequested?.Invoke();
    }

    private static void OpenConfigFolder()
    {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NumpadGridMouse");
        Directory.CreateDirectory(dir);
        Process.Start(new ProcessStartInfo { FileName = dir, UseShellExecute = true });
    }

    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
    }
}
