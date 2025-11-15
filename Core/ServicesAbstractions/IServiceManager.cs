using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesAbstractions.Baskets;
using ServicesAbstractions.Identity;
using ServicesAbstractions.Orders;
using ServicesAbstractions.Payment;
using ServicesAbstractions.Products;

namespace ServicesAbstractions
{
    public interface IServiceManager
    {
         IProductService productService { get; }
         IBasketService basketService { get; }
         ICacheService cacheService { get; }
         IAuthService authService { get; }
         IOrderService orderService { get; }
         IPaymentService paymentService { get; }
    }
}
