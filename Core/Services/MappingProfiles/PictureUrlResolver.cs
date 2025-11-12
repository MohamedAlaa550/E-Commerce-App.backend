using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.Internal;
using Domain.Entities.ProductModule;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration configuration) : 
        IValueResolver<Product, ProductDto, string>
    {
    public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.PictureUrl))
            return string.Empty;
            return $"{configuration["BaseUrl"]}/{source.PictureUrl}";
        }
    }
}
