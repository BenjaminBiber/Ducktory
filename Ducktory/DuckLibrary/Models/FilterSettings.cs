namespace DuckLibrary.Models;

public class FilterSettings
{
    public double BlurPx { get; set; } = 0;           // px
    public int BrightnessPct { get; set; } = 100;     // %
    public int ContrastPct { get; set; } = 100;       // %
    public int GrayscalePct { get; set; } = 0;        // %
    public int HueDeg { get; set; } = 0;              // -180..180
    public int InvertPct { get; set; } = 0;           // %
    public int OpacityPct { get; set; } = 100;        // %
    public int SaturatePct { get; set; } = 100;       // %
    public int SepiaPct { get; set; } = 0;            // %
    // drop-shadow
    public int ShadowX { get; set; } = 0;             // px
    public int ShadowY { get; set; } = 0;             // px
    public int ShadowBlur { get; set; } = 0;          // px
    public string ShadowColor { get; set; } = "rgba(0,0,0,0.35)";

    public string ToCanvasFilterString()
    {
        var f = new List<string>();
        if (BlurPx > 0) f.Add($"blur({BlurPx:0.##}px)");
        if (ShadowBlur > 0 || ShadowX != 0 || ShadowY != 0)
            f.Add($"drop-shadow({ShadowX}px {ShadowY}px {ShadowBlur}px {ShadowColor})");
        f.Add($"brightness({BrightnessPct}%)");
        f.Add($"contrast({ContrastPct}%)");
        if (GrayscalePct > 0) f.Add($"grayscale({GrayscalePct}%)");
        if (HueDeg != 0) f.Add($"hue-rotate({HueDeg}deg)");
        if (InvertPct > 0) f.Add($"invert({InvertPct}%)");
        if (OpacityPct != 100) f.Add($"opacity({OpacityPct}%)");
        f.Add($"saturate({SaturatePct}%)");
        if (SepiaPct > 0) f.Add($"sepia({SepiaPct}%)");
        return string.Join(' ', f);
    }
}
