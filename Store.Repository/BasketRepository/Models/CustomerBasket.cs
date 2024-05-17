using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities
{
	public class CustomerBasket
	{
		public string Id { get; set; }	
		public int? DeliveryMethod { get; set; }
		public decimal ShopingPrice { get; set; }
		public List<BasketProduct> Products { get; set; } = new List<BasketProduct>();
		public string? PaymentIntentId { get; set; }
		public string? ClientSecret { get; set; }
	}
}
