namespace PromoWeb.Services.Images;

using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection AddImageService(this IServiceCollection services)
    {
        services.AddSingleton<IImageService, ImageService>();

        return services;
    }
}
