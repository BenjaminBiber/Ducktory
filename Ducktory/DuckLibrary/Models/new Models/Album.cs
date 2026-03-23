using System.Text.Json.Serialization;

namespace DuckLibrary.Models;

public class Album
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("istSichtbar")]
    public bool IstSichtbar { get; set; } = false;
    [JsonPropertyName("sortierIndex")]
    public decimal? SortierIndex { get; set; }
    [JsonPropertyName("entenBilder")]
    public List<EntenBild> EntenBilder { get; set; } = new();
}
