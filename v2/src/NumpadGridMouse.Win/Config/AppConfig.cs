using System.Text.Json.Serialization;

namespace NumpadGridMouse.Win.Config;

public sealed class AppConfig
{
    public GridConfig Grid { get; set; } = new();
    public HudConfig Hud { get; set; } = new();
    public SoundsConfig Sounds { get; set; } = new();
    public BehaviorConfig Behavior { get; set; } = new();
    public HotkeysConfig Hotkeys { get; set; } = new();
}

public sealed class GridConfig
{
    public int LineThickness { get; set; } = 2;
    public int FontSize { get; set; } = 24;
    public double Opacity { get; set; } = 0.85;
    public string BorderColor { get; set; } = "#3B82F6";
    public string TextColor { get; set; } = "#FFFFFF";
    public string BackgroundColor { get; set; } = "#1E1E1E";
    public bool HighContrast { get; set; }
}

public sealed class HudConfig
{
    public bool Enabled { get; set; } = true;
    public int FontSize { get; set; } = 14;
    public string Position { get; set; } = "top_left";
    public string TextColor { get; set; } = "#FFFFFF";
    public string BackgroundColor { get; set; } = "#00000080";
}

public sealed class SoundsConfig
{
    public bool Enabled { get; set; } = true;
    public bool ShowGrid { get; set; } = true;
    public bool RefineSelection { get; set; } = true;
    public bool Click { get; set; } = true;
    public bool Scroll { get; set; }
}

public sealed class BehaviorConfig
{
    public int MaxDepth { get; set; }
    public int ScrollAmount { get; set; } = 3;
    public int ScrollScale { get; set; } = 10;
    public double AnimationDuration { get; set; } = 0.1;
    public int ArrowStepPx { get; set; } = 8;
    public int DoubleEnterMs { get; set; } = 450;
    public bool ToggleDrillCenter { get; set; } = true;
}

public sealed class HotkeysConfig
{
    [JsonPropertyName("toggle_grid")]
    public string ToggleGrid { get; set; } = "Win+/";

    [JsonPropertyName("toggle_arrow_mode")]
    public string ToggleArrowMode { get; set; } = "Ctrl+Shift+A";

    [JsonPropertyName("toggle_help")]
    public string ToggleHelp { get; set; } = "Win+Shift+/";

    /// <summary>Legacy Python key; kept when merging existing configs.</summary>
    public string? Toggle { get; set; }

    public string Exit { get; set; } = "Esc";
}
