using Store.Data.Entities.OrderEntities;
using Order = Store.Data.Entities.OrderEntities.Order;

namespace Store.Repository.Specification.Order
{
	public class OrderWithPaymentIntentSpecification : BaseSpecification<Store.Data.Entities.OrderEntities.Order>
	{
		public OrderWithPaymentIntentSpecification(string? paymentIntentId) 
			: base(order=>order.PaymentIntentId == paymentIntentId)
		{
		}
	}
}
