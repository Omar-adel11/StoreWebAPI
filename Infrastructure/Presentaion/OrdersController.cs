    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ServicesAbstractions;
    using Shared.DTOs.Orders;

    namespace Presentaion
    {
        [ApiController]
        [Route("api/[controller]")]
    
        public class OrdersController(IServiceManager _servicemanager) : ControllerBase
        {
            [HttpPost]
            public async Task<IActionResult> CreateOrder(OrderRequest request)
            {
                var userEmailClaim =  User.FindFirst(ClaimTypes.Email);
                var result = await _servicemanager.orderService.CreateOrderAsync(request, userEmailClaim.Value);
                return Ok(result);
            }

        // GET: api/orders
            [HttpGet]
            public async Task<IActionResult> GetOrdersForUser()
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var orders = await _servicemanager.orderService.GetOrdersForSpecificUserAsync(userEmail);
                return Ok(orders);
            }

            // GET: api/orders/{id}
            [HttpGet("{id}")]
            public async Task<IActionResult> GetOrderById([FromRoute]Guid id)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var order = await _servicemanager.orderService.GetOrderByIdForSpecificUserAsync(id, userEmail);
                return Ok(order);
            }

            // GET: api/orders/deliveryMethods
            [HttpGet("deliveryMethods")]
            public async Task<IActionResult> GetDeliveryMethods()
            {
                var deliveryMethods = await _servicemanager.orderService.GetAllDeliveryMethodAsync();
                return Ok(deliveryMethods);
            }



    }
    }
