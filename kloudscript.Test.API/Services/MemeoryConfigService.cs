using kloudscript.Test.API.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace kloudscript.Test.API.Services
{
    public interface IMemeoryConfigService
    {
        Task<bool> SetObjectInMemroy(string cacheKey, object dataObject, int slidingExpiry, int absExpiry);
        Task<object?> GetObjectFromMemory(string cacheKey);
        //bool RemoveObjectFromMemroy(string cacheKey);
    }
    public class MemeoryConfigService : IMemeoryConfigService
    {
       private readonly IMemoryCache imemoryCache; 
        public MemeoryConfigService(IMemoryCache _imemoryCache)
        {
            imemoryCache = _imemoryCache; 
        }
        public async Task<object?> GetObjectFromMemory(string cacheKey)
        {
            object? data = null;
            await Task.Run(() =>
            {
                data = imemoryCache.Get(cacheKey);
            });
            return data;
        }
         
        public async Task<bool> SetObjectInMemroy(string cacheKey, object dataObject, int slidingExpiry, int absExpiry)
        {
            bool isSuccess = true;
            object? data = null;
            await Task.Run(() =>
            {
                if (imemoryCache.TryGetValue(cacheKey, out data) == false)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpiry))
                                                                          .SetAbsoluteExpiration(TimeSpan.FromSeconds(absExpiry))
                                                                          .SetPriority(CacheItemPriority.Normal);

                    imemoryCache.Set(cacheKey, dataObject, cacheEntryOptions);
                }
            });
            return isSuccess;
        }
    }
}
