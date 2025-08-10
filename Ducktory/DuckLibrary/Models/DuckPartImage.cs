namespace DuckLibrary.Models;

public class DuckPartImage
{
    public string Name { get; set; }
    public Image Bild { get; set; }
    public bool IsDefault { get; set; }

    public DuckPartImage()
    {
        Bild = new Image();
        Name = string.Empty;
    }
}