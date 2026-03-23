using BenjaminBiber.CMS.Client.Models;

namespace DuckLibrary.Models;

public class DuckPartImage
{
    public string Name { get; set; }
    public CmsImage Bild { get; set; }
    public bool IsDefault { get; set; }

    public DuckPartImage()
    {
        Bild = new CmsImage();
        Name = string.Empty;
    }
}