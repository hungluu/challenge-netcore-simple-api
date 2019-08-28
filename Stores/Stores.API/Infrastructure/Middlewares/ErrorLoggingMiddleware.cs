using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace Stores.API.Infrastructure.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                    title = "Internal Server Error"
                }), Encoding.UTF8);


                await _next.Invoke(context);
            }
        }
    }
}
