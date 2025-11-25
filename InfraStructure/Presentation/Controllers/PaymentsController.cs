using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class PaymentsController(IServiceManger serviceManger) : ApiControllerBase
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePayment(string basketId)
       => Ok(await serviceManger.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId));
    }
}
