using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Order
{
    //Table: Orders
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            //parameterless constructor for EF
        }
        public Order(string userEmail, OrderAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> orderItems, decimal subtotal, string? paymentIntentId)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            this.Items = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } //Navigational Property
        public int DeliveryMethodId { get; set; } //FK
        public ICollection<OrderItem> Items  { get; set; } //Navigational Property
        public decimal Subtotal { get; set; } // price * quantity

        //[NotMapped]
        //public decimal Total { get; set; } // subtotal + delivery fee
        public decimal GetTotal() =>  Subtotal + DeliveryMethod.Price; //not mapped 

        public string? PaymentIntentId { get; set; }

        }
}
