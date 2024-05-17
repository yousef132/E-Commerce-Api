using AutoMapper;
using Store.Data.Entities;
using Store.Repository.BasketRepository;
using Store.Service.Services.BasketServices.Dtos;

namespace Store.Service.Services.BasketServices
{
	public class BasketService : IBasketService
	{
		private readonly IBasketRepository basketRepository;
		private readonly IMapper mapper;

		public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
			this.basketRepository = basketRepository;
			this.mapper = mapper;
		}
        public async Task<bool> DeleteBasketAsync(string id)
			=> await basketRepository.DeleteBasketAsync(id);

		public async Task<CustomerBasketDto> GetBasketAsync(string id)
		{
			var basket = await basketRepository.GetBasketAsync(id);

			if (basket is null)
				return new CustomerBasketDto();

			var mappedBasket = mapper.Map<CustomerBasketDto>(basket);
			return mappedBasket;
		}

		public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto cart)
		{
			var customerBasket = mapper.Map<CustomerBasket>(cart);
			var updatedCustomerBasket = await basketRepository.UpdateBasketAsync(customerBasket);
			var mappedCustomerBasket = mapper.Map<CustomerBasketDto>(updatedCustomerBasket);
			return mappedCustomerBasket; 
		}
	}
}
