using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Order;
using Store.Service.Services.BasketServices;
using Store.Service.Services.BasketServices.Dtos;
using Store.Service.Services.OrderService.Dtos;
using Stripe;
using Product = Store.Data.Entities.Product;
using Order = Store.Data.Entities.OrderEntities.Order;
using Store.Data.Entities.OrderEntities;
using AutoMapper;

namespace Store.Service.Services.PaymentService
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration configuration;
		private readonly IBasketService basketServices;
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;

		public PaymentService(IConfiguration configuration,
			IBasketService basketServices,
			IUnitOfWork unitOfWork,
			IMapper mapper
			)
        {
			this.configuration = configuration;
			this.basketServices = basketServices;
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExsitingOrder(CustomerBasketDto customerBasket)
		{
			StripeConfiguration.ApiKey = configuration["Stripe:Secretkey"];
			if (customerBasket == null)
				throw new Exception("Basket Is Null");
			
			// Count Total Price 

			var DeliveryMethod = await unitOfWork
								.Reposirory<DeliveryMethod, int>()
								.GetByIdAsync(customerBasket.DeliveryMethod.Value);

			var shippingPrice = DeliveryMethod.Price;

			foreach( var item  in customerBasket.Products)
			{
				var product = await unitOfWork.Reposirory<Product, int>().GetByIdAsync(item.Id);
				if (item.Price != product.Price)
					item.Price = product.Price;
			}

			var services =  new PaymentIntentService();
			PaymentIntent paymentIntent;
			// basket has no paymentIntentId
			if (string.IsNullOrEmpty(customerBasket.PaymentIntentId))
			{
				var options = new PaymentIntentCreateOptions
				{
					Amount = (long)customerBasket.Products.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
					Currency = "usd",
					PaymentMethodTypes = new List<string> { "card"}
				};
				
				paymentIntent = await services.CreateAsync(options);
				customerBasket.PaymentIntentId = paymentIntent.Id;
				customerBasket.ClientSecret = paymentIntent.ClientSecret;
			}
			else
			{
				var options = new PaymentIntentUpdateOptions
				{
					Amount = (long)customerBasket.Products.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
				};
				await services.UpdateAsync(customerBasket.PaymentIntentId, options);
			}
			await basketServices.UpdateBasketAsync(customerBasket);
			return customerBasket;
		}

		public  async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string BasketId)
		{
			StripeConfiguration.ApiKey = configuration["Stripe:Secretkey"];
			var customerBasket = await basketServices.GetBasketAsync(BasketId);
			if (customerBasket == null)
				throw new Exception("Basket Is Null");

			// Count Total Price 

			var DeliveryMethod = await unitOfWork
								.Reposirory<DeliveryMethod, int>()
								.GetByIdAsync(customerBasket.DeliveryMethod.Value);

			var shippingPrice = DeliveryMethod.Price;

			foreach (var item in customerBasket.Products)
			{
				var product = await unitOfWork.Reposirory<Product, int>().GetByIdAsync(item.Id);
				if (item.Price != product.Price)
					item.Price = product.Price;
			}

			var services = new PaymentIntentService();
			PaymentIntent paymentIntent;
			// basket has no paymentIntentId
			if (string.IsNullOrEmpty(customerBasket.PaymentIntentId))
			{
				var options = new PaymentIntentCreateOptions
				{
					Amount = (long)customerBasket.Products.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
					Currency = "usd",
					PaymentMethodTypes = new List<string> { "card" }
				};

				paymentIntent = await services.CreateAsync(options);
				customerBasket.PaymentIntentId = paymentIntent.Id;
				customerBasket.ClientSecret = paymentIntent.ClientSecret;
			}
			else
			{
				var options = new PaymentIntentUpdateOptions
				{
					Amount = (long)customerBasket.Products.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
				};
				await services.UpdateAsync(customerBasket.PaymentIntentId, options);
			}
			await basketServices.UpdateBasketAsync(customerBasket);
			return customerBasket;
		}

		public async Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentId)
		{
			var specs = new OrderWithItemsSpecifications(paymentIntentId);

			var order = await unitOfWork.
				Reposirory<Order, Guid>()
				.GetWithSpecificationsByIdAsync(specs);

			if (order is null)
				throw new Exception("Order Doesn't Exist");
			order.OrderPaymentStatus = OrderPaymentStatus.Failed;

			unitOfWork.Reposirory<Order, Guid>().Update(order);
			await unitOfWork.CompleteAsync();

			return mapper.Map<OrderResultDto>(order);	
		}

		public async Task<OrderResultDto> UpdateOrderPaymentSucceded(string paymentIntentId)
		{
			var specs = new OrderWithItemsSpecifications(paymentIntentId);

			var order = await unitOfWork.
				Reposirory<Order, Guid>()
				.GetWithSpecificationsByIdAsync(specs);

			if (order is null)
				throw new Exception("Order Doesn't Exist");
			order.OrderPaymentStatus = OrderPaymentStatus.Recieved;

			unitOfWork.Reposirory<Order, Guid>().Update(order);
			await unitOfWork.CompleteAsync();

			await basketServices.DeleteBasketAsync(order.BasketId);

			var mappedOrder = mapper.Map<OrderResultDto>(order);

			//Just for Debugging
			//mappedOrder.BasketId = order.BasketId;
			return mappedOrder;	
		}
	}
}
