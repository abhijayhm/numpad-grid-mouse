using System.Diagnostics;
using System.Windows;
using NumpadGridMouse.Win.Config;
using NumpadGridMouse.Win.Grid;
using NumpadGridMouse.Win.Input;
using NumpadGridMouse.Win.Monitors;
using NumpadGridMouse.Win.Pointer;
using NumpadGridMouse.Win.Shell;

namespace NumpadGridMouse.Win;

internal sealed class ApplicationCoordinator : IDisposable
{
    private readonly ConfigStore _configStore;
    private readonly MonitorService _monitors = new();
    private readonly PointerService _pointer = new();
    private readonly LowLevelKeyboardHook _keyboardHook = new();
    private readonly LogonRegistryService _logon = new();

    private AppConfig _config = new();
    private GridModel? _gridModel;
    private ScreenRegion? _monitorRegion;
    private GridOverlayWindow? _overlay;

    private bool _gridActive;
    private bool _helpOnly;
    private bool _helpPanel;
    private bool _leftDrag;
    private bool _rightDragPath;
    private DateTime _lastEnterUtc;
    private int _lastNumpadKey;

    public ApplicationCoordinator(ConfigStore configStore)
    {
        _configStore = configStore;
        _keyboardHook.KeyDown += OnKeyDown;
    }

    public AppConfig Config => _config;

    public void Initialize() => _config = _configStore.Load();

    public bool IsRunAtLogonEnabled() => _logon.IsEnabled();

    public void SetRunAtLogon(bool enabled)
    {
        var exe = Environment.ProcessPath ?? Process.GetCurrentProcess().MainModule?.FileName ?? "";
        if (!string.IsNullOrEmpty(exe))
            _logon.SetEnabled(enabled, exe);
    }

