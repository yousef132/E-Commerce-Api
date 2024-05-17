using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Session_1.Extentions;
using Session_1.Helper;
using Session_1.Middlewares;
using StackExchange.Redis;
using Store.Data.StoreDbContext;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.HandleResponses;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;

namespace Session_1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);


            builder.Services.AddDbContext<StoreDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<StoreIdentityDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
            });
            // singleton so that object is Available Throw Runtime
            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configs = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(configs);
            });

            builder.Services.AddSwaggerDocumentation();
            var app = builder.Build();

            await ApplySeeding.ApplySeedingAsync(app);
            // Configure the HTTP request pipeline.
             if (app.Environment.IsDevelopment())
             {
                app.UseSwagger();
                app.UseSwaggerUI();
             }


            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
