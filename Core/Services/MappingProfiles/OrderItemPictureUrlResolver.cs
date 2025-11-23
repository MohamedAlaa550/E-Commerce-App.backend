using AutoMapper;
using Domain.Entities.OrderModule;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class OrderItemPictureUrlResolver(IConfiguration configuration) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.Product.PictureUrl))
                return string.Empty;
            return $"{configuration["BaseUrl"]}{source.Product.PictureUrl}";
        }
    }
}
