using System.Text.Json.Serialization;

namespace DuckLibrary.MappingModels;

public class DuckPartDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Ist-Sichtbar")]
    public bool IstSichtbar { get; set; }

    [JsonPropertyName("Sortier-Index")]
    public int SortierIndex { get; set; }

    [JsonPropertyName("enten-Bilds")]
    public List<DuckPartImageDto> EntenBilds { get; set; }
}
