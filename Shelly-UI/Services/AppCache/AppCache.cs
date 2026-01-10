using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Shelly_UI.Services.AppCache;

public class AppCache : IAppCache
{
    private ConcurrentDictionary<string, object> _cache = new();
    
    public Task<bool> StoreAsync<T>(string key, T value)
    {
        _cache.TryGetValue(key, out var oldValue);
        return _cache.TryAdd(key, value!) ? Task.FromResult(true) : Task.FromResult(_cache.TryUpdate(key, value!, oldValue!));
    }

    public Task<T> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out var value);
        return Task.FromResult((T) value!);
    }
}
