using PromoWeb.Common.Security;

using Duende.IdentityServer.Models;
using IdentityModel;

namespace PromoWeb.Identity.Configuration
{
    public static class AppApiScopes
    {
        //что запрашивается (какие права просит клиент)
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(AppScopes.AppInfoRead, "Access to appInfo API - Read data", userClaims: new [] {JwtClaimTypes.Role}),// 1й параметр обычные строчки
                new ApiScope(AppScopes.AppInfoWrite, "Access to appInfo API - Write data", userClaims: new [] {JwtClaimTypes.Role}), //2q просто псевдоним

                new ApiScope(AppScopes.SectionRead, "Access to section API - Read data", userClaims: new [] {JwtClaimTypes.Role}),
                new ApiScope(AppScopes.SectionWrite, "Access to section API - Write data", userClaims: new [] {JwtClaimTypes.Role}),

                new ApiScope(AppScopes.LinkRead, "Access to link API - Read data", userClaims: new [] {JwtClaimTypes.Role}),
                new ApiScope(AppScopes.LinkWrite, "Access to link API - Write data", userClaims: new [] {JwtClaimTypes.Role}),

                new ApiScope(AppScopes.ImageRead, "Access to image API - Read data", userClaims: new [] {JwtClaimTypes.Role}),
                new ApiScope(AppScopes.ImageWrite, "Access to image API - Write data", userClaims: new [] {JwtClaimTypes.Role}),

                new ApiScope(AppScopes.QuestionRead, "Access to question API - Read data", userClaims: new [] {JwtClaimTypes.Role}),
                new ApiScope(AppScopes.QuestionWrite, "Access to question API - Write data", userClaims: new [] {JwtClaimTypes.Role}),

                new ApiScope(AppScopes.AnswerRead, "Access to answer API - Read data", userClaims: new [] {JwtClaimTypes.Role}),
                new ApiScope(AppScopes.AnswerWrite, "Access to answer API - Write data", userClaims: new [] {JwtClaimTypes.Role}),

                new ApiScope(AppScopes.ContactRead, "Access to contact API - Read data", userClaims: new [] {JwtClaimTypes.Role}),
                new ApiScope(AppScopes.ContactWrite, "Access to contact API - Write data", userClaims: new [] {JwtClaimTypes.Role})
            };
    }
}
