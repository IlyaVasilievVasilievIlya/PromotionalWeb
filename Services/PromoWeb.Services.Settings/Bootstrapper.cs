using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PromoWeb.Services.Settings
{
    public static class Bootstrapper //загрузить в контейнер
    {
        public static IServiceCollection AddMainSettings(this IServiceCollection services, IConfiguration configuration = null)
        {
            var settings = PromoWeb.Settings.Settings.Load<MainSettings>("Main", configuration); //<куда парсим>(ключ для значения json, источник откуда json взять)

            services.AddSingleton(settings);

            return services;
        }

        public static IServiceCollection AddIdentitySettings(this IServiceCollection services, IConfiguration configuration = null)
        {
            var settings = PromoWeb.Settings.Settings.Load<IdentitySettings>("Identity", configuration);

            services.AddSingleton(settings);

            return services;
        }

        public static IServiceCollection AddSwaggerSettings(this IServiceCollection services, IConfiguration configuration = null)
        {
            var settings = PromoWeb.Settings.Settings.Load<SwaggerSettings>("Swagger", configuration);
            services.AddSingleton(settings);

            return services;
        }
    }
}
