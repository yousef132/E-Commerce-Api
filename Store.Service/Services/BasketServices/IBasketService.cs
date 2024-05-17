using Store.Data.Entities;
using Store.Service.Services.BasketServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketServices
{
	public interface IBasketService
	{
		Task<CustomerBasketDto> GetBasketAsync(string id);
		Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto cart);

		Task<bool> DeleteBasketAsync(string id);
	}
}
