using Store.Service.Services.BasketServices.Dtos;
using Store.Service.Services.OrderService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.PaymentService
{
	public interface IPaymentService
	{
		Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExsitingOrder(CustomerBasketDto customerBasket);
		Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketId);

		Task<OrderResultDto> UpdateOrderPaymentSucceded(string paymentIntentId);
		Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentId);

	}
}
