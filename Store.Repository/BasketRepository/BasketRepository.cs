using StackExchange.Redis;
using Store.Data.Entities;
using System.Text.Json;

namespace Store.Repository.BasketRepository
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase database;

		public BasketRepository(IConnectionMultiplexer redis)
        {
			database = redis.GetDatabase();
            
        }
        public async Task<bool> DeleteBasketAsync(string id)
			=> await database.KeyDeleteAsync(id);

		public async Task<CustomerBasket> GetBasketAsync(string id)
		{
			var data = await database.StringGetAsync(id);
			if (data.IsNullOrEmpty)
				return null;

			return JsonSerializer.Deserialize<CustomerBasket>(data);
		}

		public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket cart)
		{
			var isCreated = await database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart),TimeSpan.FromDays(30));


			if (!isCreated)
				return null; 

			return await GetBasketAsync(cart.Id);

		}
	}
}
