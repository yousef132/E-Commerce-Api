using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketServices;
using Store.Service.Services.BasketServices.Dtos;

namespace Session_1.Controllers
{
	
	public class BasketController : BaseController
	{
		private readonly IBasketService basketService;

		public BasketController(IBasketService basketService)
        {
			this.basketService = basketService;
		}

        [HttpGet("{id}")]
		public async Task<ActionResult<CustomerBasketDto>> GetBasketById (string id)
			=> Ok(await basketService.GetBasketAsync(id));


		[HttpPost]
		public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto basketDto)
			=> Ok(await basketService.UpdateBasketAsync(basketDto));


		[HttpDelete]
		public async Task<ActionResult> DeleteBasketAsync(string Id )
			=> Ok(await basketService.DeleteBasketAsync(Id));


	}
}
