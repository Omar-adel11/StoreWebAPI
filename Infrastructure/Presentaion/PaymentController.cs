using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;

namespace Presentaion
{
    [ApiController]
    [Route("api/[controller]")]

    public class PaymentsController(IServiceManager _serviceManager) : ControllerBase
    {
       

        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntent([FromRoute]string basketId)
        {
            var result = await _serviceManager.paymentService.CreatePaymentIntentAsync(basketId);
            return Ok(result);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];
            await _serviceManager.paymentService.UpdatePaymentStatusAsync(json, signatureHeader);

            return new EmptyResult();
        }
    }
}
