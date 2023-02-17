
using PromoWeb.Api.Middlewares;

namespace PromoWeb.Api.Configuration
{
    public static class MiddlewaresConfiguration
    {
        public static void UseAppMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionsMiddleware>();
        }
    }
}
