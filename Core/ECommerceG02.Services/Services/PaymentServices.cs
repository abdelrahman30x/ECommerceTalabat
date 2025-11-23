using ECommerceG02.Abstractions.Services;
using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Contacts.UOW;
using ECommerceG02.Domian.Exceptions.NotFound;
using ECommerceG02.Domian.Models.Orders;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Shared.DTOs.BasketDto_s;
using ECommerceG02.Shared.DTOs.OrderDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace ECommerceG02.Services.Services
{
    public class PaymentServices(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper) : IPaymentServices
    {
        public async Task<BasketDto> CreatePaymentIntentAsync(string BasketId)
        {
            Console.WriteLine("==== CreatePaymentIntentAsync START ====");
            Console.WriteLine($"BasketId: {BasketId}");

            var basket = await basketRepository.GetBasketAsync(BasketId);

            if (basket == null)
            {
                Console.WriteLine("Basket not found.");
                throw new BasketNotFoundException(BasketId);
            }

            Console.WriteLine($"Current PaymentIntentId: {basket.PaymentIntentId}");
            Console.WriteLine($"DeliveryMethodId: {basket.DeliveryMethodId}");

            // تحديث أسعار المنتجات
            foreach (var item in basket.Items)
            {
                var productRepo = unitOfWork.GetReposatory<Domian.Models.Products.Product, int>();
                var product = await productRepo.GetByIdAsync(item.Id);

                Console.WriteLine($"Item → ProductId: {item.Id}, OldPrice: {item.Price}, NewPrice: {product.Price}");

                item.Price = product.Price;
            }

            var subtotal = basket.Items.Sum(i => i.Price * i.Quantity);
            Console.WriteLine($"Subtotal: {subtotal}");

            // الشحن
            var deliveryMethodRepo = unitOfWork.GetReposatory<DeliveryMethod, int>();
            var deliveryMethod = await deliveryMethodRepo.GetByIdAsync(basket.DeliveryMethodId.Value);

            Console.WriteLine($"DeliveryMethod Price: {deliveryMethod.Price}");

            basket.ShippingCost = deliveryMethod.Price;

            var amountToBePaid = subtotal + deliveryMethod.Price;
            Console.WriteLine($"Total Amount: {amountToBePaid}");

            // Stripe key
            var key = configuration["StripeSettings:SecretKey"];
            Console.WriteLine($"Stripe Key: {key.Substring(0, 8)}***********");
            StripeConfiguration.ApiKey = key;

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            // إنشاء/تحديث PaymentIntent
            if (basket.PaymentIntentId == null)
            {
                Console.WriteLine("Creating NEW PaymentIntent...");

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amountToBePaid * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                };

                paymentIntent = await service.CreateAsync(options);
                Console.WriteLine($"Stripe Created PaymentIntent: {paymentIntent.Id}");
            }
            else
            {
                Console.WriteLine($"Updating EXISTING PaymentIntent: {basket.PaymentIntentId}");

                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(amountToBePaid * 100),
                };

                try
                {
                    paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);
                    Console.WriteLine($"Stripe Updated PaymentIntent: {paymentIntent.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("UPDATE PAYMENT INTENT FAILED:");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);

                    throw;
                }
            }

            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            basket = await basketRepository.CreateUpdateBasketAsync(basket);

            Console.WriteLine($"Final PaymentIntentId Saved to Redis: {basket.PaymentIntentId}");
            Console.WriteLine("==== CreatePaymentIntentAsync END ====");

            return mapper.Map<BasketDto>(basket);
        }

        public Task<BasketDto> CancelPaymentIntentAsync(string BasketId)
        {
            throw new NotImplementedException();
        }

        public Task<BasketDto> ConfirmPaymentIntentAsync(string BasketId)
        {
            throw new NotImplementedException();
        }

       
        public Task<BasketDto> GetPaymentIntentAsync(string BasketId)
        {
            throw new NotImplementedException();
        }

        public Task<BasketDto> RefundPaymentIntentAsync(string BasketId)
        {
            throw new NotImplementedException();
        }

        public Task<BasketDto> UpdatePaymentIntentAsync(string BasketId)
        {
            throw new NotImplementedException();
        }
    }
}
