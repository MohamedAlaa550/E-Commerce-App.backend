using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<BasketDto?> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);
            var createdOrUpdatedBasket = await basketRepository.CreateOrUpdateBasketAsync(customerBasket);
            if (createdOrUpdatedBasket is null)
                throw new Exception("Cannot Create Or Update Basket");
            return mapper.Map<BasketDto>(createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string Id)
       => await basketRepository.DeleteBasketAsync(Id);

        public async Task<BasketDto?> GetBasketAsync(string Id)
        {
            var basket = await basketRepository.GetBasketAsync(Id);
            if (basket is null)
                throw new BasketNotFoundException(Id);
            return mapper.Map<BasketDto>(basket);
        }
    }
}
