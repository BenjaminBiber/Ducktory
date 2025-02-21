namespace Enten.Components.Classes;

public class Duckpart
{
    public List<string> Pictures { get; set; }
    public string Folder { get; set; }
    public string currentPicture { get; set; }
    public bool IsVisible { get; set; } = true;
    public int SortOrder { get; set; }
}