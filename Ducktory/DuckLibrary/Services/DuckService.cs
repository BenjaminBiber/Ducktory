using BenjaminBiber.CMS.Client.Client;
using BenjaminBiber.CMS.Client.Models;
using BenjaminBiber.CMS.Client.Query;
using DuckLibrary.Models;

namespace DuckLibrary.Services;

public class DuckService
{
    private const string BaseUrl = "https://backente.benjaminbiber.de";

    private readonly ICmsClient _cmsClient;

    public DuckService(ICmsClient cmsClient)
    {
        _cmsClient = cmsClient;
    }

    public List<DuckPart> DuckParts { get; set; } = [];
    public List<string> CanvasImages => DuckParts
        .Where(x => x.IsVisible && !string.IsNullOrEmpty(x.CurrentPicture.Bild.Original))
        .OrderBy(x => x.SortOrder)
        .Select(x => ProxyUrl(x.CurrentPicture.Bild.AbsoluteUrl(BaseUrl)))
        .ToList();

    private static string ProxyUrl(string url) =>
        "/api/imageproxy?url=" + Uri.EscapeDataString(url);

    public async Task RandomizeDuck()
    {
        var rnd = new Random();
        foreach (var item in DuckParts)
        {
            item.CurrentPicture = item.Images[rnd.Next(item.Images.Count)];
        }
    }

    public async Task GetDuckParts()
    {
        var albumsTask = _cmsClient.GetItemsAsync<Album>("alben", new CmsQueryOptions { PageSize = 100 });
        var bilderTask = _cmsClient.GetItemsAsync<EntenBild>("enten-bilder", new CmsQueryOptions { PageSize = 1000 });

        await Task.WhenAll(albumsTask, bilderTask);

        var bilder = bilderTask.Result.Items
            .GroupBy(b => b.Data.Album)
            .ToDictionary(g => g.Key, g => g.ToList());

        DuckParts = albumsTask.Result.Items.Select(item =>
        {
            var images = bilder.TryGetValue(item.Id, out var list)
                ? list.Select(b => new DuckPartImage
                {
                    Name = b.Data.Name ?? string.Empty,
                    Bild = b.Data.Bild is { } img ? new CmsImage { Original = img.AbsoluteUrl(BaseUrl), Thumbnails = img.Thumbnails } : new CmsImage(),
                    IsDefault = b.Data.IstDefault
                }).ToList()
                : [];

            return new DuckPart
            {
                Name = item.Data.Name ?? string.Empty,
                IsVisible = item.Data.IstSichtbar,
                SortOrder = (int)(item.Data.SortierIndex ?? 0),
                Images = images,
                CurrentPicture = images.FirstOrDefault(x => x.IsDefault) ?? images.FirstOrDefault() ?? new DuckPartImage()
            };
        }).ToList();
    }
}
