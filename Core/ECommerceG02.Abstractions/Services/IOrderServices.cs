using ECommerceG02.Shared.DTOs.OrderDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Abstractions.Services
{
    public interface IOrderServices
    {
        Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderCreateDto, string BuyerEmail);
        Task<OrderToReturnDto> GetOrderByIdAsync(Guid id, string BuyerEmail);
        Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string BuyerEmail);
        Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
    }
}
