namespace DuckLibrary.Models;

public class DuckPart
{
    public List<DuckPartImage> Images { get; set; }
    public string Folder { get; set; }
    public string currentPicture { get; set; }
    public bool IsVisible { get; set; } = true;
    public int SortOrder { get; set; }

    public DuckPart()
    {
        Images = new List<DuckPartImage>();
        Folder = string.Empty;
        currentPicture = string.Empty;
    }
}