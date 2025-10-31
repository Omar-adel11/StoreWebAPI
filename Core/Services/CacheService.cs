using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using ServicesAbstractions;

namespace Services
{
    public class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public async Task<string?> GetAsync(string Key)
        {
            var value = await cacheRepository.GetAsync(Key); 
            return value == null ? null : value;
        }

        public async Task SetCacheValueAsync(string Key, object Value, TimeSpan? duration)
        {
            await cacheRepository.SetAsync(Key, Value, duration);
        }
    }
}
