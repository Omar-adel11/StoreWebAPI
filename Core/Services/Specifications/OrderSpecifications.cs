using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderSpecifications : BaseSpecifications<Guid, Domain.Entities.Order.Order>
    {
        public OrderSpecifications(Guid Id, string UserEmail) : base(o => o.Id == Id && o.UserEmail.ToLower() == UserEmail.ToLower())
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);



            OrderBydescending = o => o.OrderDate;
        }
        public OrderSpecifications( string UserEmail) : base(o => o.UserEmail.ToLower() == UserEmail.ToLower())
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            OrderBydescending = o => o.OrderDate;
        }
    }
}
