using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManger serviceManger) : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create (OrderRequest request)
        {
           var email= User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManger.OrderService.CreateOrderAsync(request,email);
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await serviceManger.OrderService.GetOrdersByEmailAsync(email);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
        {
            var order = await serviceManger.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        [AllowAnonymous]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethods()
        {
            var methods = await serviceManger.OrderService.GetDeliveryMethodsAsync();
            return Ok(methods);
        }
    }
}
