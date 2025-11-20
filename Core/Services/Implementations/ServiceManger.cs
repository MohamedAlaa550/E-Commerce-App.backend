using AutoMapper;
using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstraction.Contracts;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ServiceManger(IUnitOfWork _unitOfWork, IMapper _mapper,
        IBasketRepository basketRepository, UserManager<User> userManager, IOptions<JwtOptions> options) : IServiceManger
    {
        private readonly Lazy<IProductService> _productService 
            = new Lazy<IProductService>(()=> new ProductService(_unitOfWork, _mapper));

        private readonly Lazy<IBasketService> _basketService = new Lazy<IBasketService>(()=> 
        new BasketService(basketRepository, _mapper));

        private readonly Lazy<IAuthenticationService> _authenticationService = 
            new Lazy<IAuthenticationService>(() =>new AuthenticationService(userManager,options));



        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
