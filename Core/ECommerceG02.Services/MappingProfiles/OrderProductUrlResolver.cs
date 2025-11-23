using AutoMapper;
using ECommerceG02.Domian.Models.Orders;
using ECommerceG02.Shared.DTOs.OrderDto_s;
using Microsoft.Extensions.Configuration;

namespace ECommerceG02.Services.MappingProfiles
{
    public class OrderProductUrlResolver
        : BasePictureUrlResolver<OrderItem, OrderItemDto>
    {
        public OrderProductUrlResolver(IConfiguration configuration) : base(configuration)
        {
        }

        public override string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            return BuildFullUrl(source.Product?.PictureUrl);
        }
    }
}
