using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecifications<Guid,Domain.Entities.Order.Order>
    {
        public OrderByPaymentIntentIdSpecification(string PaymentIntentId) : base(o => o.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
