using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NumpadGridMouse.Win.Config;

public sealed class ConfigStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    private readonly string _configDirectory;
    private readonly string _configFilePath;

    public ConfigStore(string? configDirectoryOverride = null)
    {
        _configDirectory = configDirectoryOverride
            ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NumpadGridMouse");
        _configFilePath = Path.Combine(_configDirectory, "config.json");
    }

    public string ConfigFilePath => _configFilePath;

    public AppConfig Load()
    {
        Directory.CreateDirectory(_configDirectory);

        if (!File.Exists(_configFilePath))
        {
            var defaults = CreateDefaults();
            Save(defaults);
            return defaults;
        }

        try
        {
            var json = File.ReadAllText(_configFilePath);
            var fileNode = JsonNode.Parse(json) as JsonObject ?? new JsonObject();
            ApplyPythonHotkeyMigration(fileNode);
            var defaultNode = JsonSerializer.SerializeToNode(CreateDefaults(), JsonOptions) as JsonObject
                ?? new JsonObject();
            var merged = DeepMerge(defaultNode, fileNode);
            var config = merged.Deserialize<AppConfig>(JsonOptions) ?? CreateDefaults();
            EnsureHotkeyDefaults(config);
            Save(config);
            return config;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceWarning("Config load failed: {0}", ex.Message);
            var defaults = CreateDefaults();
            Save(defaults);
            return defaults;
        }
    }

    public void Save(AppConfig config)
    {
        Directory.CreateDirectory(_configDirectory);
        var json = JsonSerializer.Serialize(config, JsonOptions);
        File.WriteAllText(_configFilePath, json);
    }

    private static AppConfig CreateDefaults() => new();

    private static void EnsureHotkeyDefaults(AppConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.Hotkeys.ToggleGrid))
            config.Hotkeys.ToggleGrid = "Win+/";
        if (string.IsNullOrWhiteSpace(config.Hotkeys.ToggleArrowMode))
            config.Hotkeys.ToggleArrowMode = "Ctrl+Shift+A";
        if (string.IsNullOrWhiteSpace(config.Hotkeys.ToggleHelp))
            config.Hotkeys.ToggleHelp = "Win+Shift+/";
    }

    /// <summary>
    /// If only Python <c>hotkeys.toggle</c> exists, copy to <c>toggle_grid</c> when missing.
    /// </summary>
    private static void ApplyPythonHotkeyMigration(JsonObject root)
    {
        if (root["hotkeys"] is not JsonObject hotkeys)
            return;

        var hasGrid = hotkeys["toggle_grid"] != null;
        var legacy = hotkeys["toggle"]?.GetValue<string>();
        if (!hasGrid && !string.IsNullOrWhiteSpace(legacy))
        {
            // Map python chord to Win-style; prefer new default over ctrl+shift+/
            if (legacy.Contains('/'))
                hotkeys["toggle_grid"] = "Ctrl+Shift+G";
            else
                hotkeys["toggle_grid"] = legacy;
        }

        if (hotkeys["toggle_arrow_mode"] == null)
            hotkeys["toggle_arrow_mode"] = "Ctrl+Shift+A";

        var grid = hotkeys["toggle_grid"]?.GetValue<string>() ?? "";
        if (grid.Contains("Ctrl", StringComparison.OrdinalIgnoreCase) || grid.Contains("G") && !grid.Contains("Win", StringComparison.OrdinalIgnoreCase))
            hotkeys["toggle_grid"] = "Win+/";

        var help = hotkeys["toggle_help"]?.GetValue<string>() ?? "";
        if (help.Contains("Ctrl", StringComparison.OrdinalIgnoreCase) || help == "Ctrl+Shift+?")
            hotkeys["toggle_help"] = "Win+Shift+/";
    }

    private static JsonObject DeepMerge(JsonObject baseNode, JsonObject overrideNode)
    {
        var result = baseNode.DeepClone() as JsonObject ?? new JsonObject();
        foreach (var prop in overrideNode)
        {
            if (prop.Value is JsonObject overrideChild && result[prop.Key] is JsonObject baseChild)
                result[prop.Key] = DeepMerge(baseChild, overrideChild);
            else
                result[prop.Key] = prop.Value?.DeepClone();
        }

        return result;
    }
}
