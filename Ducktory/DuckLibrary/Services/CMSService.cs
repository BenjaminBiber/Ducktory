using System.Text.Json;
using AutoMapper;
using DuckLibrary.MappingModels;
using DuckLibrary.Models;

namespace DuckLibrary.Services;

public class CmsService
{
    private readonly CacheService _cacheService;
    private readonly HttpClient _httpClient;
    private readonly string _cmsUrl = (Environment.GetEnvironmentVariable("CMSUrl") ?? "https://backente.benjaminbiber.de") + "/api";
    private readonly IMapper _mapper;

    public CmsService(CacheService cacheService, HttpClient httpClient, IMapper mapper)
    {
        _cacheService = cacheService;
        _httpClient = httpClient;
        _mapper = mapper;
    }
    
    public async Task<T> GetItems<T, TMapping>(string key, string url)
    {
        return await _cacheService.GetOrAddAsync<T, TMapping>(key, async () =>
        {
            var result = await GetData<T, TMapping>(_cmsUrl + url);
            if (result.Success)
            {
                return result.Data;
            }
            else
            {
                return default;
            }
        });
    }
   
    private async Task<ApiResult<T>> GetData<T, TMapping>(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return new ApiResult<T>
            {
                Success = false,
                Data = default,
                Error = "URL is null or empty"
            };
        }

        try
        {
            var response = await _httpClient.GetAsync(new Uri(new Uri(_cmsUrl), url));
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<ResponseItem<TMapping>>(json, options);

            return new ApiResult<T>
            {
                Success = true,
                Data = _mapper.Map<T>(data.Data)
            };
        }
        catch (Exception ex)
        {
            return new ApiResult<T>
            {
                Success = false,
                Data = default,
                Error = ex.Message
            };
        }
    }
}