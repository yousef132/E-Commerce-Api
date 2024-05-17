using Store.Data.Entities.OrderEntities;

namespace Store.Service.Services.OrderService.Dtos
{
	public class OrderResultDto
	{
		public Guid Id { get; set; }
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } 

		public AddressDto ShippingAddress { get; set; }

		public string DeliveryMethodName { get; set; }

		public OrderPaymentStatus OrderPaymentStatus { get; set; }

		public IReadOnlyList<OrderItemDto> OrderItems { get; set; }

		public double SubTotal { get; set; }

		public double ShippingPrice { get; set; }
		public double Total { get; set; }

		public string? PaymentIntentId { get; set; }

		public string? BasketId { get; set; }

	}
}