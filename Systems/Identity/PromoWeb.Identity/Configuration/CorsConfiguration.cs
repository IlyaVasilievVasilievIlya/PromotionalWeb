using Duende.IdentityServer.Services;

namespace PromoWeb.Identity.Configuration
{
    /// <summary>
    /// CORS configuration
    /// </summary>
    public static class CorsConfiguration
    {
        //на наш api приходит приложение мы отдаем скрипт с редиректом сюда?
        /// <summary>
        /// Add CORS
        /// </summary>
        /// <param name="services">Services collection</param>
        public static IServiceCollection AddAppCors(this IServiceCollection services)
        {
            //Icorspolicy маппится с ответом лямбда-функции (вернет defaultcorspolicy
            services.AddSingleton<ICorsPolicyService>((container) =>
            {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();

                return new DefaultCorsPolicyService(logger)
                {
                    AllowAll = true
                };
            });
            return services;
        }

        /// <summary>
        /// Use service
        /// </summary>
        /// <param name="app">Application</param>
        public static void UseAppCors(this IApplicationBuilder app)
        {
            //app.UseCors();
        }
    }
}
