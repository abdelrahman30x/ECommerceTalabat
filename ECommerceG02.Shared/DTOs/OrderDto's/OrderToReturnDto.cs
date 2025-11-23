using ECommerceG02.Shared.DTOs.AddressDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.DTOs.OrderDto_s
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public decimal ShippingPrice { get; set; }
        public string OrderStatus { get; set; } = null!;
        public AddressDto ShipToAddress { get; set; } = null!;
        public string DeliveryMethod { get; set; } = null!;

        public List<OrderItemDto> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal Total => SubTotal + ShippingPrice;


    }
}
