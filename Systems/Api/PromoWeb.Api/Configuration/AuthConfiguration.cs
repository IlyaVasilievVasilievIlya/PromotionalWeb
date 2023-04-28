using Microsoft.AspNetCore.Identity;
using PromoWeb.Common.Security;
using PromoWeb.Context.Entities;
using PromoWeb.Context;
using PromoWeb.Services.Settings;
using IdentityServer4.AccessTokenValidation;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;

namespace PromoWeb.Api.Configuration
{
    public static class AuthConfiguration
    {
        public static IServiceCollection AddAppAuth(this IServiceCollection services, IdentitySettings settings)
        {
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
                .AddUserManager<UserManager<User>>()
                .AddRoleManager<RoleManager<UserRole>>()
                .AddDefaultTokenProviders();

			

			services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = settings.Url.StartsWith("https://");
                    options.Authority = settings.Url;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Audience = "api";
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppScopes.AppApi, policy => policy.RequireClaim("scope", AppScopes.AppApi));
                options.AddPolicy(Roles.Admin,  policy => policy.RequireClaim("role", Roles.Admin));
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
