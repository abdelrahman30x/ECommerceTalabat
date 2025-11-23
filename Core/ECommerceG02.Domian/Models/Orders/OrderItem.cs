using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Models.Orders
{
    public class OrderItem: BaseEntity<int>
    {
        public ProductItemOrdered Product { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        private OrderItem() { }
        public OrderItem(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }
    }
}
