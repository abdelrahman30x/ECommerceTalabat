using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.NotFound
{
    public class DeliveryMethodNotFoundException : NotFoundException
    {
        public DeliveryMethodNotFoundException(int Id)
            : base($"Delivery method with id {Id} not found.") { }
    }
}