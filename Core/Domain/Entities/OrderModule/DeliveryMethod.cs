using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderModule
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public DeliveryMethod()
        {
        }

        public DeliveryMethod(string shortName, string description, string deliveryTime, decimal price)
        {
            ShortName = shortName;
            Description = description;
            DeliveryTime = deliveryTime;
            Price = price;
        }

        public string ShortName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DeliveryTime { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
