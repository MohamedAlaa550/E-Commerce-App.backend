using Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderWithIncludesSpecifications : BaseSpecifications<Order , Guid>
    {
        public OrderWithIncludesSpecifications(Guid id) : base(o => o.Id == id)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
        public OrderWithIncludesSpecifications(string email) : base(o => o.UserEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            SetOrderByDesc(o => o.OrderDate);
        }
    }
}
