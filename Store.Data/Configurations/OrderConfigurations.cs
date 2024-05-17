using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Configurations
{
	public class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			// ShippingAddress is not an entity
			builder.OwnsOne(order => order.ShippingAddress, x =>
			{
				x.WithOwner();
			});
			builder.HasMany(x=>x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
		}
	}
}
