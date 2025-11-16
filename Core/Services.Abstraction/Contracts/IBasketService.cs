using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IBasketService
    {
        public  Task<BasketDto?> GetBasketAsync(string Id);
        public Task<BasketDto?> CreateOrUpdateBasketAsync(BasketDto basket);
        public Task<bool> DeleteBasketAsync(string Id);
    }
}
