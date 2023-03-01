using PromoWeb.Services.EmailSender;
using PromoWeb.Services.RabbitMq;

namespace PromoWeb.Worker
{
    public static class Bootstrapper
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            services
                .AddRabbitMq()
                .AddEmailSender()
                ;

            services.AddSingleton<ITaskExecutor, TaskExecutor>();

            return services;
        }
    }
}
