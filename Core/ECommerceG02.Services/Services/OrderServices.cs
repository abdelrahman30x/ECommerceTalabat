using AutoMapper;
using ECommerceG02.Abstractions.Services;
using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Contacts.UOW;
using ECommerceG02.Domian.Exceptions.NotFound;
using ECommerceG02.Domian.Models.Orders;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Services.Specifications;
using ECommerceG02.Shared.DTOs.AddressDtos;
using ECommerceG02.Shared.DTOs.OrderDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.Services
{
    public class OrderServices(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IOrderServices
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string BuyerEmail)
        {
            var OrderAddress = mapper.Map<AddressDto, OrderAddress>(orderDto.Address);
            var Basket = await basketRepository.GetBasketAsync(orderDto.BasketId) ?? throw new BasketNotFoundException(orderDto.BasketId);
            List<OrderItem> orderItems = new List<OrderItem>();
            var ProductRepo = unitOfWork.GetReposatory<Product, int>();
            foreach (var item in Basket.Items)
            {
                var product = await ProductRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                var orderItemProduct = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(orderItemProduct, product.Price, item.Quantity);
                orderItems.Add(orderItem);


            }
            var DeliveryMethodRepo = unitOfWork.GetReposatory<DeliveryMethod, int>();
            var deliveryMethod = await DeliveryMethodRepo.GetByIdAsync(orderDto.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            var order = new Order(BuyerEmail, OrderAddress, deliveryMethod, orderItems, subtotal, Basket.PaymentIntentId);
            unitOfWork.GetReposatory<Order, Guid>().Add(order);
            await unitOfWork.SaveChangesAsync();

            Console.WriteLine($"Order BuyerEmail: {order.UserEmail}");
            Console.WriteLine($"Order Items count: {order.Items.Count}");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"Product Name: {item.Product.ProductName}, PictureUrl: {item.Product.PictureUrl}");
            }

            return mapper.Map<Order, OrderToReturnDto>(order);

        }
        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id, string BuyerEmail)
        {
            var spec = new OrderSpecifications(id, BuyerEmail);
            var order = await unitOfWork.GetReposatory<Order, Guid>().GetByIdWithSpecifiactionAsync(spec) ?? throw new OrderNotFoundException(id);
            return mapper.Map<Order, OrderToReturnDto>(order);
        }
        public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecifications(BuyerEmail);
            var orders = await unitOfWork.GetReposatory<Order, Guid>().GetAllWithSpecificationAsync(spec);
            return mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(orders);
        }
        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetReposatory<DeliveryMethod, int>().GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

    }
}
