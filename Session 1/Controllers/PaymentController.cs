using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketServices.Dtos;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using Stripe;

namespace Session_1.Controllers
{

	public class PaymentController : BaseController
	{
		private readonly IPaymentService paymentService;
		private readonly ILogger<PaymentController> logger;
		private const string endpointSecret = "whsec_4fc029d54cfa66813bfb34452e9789db37566f327fcd128ed23795a200f4987f";
		public PaymentController(IPaymentService paymentService
			, ILogger<PaymentController> logger)
		{
			this.paymentService = paymentService;
			this.logger = logger;
		}

		[HttpPost("{basketId}")]

		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExsitingOrder(CustomerBasketDto basket)
		=> Ok(await paymentService.CreateOrUpdatePaymentIntentForExsitingOrder(basket));

		[HttpPost("{basketId}")]


		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
		=> Ok(await paymentService.CreateOrUpdatePaymentIntentForNewOrder(basketId));


		[HttpPost("webhook")]
		public async Task<IActionResult> Index() 
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			try
			{
				var stripeEvent = EventUtility.ConstructEvent(json,
					Request.Headers["Stripe-Signature"], endpointSecret);

				PaymentIntent paymentIntent;
				OrderResultDto order;
				// Handle the event
				if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
				{
					paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
					logger.LogInformation("Payment Failed", paymentIntent.Id);
					order = await paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
					logger.LogInformation("Order Updated To Payment Failed",order.Id);

				}
				else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
				{

					paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
					logger.LogInformation("Payment Succeeded", paymentIntent.Id);
					order = await paymentService.UpdateOrderPaymentSucceded(paymentIntent.Id);
					logger.LogInformation("Order Updated To Payment Succeded", order.Id);
				}
				// ... handle other event types
				else
				{
					Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
				}

				return Ok();
			}
			catch (StripeException e)
			{
				return BadRequest();
			}
		}

	}
}
