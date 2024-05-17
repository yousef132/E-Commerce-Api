namespace Store.Data.Entities
{
	public class BasketProduct
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string Brand { get; set; }
		public double Price { get; set; }
		public string PictureUrl { get; set; }
		public string Type { get; set; }

		public int Quantity { get; set; }
	}
}