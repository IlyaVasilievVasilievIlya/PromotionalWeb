namespace PromoWeb.Api.Configuration
{
    public static class ControllerAndViewsConfiguration
    {
        public static IServiceCollection AddAppControllerAndViews(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddValidator();
            return services;
        }

        public static IEndpointRouteBuilder UseAppControllerAndViews(this IEndpointRouteBuilder app)
        {
            app.MapControllers();
            return app;
        }
    }

    
}
