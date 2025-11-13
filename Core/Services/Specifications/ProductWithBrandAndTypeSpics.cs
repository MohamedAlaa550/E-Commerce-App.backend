using Domain.Entities.ProductModule;
using Shared;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpics : BaseSpecifications<Product,int>
    {
        public ProductWithBrandAndTypeSpics(ProductSpecParams parameters) 
            : base(product=>
            (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId.Value ) &&
            (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId.Value))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            switch(parameters.sort)
            {
                case ProductSortringOptions.NameAsc:
                    SetOrderBy(p => p.Name);
                    break;
                case ProductSortringOptions.NameDesc:
                    SetOrderByDesc(p => p.Name);
                    break;
                    case ProductSortringOptions.PriceAsc:
                        SetOrderBy(p => p.Price);
                    break;
                    case ProductSortringOptions.PriceDesc:
                        SetOrderByDesc(p => p.Price);
                    break;
                default:
                    SetOrderBy(p => p.Name);
                    break;
            }
            ApplyPagination(parameters.pageIndex, parameters.pageSize);
        }
        public ProductWithBrandAndTypeSpics(int id) 
            : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
