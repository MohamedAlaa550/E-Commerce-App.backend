using Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ServiceMangerWithFactoryDelegate(Func<IProductService > _productService
        ,Func<IBasketService> _basketService,
       Func<IAuthenticationService> _authenticationService,
        Func<IOrderService> _orderService,
        Func<IPaymentService> _paymentService) : IServiceManger
    {
        public IProductService ProductService => _productService.Invoke();

        public IBasketService BasketService => _basketService.Invoke();

        public IAuthenticationService AuthenticationService => _authenticationService.Invoke();

        public IOrderService OrderService => _orderService.Invoke();

        public IPaymentService PaymentService => _paymentService.Invoke();
    }
}
