using System.Collections.Concurrent;
using DuckLibrary.Models;

namespace DuckLibrary.Services;

public class CacheService
{
    private readonly int _cacheDurationInHours = (System.Diagnostics.Debugger.IsAttached ? 0 : 1);
    private readonly ConcurrentDictionary<string, CacheItem<object>> _cache = new();

    public async Task<T> GetOrAddAsync<T, TMapping>(string key, Func<Task<T>> dataFactory)
    {
        var now = DateTime.UtcNow;

        if (_cache.TryGetValue(key, out var existingItem))
        {
            if (existingItem.LastWrite.AddHours(_cacheDurationInHours) > now)
            {
                // Cache gültig
                return (T)existingItem.Data;
            }
        }

        var newData = await dataFactory();

        var newItem = new CacheItem<object>
        {
            Data = newData,
            LastWrite = now
        };

        if (newItem != null)
        {
            _cache[key] = newItem;
        }

        return newData;
    }
}
