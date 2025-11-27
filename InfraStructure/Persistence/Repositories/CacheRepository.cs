using Microsoft.EntityFrameworkCore.Storage;
using Services.Abstraction.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connectionMultiplexer) : ICacheRepository
    {
        private readonly StackExchange.Redis.IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.IsNullOrEmpty ? default : value;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            var serializedOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var serializedObject = JsonSerializer.Serialize(value,serializedOptions);
            await _database.StringSetAsync(key, serializedObject, duration);
        }
    }
}
