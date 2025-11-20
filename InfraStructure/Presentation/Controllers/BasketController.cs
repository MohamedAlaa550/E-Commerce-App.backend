using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class BasketController(IServiceManger serviceManger) : ApiControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
      
        public async Task<ActionResult<BasketDto>> Get(string id)
        => Ok(await serviceManger.BasketService.GetBasketAsync(id));

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basket)
            => Ok(await serviceManger.BasketService.CreateOrUpdateBasketAsync(basket));

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await serviceManger.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }

    }
}
