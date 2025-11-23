using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Abstractions.Services
{
    public interface IServicesManager
    {
        public IProductServices ProductServices { get; }
        public IBasketServices BasketServices { get; }
        public IAuthServices AuthenticationServices { get; }

        public IOrderServices OrderServices { get; }

        public IPaymentServices PaymentServices { get; }

        public ICacheServices CacheServices { get; }
    }
}

