namespace PromoWeb.Services.Sections;

using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection AddSectionService(this IServiceCollection services)
    {
        services.AddSingleton<ISectionService, SectionService>();

        return services;
    }
}
