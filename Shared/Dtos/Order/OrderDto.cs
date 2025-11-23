using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Order
{
    public record OrderDto
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public AddressDto ShippingAddress { get; init; } = new AddressDto();
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
        public string PaymentStatus { get; init; } = string.Empty;
        public string DeliveryMethod { get; init; } = string.Empty;
        public int? DeliveryMethodId { get; init; }
        public string PaymentIntentId { get; init; } = string.Empty;
        public decimal Subtotal { get; init; }
        public decimal Total { get; init; }
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.Now;
    }
}
