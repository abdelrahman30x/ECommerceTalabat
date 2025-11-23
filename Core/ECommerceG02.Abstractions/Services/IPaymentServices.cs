using ECommerceG02.Shared.DTOs.BasketDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Abstractions.Services
{
    public interface IPaymentServices
    {
        Task<BasketDto> CreatePaymentIntentAsync(string BasketId);
        Task<BasketDto> UpdatePaymentIntentAsync(string BasketId);

        Task<BasketDto> CancelPaymentIntentAsync(string BasketId);

        Task<BasketDto> ConfirmPaymentIntentAsync(string BasketId);
        Task<BasketDto> GetPaymentIntentAsync(string BasketId);

        Task<BasketDto> RefundPaymentIntentAsync(string BasketId);

        
    }
}
