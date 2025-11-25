using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class OrderService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IBasketRepository basketRepository) : IOrderService
    {
        public async Task<OrderDto> CreateOrderAsync(OrderRequest request, string userEmail)
        {
           var address = mapper.Map<ShippingAddress>(request.ShippingAddress);
            var basket = await basketRepository.GetBasketAsync(request.BasketId) ?? throw new
                BasketNotFoundException(request.BasketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product,int>().GetAsync
                    (item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(item, product));
            }
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod,
                int>().GetAsync(request.DeliveryMethodId) ?? throw new
                DeliveryMethodException(request.DeliveryMethodId);
            var existingOrder = await unitOfWork.GetRepository<Order,Guid>()
                .GetAsync(new OrderWithPaymentSpecifications(basket.PaymentIntentId!));
            if (existingOrder is not null)
                 unitOfWork.GetRepository<Order, Guid>().Delete(existingOrder);
            var subtotal = orderItems.Sum(i=> i.Price * i.Quantity);
            var order = new Order(userEmail, address, orderItems
                , deliveryMethod,subtotal,basket.PaymentIntentId);
            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var methods =await unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResult>>(methods);
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var order =  await unitOfWork.GetRepository<Order,Guid>()
                .GetAsync(new OrderWithIncludesSpecifications(id)) ??
                throw new OrderNotFoundException(id);
            return mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByEmailAsync(string email)
        {
           var orders =  await unitOfWork.GetRepository<Order,Guid>()
                .GetAllAsync(new OrderWithIncludesSpecifications(email));
            return mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        #region Helpper Methods

        private OrderItem CreateOrderItem(BasketItem item, Product product)
        => new OrderItem(new ProductInOrderItem(product.Id, product.Name,
            product.PictureUrl), product.Price, item.Quantity);

        #endregion
    }
}
