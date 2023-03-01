using Duende.IdentityServer.Models;
using IdentityModel;

namespace PromoWeb.Identity.Configuration
{
    public static class AppIdentityResources //identityresource - данные юзера (не api)
    {
        public static IEnumerable<IdentityResource> Resources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }
}

