using System.Text.Json.Serialization;

namespace DuckLibrary.MappingModels;

public class ImageDto
{
    [JsonPropertyName("small")]
    public string Small { get; set; } = string.Empty;

    [JsonPropertyName("large")]
    public string Large { get; set; } = string.Empty;

    [JsonPropertyName("original")]
    public string Original { get; set; } = string.Empty;
}