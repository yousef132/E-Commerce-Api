using AutoMapper;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketServices.Dtos
{
	public class BasketProfiler:Profile
	{
        public BasketProfiler()
        {
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketProduct, BasketItemDto>().ReverseMap();
        }
    }
}
