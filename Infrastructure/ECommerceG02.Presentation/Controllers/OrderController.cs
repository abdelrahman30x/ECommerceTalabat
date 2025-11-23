using ECommerceG02.Abstractions.Services;
using ECommerceG02.Shared.DTOs.OrderDto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceG02.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Orders")]
    public class OrderController(IServicesManager ServicesManager) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id)
        {
            var ByerEmail = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("User email not found");
            var order = await ServicesManager.OrderServices.GetOrderByIdAsync(id, ByerEmail);
            return Ok(order);
        }
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var ByerEmail = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("User email not found");
            var order = await ServicesManager.OrderServices.CreateOrderAsync(orderDto, ByerEmail);
            return Ok(order);

        }
        [HttpGet]
        [Route("deliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var deliveryMethods = await ServicesManager.OrderServices.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersForUser()
        {
            var ByerEmail = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("User email not found");
            var orders = await ServicesManager.OrderServices.GetOrdersForUserAsync(ByerEmail);
            return Ok(orders);
        }

    }
}
