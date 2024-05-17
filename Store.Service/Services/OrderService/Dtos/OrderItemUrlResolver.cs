using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities.OrderEntities;

namespace Store.Service.Services.OrderService.Dtos
{
	partial class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration configuration;

		public OrderItemUrlResolver(IConfiguration configuration)
        {
			this.configuration = configuration;
		}

		public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.ProductItemOrdered.PictureUrl))
				return $"{configuration["BaseUrl"]}{source.ProductItemOrdered.PictureUrl}";

			return null;
		}
	}
}