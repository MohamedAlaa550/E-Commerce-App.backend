using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Order
{
    public record OrderRequest
    {
        public AddressDto ShippingAddress { get; set; }
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
    }
}
