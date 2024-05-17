﻿using Store.Service.HandleResponses;
using System.Net;
using System.Text.Json;

namespace Session_1.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostEnvironment environment;
        private readonly ILogger logger;

        public ExceptionMiddleware(RequestDelegate next ,
            IHostEnvironment environment,
            ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.environment = environment;
            this.logger = logger;
        }
        // called in runtime in pipline
        // Get Current Request 
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = environment.IsDevelopment() ? new CustomException
                    ((int)HttpStatusCode.InternalServerError,
                    ex.Message,
                    ex.StackTrace)
                    : new CustomException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json  = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }

    }
}
