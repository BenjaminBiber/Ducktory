using DuckLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

[ApiController]
[Route("api/[controller]")]
public class DuckController : ControllerBase
{
    private readonly IHttpClientFactory _http;
    private readonly DuckService _duckService;

    public DuckController(IHttpClientFactory http, DuckService duckService)
    {
        _http = http;
        _duckService = duckService;
    }

    // GET /api/duck/picture   (lädt URLs aus deinem Service)
    [HttpGet("picture")]
    public async Task<IActionResult> GetPicture(CancellationToken ct)
    {
        await _duckService.GetDuckParts();
        await _duckService.RandomizeDuck();
        var urls = _duckService.CanvasImagesAbsolute?.ToList() ?? [];
        if (urls.Count == 0) return BadRequest("Keine Bild-URLs vorhanden.");

        var bytes = await MergeImagesAsync(urls, ct);

        // CORS, falls du das Bild cross-origin im <img>/Canvas laden willst
        Response.Headers["Access-Control-Allow-Origin"] = "*";
        Response.Headers["Cross-Origin-Resource-Policy"] = "cross-origin";
        return File(bytes, "image/png");
    }

    private async Task<byte[]> MergeImagesAsync(List<string> urls, CancellationToken ct)
    {
        var client = _http.CreateClient("backente");
        var loaded = new List<Image<Rgba32>>();
        try
        {
            // Bilder laden
            foreach (var u in urls)
            {
                using var resp = await client.GetAsync(u, HttpCompletionOption.ResponseHeadersRead, ct);
                if (!resp.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Bild konnte nicht geladen werden: {u}");

                await using var s = await resp.Content.ReadAsStreamAsync(ct);
                loaded.Add(await Image.LoadAsync<Rgba32>(s, ct));
            }

            // Canvas an Größe des ersten Bilds
            int w = loaded[0].Width, h = loaded[0].Height;
            using var canvas = new Image<Rgba32>(w, h, Color.Transparent);

            // In Reihenfolge übereinander zeichnen
            foreach (var img in loaded)
            {
                using var layer = EnsureSize(img, w, h);     // ggf. skalieren
                canvas.Mutate(c => c.DrawImage(layer, new Point(0, 0), 1f));
            }

            using var ms = new MemoryStream();
            await canvas.SaveAsync(ms, new PngEncoder());    // PNG (mit Alpha)
            return ms.ToArray();
        }
        finally
        {
            foreach (var i in loaded) i.Dispose();
        }
    }

    private static Image<Rgba32> EnsureSize(Image<Rgba32> img, int w, int h)
    {
        if (img.Width == w && img.Height == h) return img.Clone();
        return img.Clone(c => c.Resize(new ResizeOptions {
            Size = new Size(w, h),
            Mode = ResizeMode.Stretch // alternativ Pad/Crop je nach Bedarf
        }));
    }
}
