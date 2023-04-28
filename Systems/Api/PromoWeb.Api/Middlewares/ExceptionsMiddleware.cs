using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Extensions;
using PromoWeb.Common.Responses;
using System.Text.Json;

namespace PromoWeb.Api.Middlewares
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionsMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ErrorResponse response = null;
            try
            {
                await next.Invoke(context);
            }
            catch (ProcessException pe)
            {
                response = pe.ToErrorResponse();
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
            catch (Exception pe)
            {
                response = pe.ToErrorResponse();
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			}
            finally
            {
                if (response is not null)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    await context.Response.StartAsync();
                    await context.Response.CompleteAsync();
                }
            }
        }
    }
}
