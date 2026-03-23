using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ImageProxyController : ControllerBase
{
    private readonly IHttpClientFactory _http;

    public ImageProxyController(IHttpClientFactory http)
    {
        _http = http;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string url, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return BadRequest();

        var client = _http.CreateClient("proxy");
        using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, ct);
        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode);

        var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/png";
        var bytes = await response.Content.ReadAsByteArrayAsync(ct);
        return File(bytes, contentType);
    }
}
