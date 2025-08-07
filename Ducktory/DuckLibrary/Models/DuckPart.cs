namespace DuckLibrary.Models;

public class DuckPart
{
    public List<DuckPartImage> Images { get; set; }
    public string Folder { get; set; }
    public DuckPartImage CurrentPicture { get; set; }
    public string Name { get; set; }
    public bool IsVisible { get; set; } = true;
    public int SortOrder { get; set; }

    public DuckPart()
    {
        Images = new List<DuckPartImage>();
        Folder = string.Empty;
        CurrentPicture = new DuckPartImage();
        Name = string.Empty;
    }
}