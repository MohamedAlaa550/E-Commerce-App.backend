using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Product = Domain.Entities.ProductModule.Product;

namespace Services.Implementations
{
    public class PaymentService(IConfiguration configuration,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork ,IMapper mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = configuration.GetSection("StripeSettings")
                ["SecretKey"];
            var basket =  await basketRepository.GetBasketAsync(basketId) ??
                throw new BasketNotFoundException(basketId);
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product,int>()
                    .GetAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
            if (!basket.DeliveryMethodId.HasValue) throw new
                   Exception("No DeliverMethod Was Selected");
            var deliveryMethod = await unitOfWork
                .GetRepository<DeliveryMethod, int>()
                .GetAsync(basket.DeliveryMethodId.Value) ??
                throw new DeliveryMethodException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
            var total = (long)(basket.Items.Sum(i => i.Quantity * i.Price) +
                basket.ShippingPrice) * 100;
            var stripServices = new PaymentIntentService();
            if (String.IsNullOrWhiteSpace(basket.PaymentIntentId))
            {
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = total,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await stripServices.CreateAsync(createOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = total
                };
                await stripServices.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }
            await basketRepository.CreateOrUpdateBasketAsync(basket);
            return mapper.Map<BasketDto>(basket);
        }

        public async Task UpdateOrderPaymentStatusAsync(string json, string header)
        {
            var endpointSecret = configuration.GetSection("StripeSettings")
                ["EndPointSecret"];

            
                var stripeEvent = EventUtility.ParseEvent(json,throwOnApiVersionMismatch:false);

                stripeEvent = EventUtility.ConstructEvent(json,
                    header, endpointSecret, throwOnApiVersionMismatch: false);
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;


            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentIntentSucceeded(paymentIntent!.Id);
                    break;
                case EventTypes.PaymentIntentPaymentFailed:
                   await UpdatePaymentIntentFailed(paymentIntent!.Id);
                    break;

                default:
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }



        }

        #region Helper Methods

        private async Task UpdatePaymentIntentFailed(string paymentIntentId)
        {
            var orderRepo =  unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo
                .GetAsync(new OrderWithPaymentSpecifications(paymentIntentId)) ??
                throw new Exception();
            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            orderRepo.Update(order);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentIntentSucceeded(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo
                .GetAsync(new OrderWithPaymentSpecifications(paymentIntentId)) ??
                throw new Exception();
            order.PaymentStatus = OrderPaymentStatus.PaymentReceived;
            orderRepo.Update(order);
            await unitOfWork.SaveChangesAsync();
        }

        #endregion
    }
}
