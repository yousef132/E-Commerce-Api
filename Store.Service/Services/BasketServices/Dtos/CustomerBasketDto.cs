using Store.Data.Entities;

namespace Store.Service.Services.BasketServices.Dtos
{
	public class CustomerBasketDto
	{
		public string Id { get; set; }
		public int? DeliveryMethod { get; set; }

		public decimal ShopingPrice { get; set; }
		public List<BasketProduct> Products { get; set; } = new List<BasketProduct>();
		public string? PaymentIntentId { get; set; }
		public string? ClientSecret { get; set; }
	}
}
