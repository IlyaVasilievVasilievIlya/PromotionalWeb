using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PromoWeb.Settings;

namespace PromoWeb.Services.EmailSender
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration = null)
        {

            var settings = Settings.Settings.Load<EmailSettings>("Email", configuration);
            services.AddSingleton(settings);

            services.AddSingleton<IEmailSender, EmailSender>();

            return services;
        }
    }
}
