using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Models.Baskets
{
    public class CustomerBasket
    {
        public string Id { get; set; } = null!;
        public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();

        public int? DeliveryMethodId { get; set; }

        public string? ClientSecret { get; set; }

        public string? PaymentIntentId { get; set; }

        public decimal? ShippingCost { get; set; }

    }
}
