using DuckLibrary.MappingModels;
using DuckLibrary.Models;

namespace DuckLibrary.Services;

public class DuckService
{
    public List<DuckPart> DuckParts { get; set; } = [];
    public List<string> CanvasImages => DuckParts.Where(x => x.IsVisible && !String.IsNullOrEmpty(x.CurrentPicture.Bild.Original)).OrderBy(x => x.SortOrder).Select(x => x.CurrentPicture.Bild.Original).ToList();
    
    public async Task RandomizeDuck()
    {
        var rnd = new Random();
        foreach (var item in DuckParts)
        {
            item.CurrentPicture = item.Images[rnd.Next(item.Images.Count)];
        }
    }

    public async Task GetDuckParts(CmsService cms)
    {
        DuckParts = await cms.GetItems<List<DuckPart>, DuckPartDto>("DuckParts", "/collections/albums");
    }
}