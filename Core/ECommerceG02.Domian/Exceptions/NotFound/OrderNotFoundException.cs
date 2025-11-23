using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.NotFound
{
    public class OrderNotFoundException(Guid orderId) : NotFoundException($"Order with Id: {orderId} was not found.")
    {
    }
}
