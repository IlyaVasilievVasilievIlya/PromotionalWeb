using PromoWeb.Services.Actions;
using PromoWeb.Services.Answers;
using PromoWeb.Services.AppInfos;
using PromoWeb.Services.Contacts;
using PromoWeb.Services.Images;
using PromoWeb.Services.Links;
using PromoWeb.Services.Questions;
using PromoWeb.Services.RabbitMq;
using PromoWeb.Services.Sections;
using PromoWeb.Services.Settings;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api
{
    public static class Bootstrapper
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            services
                .AddMainSettings()
                .AddSwaggerSettings()
                .AddIdentitySettings()
                .AddQuestionService()
                .AddAnswerService()
                .AddContactService()
                .AddSectionService()
                .AddAppInfoService()
                .AddImageService()
                .AddLinkService()
                .AddUserAccountService()
                .AddRabbitMq()
                .AddActions()
                ;

            return services;
        }
    }
}
