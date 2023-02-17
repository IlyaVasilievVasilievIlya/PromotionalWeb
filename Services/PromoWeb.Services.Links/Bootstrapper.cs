namespace PromoWeb.Services.Links;

using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection AddLinkService(this IServiceCollection services)
    {
        services.AddSingleton<ILinkService, LinkService>();

        return services;
    }
}
