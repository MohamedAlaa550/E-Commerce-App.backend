using Shared.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IOrderService
    {
        public Task<OrderDto> GetOrderByIdAsync(Guid id);

        public Task<IEnumerable<OrderDto>> GetOrdersByIdAsync(string email);

        public Task<OrderDto> CreateOrderAsync(OrderRequest request, string userEmail);

        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();
    }
}
