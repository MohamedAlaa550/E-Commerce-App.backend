using Domain.Contracts;
using Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connectionMultiplexer) 
        : IBasketRepository

    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var isCreatedOrUpdated = await _database.StringSetAsync
                (basket.Id, jsonBasket, timeToLive ?? TimeSpan.FromDays(30));
            return  isCreatedOrUpdated ? await GetBasketAsync(basket.Id) : null;
        }

        public async Task<bool> DeleteBasketAsync(string Id)
        => await _database.KeyDeleteAsync(Id);

        public async Task<CustomerBasket?> GetBasketAsync(string Id)
        {
            var value = await _database.StringGetAsync(Id);
            if (value.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(value!);
        }
    }
}
