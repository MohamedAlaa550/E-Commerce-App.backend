global using ShippingAddress = Domain.Entities.OrderModule.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
        }

        public Order(string userEmail, Address shipToAddress,
            ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod,
            decimal subtotal,string paymentIntentId)
        {
            UserEmail = userEmail;
            ShippingAddress = shipToAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string UserEmail { get; set; } = string.Empty;
        public Address ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; } // FK

        public decimal Subtotal { get; set; }

        public string PaymentIntentId { get; set; } 
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    }
}
