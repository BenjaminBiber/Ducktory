namespace DuckLibrary.Models;

public class Image
{
    public string Small { get; set; }
    public string Large { get; set; }
    public string Original { get; set; }
    
    public Image()
    {
        Small = string.Empty;
        Large = string.Empty;
        Original = string.Empty;
    }
}