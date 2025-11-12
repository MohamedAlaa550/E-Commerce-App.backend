using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProductModule;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
       
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var brandDtos = _mapper.Map<IEnumerable<BrandDto>>(brands);
            return brandDtos;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(ProductSortringOptions sort)
        {
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync
                (new ProductWithBrandAndTypeSpics(sort));
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productDtos;
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
           var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
              var typeDtos = _mapper.Map<IEnumerable<TypeDto>>(types);
            return typeDtos;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().
                GetAsync(new ProductWithBrandAndTypeSpics(id));
            var productDto = _mapper.Map<ProductDto?>(product);
            return productDto;
        }
    }
}
