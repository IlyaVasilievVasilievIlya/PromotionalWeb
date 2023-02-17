using PromoWeb.Common.Helpers;

namespace PromoWeb.Api.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddAppAutoMappers(this IServiceCollection services)
        {
            AutoMapperHelper.Register(services);
            return services;
        }
    }
}
