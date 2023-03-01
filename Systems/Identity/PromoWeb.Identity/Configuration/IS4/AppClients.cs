using PromoWeb.Common.Security;
using Duende.IdentityServer.Models;

namespace PromoWeb.Identity.Configuration
{
    public static class AppClients
    {
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "swagger",
                    ClientSecrets =
                    {
                        new Secret("B3AT_HaPp3ZZIng".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ClientCredentials, //нет юзера за клиентом

                    AccessTokenLifetime = 3600, //1 hour
                    AllowedScopes =
                    {
                        AppScopes.AppInfoRead,
                        AppScopes.AppInfoWrite,
                        AppScopes.SectionRead, 
                        AppScopes.SectionWrite,
                        AppScopes.LinkRead,
                        AppScopes.LinkWrite,
                        AppScopes.ImageRead,
                        AppScopes.ImageWrite,
                        AppScopes.QuestionRead,
                        AppScopes.QuestionWrite,
                        AppScopes.AnswerRead,
                        AppScopes.AnswerWrite, 
                        AppScopes.ContactRead, 
                        AppScopes.ContactWrite,
                        "roles"
                    }
                }
                ,
                new Client
                {
                    ClientId = "frontend",
                    ClientSecrets =
                    {
                        new Secret("B3AT_HaPp3ZZIng".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowOfflineAccess = true,
                    AccessTokenType = AccessTokenType.Jwt,

                    AccessTokenLifetime = 3600, // 1 hour

                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AbsoluteRefreshTokenLifetime = 2592000, // 30 days
                    SlidingRefreshTokenLifetime = 1296000, // 15 days

                    AllowedScopes = {
                        AppScopes.AppInfoRead,
                        AppScopes.AppInfoWrite,
                        AppScopes.SectionRead,
                        AppScopes.SectionWrite,
                        AppScopes.LinkRead,
                        AppScopes.LinkWrite,
                        AppScopes.ImageRead,
                        AppScopes.ImageWrite,
                        AppScopes.QuestionRead,
                        AppScopes.QuestionWrite,
                        AppScopes.AnswerRead,
                        AppScopes.AnswerWrite,
                        AppScopes.ContactRead,
                        AppScopes.ContactWrite,
                        "roles"
                    }
                }
            };
    }
}
