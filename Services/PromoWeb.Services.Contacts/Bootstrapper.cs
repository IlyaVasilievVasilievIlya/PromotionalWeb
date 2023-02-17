namespace PromoWeb.Services.Contacts;

using Microsoft.Extensions.DependencyInjection;

public static class Bootstrapper
{
    public static IServiceCollection AddContactService(this IServiceCollection services)
    {
        services.AddSingleton<IContactService, ContactService>();

        return services;
    }
}
