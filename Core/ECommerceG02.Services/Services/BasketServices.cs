using AutoMapper;
using ECommerceG02.Abstractions.Services;
using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Exceptions;
using ECommerceG02.Domian.Exceptions.NotFound;
using ECommerceG02.Domian.Models.Baskets;
using ECommerceG02.Shared.DTOs.BasketDto_s;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.Services
{
    public class BasketServices(IBasketRepository basketRepository, IMapper mapper, ILogger<BasketServices> _logger) : IBasketServices
    {
        private readonly IBasketRepository _basketRepository = basketRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<BasketDto?> CreateUpdateBasketAsync(BasketDto basket)
        {
            var basketEntity = _mapper.Map<CustomerBasket>(basket);
            var createdOrUpdatedBasket = await _basketRepository.CreateUpdateBasketAsync(basketEntity);   
            if (createdOrUpdatedBasket == null)
               throw new Exception("Failed to create or update basket.");
            return _mapper.Map<BasketDto>(createdOrUpdatedBasket);
        }
        public async Task<BasketDeleteDto> DeleteBasketAsync(string basketId)
        {
            var deleted = await _basketRepository.DeleteBasketAsync(basketId);

            return new BasketDeleteDto
            {
                Deleted = deleted,
                Message = deleted ? "Basket deleted successfully." : "Failed to delete basket."
            };
        }

        public async Task<BasketDto?> GetBasketAsync(string basketId)
        {
            var basketEntity = await _basketRepository.GetBasketAsync(basketId);

            if (basketEntity == null)
                throw new BasketNotFoundException(basketId);

            return _mapper.Map<BasketDto>(basketEntity);
        }

    }
}