    public void ToggleGrid()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            if (_gridActive)
                CloseAll();
            else
                OpenGrid();
        });
    }

    public void ToggleHelp()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            if (_gridActive)
            {
                _helpPanel = !_helpPanel;
                RefreshOverlay();
                return;
            }

            if (_helpOnly)
                CloseAll();
            else
                OpenHelpOnly();
        });
    }

    private void OpenGrid()
    {
        CloseAll();
        _monitorRegion = _monitors.GetMonitorUnderCursor();
        _gridModel = new GridModel(_monitorRegion, _config.Behavior.MaxDepth);
        _overlay = new GridOverlayWindow();
        _gridActive = true;
        _helpPanel = false;
        _lastEnterUtc = DateTime.MinValue;
        RefreshOverlay();
        _overlay.Show();
        EnsureHook();
        MovePointerToCurrentCenter();
    }

    private void OpenHelpOnly()
    {
        CloseAll();
        _monitorRegion = _monitors.GetMonitorUnderCursor();
        _overlay = new GridOverlayWindow();
        _overlay.ShowHelp(_monitorRegion, _config);
        _overlay.Show();
        _helpOnly = true;
        EnsureHook();
    }

    private void CloseAll()
    {
        EndDrags();
        _overlay?.Close();
        _overlay = null;
        _gridModel = null;
        _gridActive = false;
        _helpOnly = false;
        _helpPanel = false;
        ReleaseHookIfIdle();
    }

    private void EndDrags()
    {
        if (_leftDrag)
        {
            _pointer.LeftButtonUp();
            _leftDrag = false;
        }

        if (_rightDragPath)
        {
            MovePointerToCurrentCenter();
            _pointer.RightClick();
            _rightDragPath = false;
        }
    }

    private void EnsureHook() => _keyboardHook.Install();

    private void ReleaseHookIfIdle()
    {
        if (!_gridActive && !_helpOnly)
            _keyboardHook.Uninstall();
    }

    private void RefreshOverlay()
    {
        if (_overlay is null || _gridModel is null || _monitorRegion is null)
            return;

        if (_helpPanel)
        {
            _overlay.ShowHelp(_gridModel.CurrentRegion, _config);
            return;
        }

        var hud = _config.Hud.Enabled
            ? $"Depth {_gridModel.Depth} · 1–9 aim · Win+Enter click · Win+Alt right · Win+Shift+? help"
            : "";
        _overlay.ShowGrid(_gridModel.CurrentRegion, _gridModel, _config, hud);
    }

    private void MovePointerToCurrentCenter()
    {
        if (_gridModel is null)
            return;
        MovePointerTo(_gridModel.CurrentRegion);
    }

    private void MovePointerTo(ScreenRegion region)
    {
        var bounds = _gridModel?.Root ?? _monitorRegion;
        var (x, y) = region.Center();
        if (bounds is not null)
        {
            x = Math.Clamp(x, bounds.X, bounds.X + bounds.Width - 1);
            y = Math.Clamp(y, bounds.Y, bounds.Y + bounds.Height - 1);
        }

        _pointer.MoveTo(x, y);
    }

    private bool OnKeyDown(int vk, bool _)
    {
        if (_helpOnly)
        {
            if (vk == 0x1B)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(CloseAll);
                return true;
            }

            return false;
        }

        if (!_gridActive || _gridModel is null)
            return false;

        if (_helpPanel)
        {
            if (vk == 0x1B)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    _helpPanel = false;
                    RefreshOverlay();
                });
                return true;
            }

            return false;
        }

        return HandleGridKey(vk);
    }

    private bool HandleGridKey(int vk)
    {
        if (vk == 0x1B)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(CloseAll);
            return true;
        }

        if (WinChord.IsWinShift())
        {
            switch (vk)
            {
                case 0x26:
                    MovePointerToCurrentCenter();
                    _pointer.Scroll(0, _config.Behavior.ScrollAmount * _config.Behavior.ScrollScale);
                    return true;

                case 0x28:
                    MovePointerToCurrentCenter();
                    _pointer.Scroll(0, -_config.Behavior.ScrollAmount * _config.Behavior.ScrollScale);
                    return true;

                case WinChord.VkOemSemicolon:
                    ToggleLeftDrag();
                    return true;

                case WinChord.VkOemQuote:
                    ToggleRightDragPath();
                    return true;
            }

            return false;
        }

        if (WinChord.IsWinOnly())
        {
            if (vk == WinChord.VkReturn)
            {
                CommitPointerClick(left: true);
                return true;
            }

            if (WinChord.IsAltKey(vk))
            {
                CommitPointerClick(left: false);
                return true;
            }

            return false;
        }

        if (WinChord.IsWinDown())
            return false;

        if (TryMapNumpad(vk, out var key))
        {
            var region = _gridModel!.SelectRegion(key);
            if (region is not null)
            {
                _lastNumpadKey = key;
                MovePointerTo(region);
                RefreshOverlay();
            }

            return true;
        }

        if (vk == 0x08)
        {
            var region = _gridModel!.GoUp();
            if (region is not null)
            {
                MovePointerTo(region);
                RefreshOverlay();
            }

            return true;
        }

        return false;
    }

    private void CommitPointerClick(bool left)
    {
        MovePointerToCurrentCenter();
        if (left)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastEnterUtc).TotalMilliseconds <= _config.Behavior.DoubleEnterMs)
                _pointer.DoubleClick();
            else
                _pointer.LeftClick();
            _lastEnterUtc = now;
        }
        else
        {
            _pointer.RightClick();
        }

        System.Windows.Application.Current.Dispatcher.Invoke(CloseAll);
    }

    private void ToggleLeftDrag()
    {
        if (!_leftDrag)
        {
            MovePointerToCurrentCenter();
            _pointer.LeftButtonDown();
            _leftDrag = true;
        }
        else
        {
            _pointer.LeftButtonUp();
            _leftDrag = false;
        }
    }

    private void ToggleRightDragPath()
    {
        if (!_rightDragPath)
        {
            _rightDragPath = true;
            MovePointerToCurrentCenter();
        }
        else
        {
            MovePointerToCurrentCenter();
            _pointer.RightClick();
            _rightDragPath = false;
        }
    }

    private static bool TryMapNumpad(int vk, out int key)
    {
        if (vk >= 0x31 && vk <= 0x39)
        {
            key = vk - 0x30;
            return true;
        }

        if (vk >= 0x61 && vk <= 0x69)
        {
            key = vk - 0x60;
            return true;
        }

        key = 0;
        return false;
    }

    public void Dispose()
    {
        _keyboardHook.Dispose();
        CloseAll();
    }
}
