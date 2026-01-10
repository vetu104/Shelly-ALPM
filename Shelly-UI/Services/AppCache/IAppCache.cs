using System.Threading.Tasks;

namespace Shelly_UI.Services.AppCache;

public interface IAppCache
{
    Task<bool> StoreAsync<T>(string key, T value);
    Task<T> GetAsync<T>(string key);
}
