using Microsoft.Extensions.DependencyInjection;

namespace PromoWeb.Services.AppInfos
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddAppInfoService(this IServiceCollection services)
        {
            services.AddSingleton<IAppInfoService, AppInfoService>();

            return services;
        }
    }
}
