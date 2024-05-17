using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.BasketRepository
{
	public interface IBasketRepository
	{

		Task<CustomerBasket> GetBasketAsync(string id);
		Task<CustomerBasket> UpdateBasketAsync(CustomerBasket cart);

		Task<bool> DeleteBasketAsync(string id);
	}
}
