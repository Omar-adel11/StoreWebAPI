using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using StackExchange.Redis;

namespace Persistance.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsync(string Key)
        {
            var value = await _database.StringGetAsync(Key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task SetAsync(string Key, object Value, TimeSpan? duration)
        {
            var RedisValue = JsonSerializer.Serialize(Value);
            await _database.StringSetAsync(Key, RedisValue, duration);
        }
    }
}
