using Microsoft.AspNetCore.Identity;
using PromoWeb.Context.Entities;
using PromoWeb.Identity.Configuration;

namespace PromoWeb.Identity
{
    public static class Bootstrapper
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
