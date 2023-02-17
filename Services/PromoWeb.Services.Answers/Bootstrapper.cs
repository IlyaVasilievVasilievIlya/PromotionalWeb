using Microsoft.Extensions.DependencyInjection;

namespace PromoWeb.Services.Answers
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddAnswerService(this IServiceCollection services)
        {
            services.AddSingleton<IAnswerService, AnswerService>();

            return services;
        }
    }
}
