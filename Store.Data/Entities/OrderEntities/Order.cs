﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities.OrderEntities
{
	public enum OrderPaymentStatus
	{
		Pending,
		Recieved,
		Failed
	};
	public class Order:BaseEntity<Guid>
	{
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

		public ShippingAddress ShippingAddress { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }

		public int? DeliveryMethodId { get; set; }

		public OrderPaymentStatus OrderPaymentStatus { get; set; } = OrderPaymentStatus.Pending;

		public IReadOnlyList<OrderItem> OrderItems { get; set; }
		public double SubTotal { get; set; }
		public string? PaymentIntentId { get; set; }

		public double GetTotal() => SubTotal + DeliveryMethod.Price;
		public string? BasketId { get; set; }

	}
}
