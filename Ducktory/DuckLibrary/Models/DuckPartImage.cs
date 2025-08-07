namespace DuckLibrary.Models;

public class DuckPartImage
{
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsDefault { get; set; }

    public DuckPartImage()
    {
        Name = string.Empty;
        Url = string.Empty;
    }
}