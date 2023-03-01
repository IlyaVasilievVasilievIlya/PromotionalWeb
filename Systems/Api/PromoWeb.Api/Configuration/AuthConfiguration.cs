using Microsoft.AspNetCore.Identity;
using PromoWeb.Common.Security;
using PromoWeb.Context.Entities;
using PromoWeb.Context;
using PromoWeb.Services.Settings;
using IdentityServer4.AccessTokenValidation;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;

namespace PromoWeb.Api.Configuration
{
    public static class AuthConfiguration
    {
        public static IServiceCollection AddAppAuth(this IServiceCollection services, IdentitySettings settings)
        {
        //https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html
            IdentityModelEventSource.ShowPII = true;

            services
                .AddIdentity<User, UserRole>(opt =>
                {
                    opt.Password.RequiredLength = 0;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<MainDbContext>()
                .AddUserManager<UserManager<User>>() //di для user сервиса
                .AddRoleManager<RoleManager<UserRole>>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme; //строка "bearer" везде из is4
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>   //получение токена для обработки
                {
                    options.RequireHttpsMetadata = settings.Url.StartsWith("https://"); //поиск в url настройке по началу значениям (надо ли https)
                    options.Authority = settings.Url;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        ValidateIssuer = false, //если издатель не валидируется те не нужно подпись проверять?
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        //RoleClaimType = JwtClaimTypes.Role,
                        //NameClaimType = JwtClaimTypes.Name
                    };
                    options.Audience = "api";
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppScopes.AppInfoRead, policy => policy.RequireClaim("scope", AppScopes.AppInfoRead)); //у клиента в scope заголовке должна быть эта строка
                options.AddPolicy(AppScopes.AppInfoWrite, policy => policy.RequireClaim("scope", AppScopes.AppInfoWrite));
                options.AddPolicy(AppScopes.SectionRead, policy => policy.RequireClaim("scope", AppScopes.SectionRead));
                options.AddPolicy(AppScopes.SectionWrite, policy => policy.RequireClaim("scope", AppScopes.SectionWrite));
                options.AddPolicy(AppScopes.LinkRead, policy => policy.RequireClaim("scope", AppScopes.LinkRead));
                options.AddPolicy(AppScopes.LinkWrite, policy => policy.RequireClaim("scope", AppScopes.LinkWrite));
                options.AddPolicy(AppScopes.ImageRead, policy => policy.RequireClaim("scope", AppScopes.ImageRead));
                options.AddPolicy(AppScopes.ImageWrite, policy => policy.RequireClaim("scope", AppScopes.ImageWrite));
                options.AddPolicy(AppScopes.QuestionRead, policy => policy.RequireClaim("scope", AppScopes.QuestionRead));
                options.AddPolicy(AppScopes.QuestionWrite, policy => policy.RequireClaim("scope", AppScopes.QuestionWrite));
                options.AddPolicy(AppScopes.AnswerRead, policy => policy.RequireClaim("scope", AppScopes.AnswerRead));
                options.AddPolicy(AppScopes.AnswerWrite, policy => policy.RequireClaim("scope", AppScopes.AnswerWrite));
                options.AddPolicy(AppScopes.ContactRead, policy => policy.RequireClaim("scope", AppScopes.ContactRead));
                options.AddPolicy(AppScopes.ContactWrite, policy => policy.RequireClaim("scope", AppScopes.ContactWrite));
                options.AddPolicy("admin",  policy => policy.RequireClaim("role", "admin"));//не видит этот клейм
            });

            return services;
        }

        public static IApplicationBuilder UseAppAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}
