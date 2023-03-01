using Microsoft.Extensions.DependencyInjection;

namespace PromoWeb.Services.UserAccount
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddUserAccountService(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountService, UserAccountService>(); 
            //scope (для других сервисов singletone) т.к. usermanager работает как scope
            //он внутри сервиса внедряется поэтому нужен scope или transient
            return services;
        }
    }
}
