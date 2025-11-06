using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Shared.DTOs.Orders;

namespace ServicesAbstractions.Orders
{
    public interface IOrderService
    {
        Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string UserEmail);
        /*summary
         * Parameters:
         *      UserEmail from token
         *      OrderRequest from body (BasketId, ShippingAddressId, DeliveryMethodId)
         * returns OrderResponse 
         * 
         * Creates an order based on the provided OrderRequest and UserEmail.
         */


        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodAsync();
        /*summary
         * returns all DeliveryMethodDto
         */

        Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid Id, string UserEmail);
        /*summary
         * Parameters:
         *      Id from route
         *      UserEmail from token
         * returns OrderResponse for specific user
         */

        Task<IEnumerable<OrderResponse?>> GetOrdersForSpecificUserAsync(string UserEmail);
        /*summary
         * Parameters:
         *      UserEmail from token
         * returns all OrderResponse for specific user
         */
    }
}
