using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IPaymentService
    {
        public Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);
        public Task UpdateOrderPaymentStatusAsync(string json, string header);
    }
}
