using ECommerceG02.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Payments")]
    public class PaymentController(IServicesManager servicesManager) : ControllerBase
    {
      [HttpPost("{basketId}")]
        public async Task<ActionResult> CreatePaymentIntent(string basketId)
        {
            var basket = await servicesManager.PaymentServices.CreatePaymentIntentAsync(basketId);
            if (basket == null) return BadRequest(new { Error = "Problem with your basket" });
            return Ok(basket);
        }
    }
}
