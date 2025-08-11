namespace DuckLibrary.Models;

public class DuckPart
{
    public List<DuckPartImage> Images { get; set; }
    public string Folder { get; set; }

    private DuckPartImage _currentPicture;

    public DuckPartImage CurrentPicture
    {
        get
        {
            if (_currentPicture.Bild != null && String.IsNullOrEmpty(_currentPicture.Bild.Original) && (Images != null || Images.Any()))
            {
                _currentPicture.Bild = (Images.FirstOrDefault(x => x.IsDefault) ?? new DuckPartImage()).Bild;
                return _currentPicture;
            }
            else
            {
                return _currentPicture;
            }
        }
        set
        {
            _currentPicture = value;
        }
    }

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