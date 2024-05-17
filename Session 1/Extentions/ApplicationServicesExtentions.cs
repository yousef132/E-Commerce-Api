using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.Services.ProductService.Dtos;
using Store.Service.Services.ProductService;
using Store.Service.HandleResponses;
using Store.Service.Services.CacheService;
using Store.Repository.BasketRepository;
using Store.Service.Services.BasketServices.Dtos;
using Store.Service.Services.BasketServices;
using Store.Service.Services.TokenService;
using Store.Service.Services.UserService.UserService;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using Store.Service.Services.OrderService;

namespace Session_1.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services )
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<ICacheService, CacheService>();
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddScoped<IBasketService, BasketService>();
            Services.AddScoped<ITokenServices, TokenServices>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped<IOrderService, OrderService>();

            Services.AddAutoMapper(typeof(ProductProfile));
            Services.AddAutoMapper(typeof(BasketProfiler));
            Services.AddAutoMapper(typeof(OrderProfile));
            Services.Configure<ApiBehaviorOptions>(options =>
            { 
                options.InvalidModelStateResponseFactory = actioncontext =>
                {
                    var errors = actioncontext.ModelState.Where(model => model.Value.Errors.Count > 0)
                    .SelectMany(model => model.Value.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return Services;
        }
    }
}
