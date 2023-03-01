using Microsoft.AspNetCore.Identity;
using PromoWeb.Context.Entities;
using PromoWeb.Context;

namespace PromoWeb.Identity.Configuration
{
    public static class IS4Configuration
    {
        public static IServiceCollection AddIS4(this IServiceCollection services)
        {
            services //addidentity добавить элемент по которому будет идентифицироваться пользователь
                .AddIdentity<User, UserRole>(opt => //юзеру какому то какую то роль (таблица раскрытия многих ко многим (регистрация di для 
                {
                    opt.Password.RequiredLength = 0;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<MainDbContext>() //хранение будет в бд
                .AddUserManager<UserManager<User>>() //класс для работы с ними (как то связыватеся с контекстом базы (те уже доступен в di)
                .AddDefaultTokenProviders()
                ;

            services
                .AddIdentityServer() //это все уже от duende

                .AddAspNetIdentity<User>() //This is needed when IdentityServer must add claims for the users into tokens.

                .AddInMemoryApiScopes(AppApiScopes.ApiScopes) //подгружаем скопы
                .AddInMemoryClients(AppClients.Clients) //и клиентов

                .AddInMemoryApiResources(AppResources.Resources) //для сваггера доступ к API (apiresource это просто сборик скопов?
                .AddInMemoryIdentityResources(AppIdentityResources.Resources) //подгружаем сведения о юзере (identity token)

                // .AddTestUsers(AppApiTestUsers.ApiUsers) //юзеры в памяти

                .AddDeveloperSigningCredential();

            return services;
        }

        public static IApplicationBuilder UseIS4(this IApplicationBuilder app)
        {
            app.UseIdentityServer();

            return app;
        }
    }
}
