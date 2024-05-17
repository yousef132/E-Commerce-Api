using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Service.HandleResponses;
using Store.Service.Services.OrderService;
using Store.Service.Services.OrderService.Dtos;
using System.Security.Claims;

namespace Session_1.Controllers
{
	[Authorize]
	public class OrderController : BaseController
	{
		private readonly IOrderService orderService;

		public OrderController(IOrderService orderService)
        {
			this.orderService = orderService;
		}
		[HttpPost]

		public async Task<ActionResult<OrderResultDto>> CreateOrderAsync(OrderDto input)
		{
			var order = await orderService.CreateOrderAsync(input);
			if (order == null)
				return BadRequest(new Response(400, "Error While Creating Your Order"));

			return Ok(order);

		}
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<OrderResultDto>>> GetAllOrdersAsync()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var orders = await orderService.GetAllOrdersForUserAsync(email);

			return Ok(orders);
		}
		[HttpGet]
		public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync(Guid orderId)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var order = await orderService.GetOrderByIdAsync(orderId, email);

			return Ok(order);
		}
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethodsAsync()
			=> Ok(await orderService.GetAllDeliveryMethodsAsync());



	}
}
