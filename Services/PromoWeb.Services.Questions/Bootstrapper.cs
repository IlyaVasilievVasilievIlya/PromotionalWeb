using Microsoft.Extensions.DependencyInjection;

namespace PromoWeb.Services.Questions
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddQuestionService(this IServiceCollection services)
        {
            services.AddSingleton<IQuestionService, QuestionService>();

            return services;
        }
    }
}
