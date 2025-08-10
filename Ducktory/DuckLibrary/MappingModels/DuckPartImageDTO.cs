using System.Text.Json.Serialization;
namespace DuckLibrary.MappingModels;

public class DuckPartImageDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("Bild")]
    public ImageDto Bild { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Ist Default?")]
    public bool IstDefault { get; set; }
}