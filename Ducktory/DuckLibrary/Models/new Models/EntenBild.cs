using System.Text.Json.Serialization;
using BenjaminBiber.CMS.Client.Models;

namespace DuckLibrary.Models;

public class EntenBild
{
    [JsonPropertyName("bild")]
    public CmsImage? Bild { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("istDefault")]
    public bool IstDefault { get; set; } = false;
    [JsonPropertyName("album")]
    public Guid? Album { get; set; }
}
