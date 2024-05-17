using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Order
{
	public class OrderWithItemsSpecifications : BaseSpecification<Data.Entities.OrderEntities.Order>
	{
		public OrderWithItemsSpecifications(string buyerEmail) 
			: base(order=>order.BuyerEmail== buyerEmail)
		{
			AddInclude(o => o.OrderItems);
			AddInclude(o => o.DeliveryMethod);
			AddOrderByDescending(o => o.OrderDate);

		}
		public OrderWithItemsSpecifications(string buyerEmail , Guid id) 
			: base(order=>order.BuyerEmail== buyerEmail && order.Id == id)
		{
			AddInclude(o => o.OrderItems);
			AddInclude(o => o.DeliveryMethod);
		}


	}
}
