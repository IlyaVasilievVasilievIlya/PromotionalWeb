using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PromoWeb.Context.Entities;
using System.Security.Claims;

namespace PromoWeb.Identity.Configuration
{
    /*public class ClaimsFactory : UserClaimsPrincipalFactory<User>

    {
        private readonly UserManager<User> _userManager;

        public ClaimsFactory(
            UserManager<User> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            identity.AddClaims(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));

            return identity;
        }
    }*/
}
