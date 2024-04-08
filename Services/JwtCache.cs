using MedbaseLibrary.Services;
using Microsoft.Extensions.Caching.Memory;

namespace MedbaseBlazor.Services;

public class JwtCache : IAuthMemory
{ 
    private readonly IMemoryCache _memoryCache;

    public JwtCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void StoreToken(string userId, string jwt)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Set an appropriate expiration time
        };

        _memoryCache.Set(userId, jwt, cacheEntryOptions);
    }

    public string GetToken(string userId)
    {
        if (_memoryCache.TryGetValue(userId, out string jwt))
        {
            return jwt;
        }

        // Token not found in cache
        return null;
    }

    public void RemoveToken(string userId)
    {
        _memoryCache.Remove(userId);
    }
}
