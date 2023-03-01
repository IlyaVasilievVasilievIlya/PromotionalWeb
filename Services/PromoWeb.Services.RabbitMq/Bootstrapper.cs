using PromoWeb.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PromoWeb.Services.RabbitMq
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration = null)
        {
            var settings = Settings.Settings.Load<RabbitMqSettings>("RabbitMq", configuration);
            services.AddSingleton(settings);

            services.AddSingleton<IRabbitMq, RabbitMq>();

            return services;
        }
    }
}
