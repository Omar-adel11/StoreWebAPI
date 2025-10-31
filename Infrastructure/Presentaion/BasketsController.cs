using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Basket;

namespace Presentaion
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager serviceManager) :  ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetBasketById(string Id)
        {
            var result = await serviceManager.basketService.GetBasketAsync(Id);
            return Ok(result);
                
        }


        [HttpPost]
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var result = await serviceManager.basketService.UpdateBasketAsync(basketDto);
            return Ok(result);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string Id)
        {
            var result = await serviceManager.basketService.DeleteBasketAsync(Id);
            return NoContent();

        }


    }
}
