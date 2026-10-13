using NumpadGridMouse.Win.Config;

namespace NumpadGridMouse.Win.Tests;

public class ConfigStoreTests
{
    [Fact]
    public void Load_CreatesDefaults_WhenMissing()
    {
        var dir = Path.Combine(Path.GetTempPath(), "ngm-test-" + Guid.NewGuid());
        var store = new ConfigStore(dir);
        var config = store.Load();

        Assert.Equal("Win+/", config.Hotkeys.ToggleGrid);
        Assert.Equal("Win+Shift+/", config.Hotkeys.ToggleHelp);
        Assert.True(File.Exists(store.ConfigFilePath));
    }

    [Fact]
    public void Load_MergesPythonToggle_ToToggleGrid()
    {
        var dir = Path.Combine(Path.GetTempPath(), "ngm-test-" + Guid.NewGuid());
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "config.json");
        File.WriteAllText(path, """
            {
              "hotkeys": { "toggle": "ctrl+shift+/" }
            }
            """);

        var store = new ConfigStore(dir);
        var config = store.Load();

        Assert.Equal("Win+/", config.Hotkeys.ToggleGrid);
        Assert.Equal("Win+Shift+/", config.Hotkeys.ToggleHelp);
    }

    [Fact]
    public void Load_PreservesUserGridOpacity()
    {
        var dir = Path.Combine(Path.GetTempPath(), "ngm-test-" + Guid.NewGuid());
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "config.json");
        File.WriteAllText(path, """{ "grid": { "opacity": 0.42 } }""");

        var store = new ConfigStore(dir);
        var config = store.Load();

        Assert.Equal(0.42, config.Grid.Opacity);
        Assert.Equal(24, config.Grid.FontSize);
    }
}
