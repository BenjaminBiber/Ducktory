namespace DuckLibrary.Models;

public class CacheItem<T>
{
    public T Data { get; set; }
    public DateTime LastWrite { get; set; }
}