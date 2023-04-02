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
				new ApiScope(AppScopes.UsersApi),
				new ApiScope(AppScopes.AppApi)
			};
	}
}
