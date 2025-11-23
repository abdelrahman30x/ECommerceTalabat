using ECommerceG02.Domian.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.Specifications
{
    public class OrderSpecifications : BaseSpecification<Order, Guid>
    {
        public OrderSpecifications(string buyerEmail)
            : base(o => o.UserEmail == buyerEmail)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDesc(o => o.OrderDate);
            AddInclude(o => o.ShipToAddress);

        }
        public OrderSpecifications(Guid id, string buyerEmail)
            : base(o => o.Id == id && o.UserEmail == buyerEmail)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.ShipToAddress);

        }
    }
}
