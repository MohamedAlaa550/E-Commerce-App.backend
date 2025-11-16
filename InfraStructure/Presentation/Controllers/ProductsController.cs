using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos;
using Shared.Enums;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{

    public class ProductsController(IServiceManger serviceManger) : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagiantedResult<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams parametrs)
        => Ok(await serviceManger.ProductService.GetAllProductsAsync(parametrs));

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
            => Ok (await serviceManger.ProductService.GetAllBrandsAsync());

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
            => Ok (await serviceManger.ProductService.GetAllTypesAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ValidationErrorRespons), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<ProductDto?>> GetProduct(int id)
        {
            var productDto = await serviceManger.ProductService.GetProductByIdAsync(id);
            if (productDto is null)
                return NotFound();
            return Ok(productDto);
        }
    }

    
}
