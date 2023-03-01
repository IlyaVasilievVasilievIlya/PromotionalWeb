using PromoWeb.Api.Configuration.HealthChecks;
using PromoWeb.Common;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace PromoWeb.Api.Configuration
{
    public static class HealthCheckConfiguration
    {
        public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<SelfHealthCheck>("PromoWeb.API");

            return services;
        }

        public static void UseAppHealthChecks(this WebApplication app)
        {
            app.MapHealthChecks("/health");

            app.MapHealthChecks("/health/detail", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckHelper.WriteHealthCheckResponse,
                AllowCachingResponses = false,
            });
        }
    }   
}
