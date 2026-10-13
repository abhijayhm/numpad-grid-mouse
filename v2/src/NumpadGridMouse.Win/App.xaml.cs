namespace NumpadGridMouse.Win;

public partial class App : System.Windows.Application
{
    private AppBootstrap? _bootstrap;

    protected override void OnStartup(System.Windows.StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;

        _bootstrap = new AppBootstrap();
        if (!_bootstrap.TryAcquireSingleInstance())
        {
            System.Windows.MessageBox.Show(
                "Numpad Grid Mouse is already running.",
                "Numpad Grid Mouse",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
            Shutdown();
            return;
        }

        _bootstrap.Start();
    }

    protected override void OnExit(System.Windows.ExitEventArgs e)
    {
        _bootstrap?.Dispose();
        base.OnExit(e);
    }
}
