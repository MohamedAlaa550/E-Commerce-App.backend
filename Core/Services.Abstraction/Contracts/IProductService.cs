using Shared;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(ProductSpecParams parameters);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();

        Task<IEnumerable<TypeDto>> GetAllTypesAsync();



    }
}
