namespace Store.Service.Services.OrderService.Dtos
{
	public class OrderItemDto
	{
		public double Price { get; set; }

		public int Quantity { get; set; }

		public int ProductId { get; set; }

		public string ProductName { get; set; }
		public string PictureUrl { get; set; }

		public Guid OrderId { get; set; }
	}
}