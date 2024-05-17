using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Order;
using Store.Service.Services.BasketServices;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;

namespace Store.Service.Services.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IBasketService basketService;
		private readonly IMapper mapper;
		private readonly IPaymentService paymentService;

		public OrderService(IUnitOfWork unitOfWork
			,IBasketService basketService,
			IMapper mapper,
			IPaymentService paymentService)
        {
			this.unitOfWork = unitOfWork;
			this.basketService = basketService;
			this.mapper = mapper;
			this.paymentService = paymentService;
		}
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto orderDto)
		{
			// Get Basket
			var basket = await basketService.GetBasketAsync(orderDto.BasketId);
			if (basket is null)
				throw new Exception("Basket Not Found");

			// Fill OrderItems From BasketItems

			var orderItems = new List<OrderItemDto>();

			// fill orderItems
			foreach(var basketItem in basket.Products)
			{
				var productItem = await unitOfWork.Reposirory<Product, int>().GetByIdAsync(basketItem.Id);
				if (productItem is null)
					throw new Exception($"Product With Id : {basketItem.Id} Not Exist");

				var itemOrdered = new ProductItemOrdered
				{
					ProductId = productItem.Id,
					ProductName = productItem.Name,
					PictureUrl = productItem.PictureUrl,
				};
				var orderItem = new OrderItem
				{
					Price = productItem.Price,
					Quantity = basketItem.Quantity,
					ProductItemOrdered = itemOrdered,
				};
				var mappedProduct = mapper.Map<OrderItemDto>(orderItem);

				orderItems.Add(mappedProduct);	
			}


			// Get Delivery Method
			var deliveryMethod = await unitOfWork
								 .Reposirory<DeliveryMethod, int>()
								 .GetByIdAsync(orderDto.DeliveryMethodId);

			if (deliveryMethod is null)
				throw new Exception("Delivery Method Not Found");

			//Calculate SubTotal
			var subTotal = orderItems.Sum(item=>item.Price*item.Quantity);

			//To Do => Check If Order Exist

			var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
			var existingOrder = await unitOfWork.Reposirory<Order,Guid>().GetWithSpecificationsByIdAsync(specs);
			if (existingOrder is not null)
			{
				unitOfWork.Reposirory<Order, Guid>().Delete(existingOrder);
				await paymentService.CreateOrUpdatePaymentIntentForExsitingOrder(basket);
			}
			else
				await paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);

			//Create Order
			var mappedShippingAddress = mapper.Map<ShippingAddress>(orderDto.ShippingAddress);
			var mappedOrderItems = mapper.Map<List<OrderItem>>(orderItems);
			Order order = new Order
			{
				SubTotal = subTotal,
				DeliveryMethodId = deliveryMethod.Id,	
				ShippingAddress = mappedShippingAddress,
				BuyerEmail = orderDto.BuyerEmail,
				OrderItems = mappedOrderItems,
				BasketId = basket.Id,
				
			};
			await unitOfWork.Reposirory<Order,Guid>().AddAsync(order);
			await unitOfWork.CompleteAsync();

			var mappedOrder = mapper.Map<OrderResultDto>(order);
			//mappedOrder.BasketId = basket.Id;
			return mappedOrder;
		}

		public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
			=> await unitOfWork.Reposirory<DeliveryMethod, int>().GetAllAsync();

		public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string BuyerEmail)
		{
			var specs = new OrderWithItemsSpecifications(BuyerEmail);

			var orders = await unitOfWork.Reposirory<Order, Guid>().GetWithSpecificationsAllAsync(specs);
			if(orders is { Count: <= 0 })
			{
				throw new Exception("CYou Don't Have Any Orders Yes ! ");
			}

			var mappedOrders = mapper.Map<IReadOnlyList<OrderResultDto>>(orders);

			return mappedOrders;
		}

		public async Task<OrderResultDto> GetOrderByIdAsync(Guid OrderId, string BuyerEmail)
		{
			var specs = new OrderWithItemsSpecifications(BuyerEmail, OrderId);

			var order = await unitOfWork.Reposirory<Order, Guid>().GetWithSpecificationsByIdAsync(specs);
			if (order is null)
				throw new Exception($"There is no Order With ID : {OrderId}");
			var mappedOrders = mapper.Map<OrderResultDto>(order);

			return mappedOrders;
		}
	}
}
