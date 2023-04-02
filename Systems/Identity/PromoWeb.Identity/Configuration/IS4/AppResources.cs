using Duende.IdentityServer.Models;
using IdentityModel;
using PromoWeb.Common.Security;

namespace PromoWeb.Identity.Configuration
{
    public static class AppResources
    {
        //что присоединяется к скопам запрошенным
        public static IEnumerable<ApiResource> Resources => new List<ApiResource>
        {
            new ApiResource("role", "role", new string[] { JwtClaimTypes.Role })
            {
                Scopes = 
                {
					AppScopes.UsersApi
				}
            },
        };
    }
}
