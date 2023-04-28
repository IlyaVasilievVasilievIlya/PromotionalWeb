using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Common.Security;
using PromoWeb.Context;
using PromoWeb.Context.Entities;

namespace PromoWeb.Identity.Configuration
{
	public static class SeedUserData
	{
		private static IServiceScope ServiceScope(IServiceProvider serviceProvider) => serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();	
		private static UserManager<User> User(IServiceProvider serviceProvider) => ServiceScope(serviceProvider).ServiceProvider.GetRequiredService<UserManager<User>>();
		private static RoleManager<UserRole> UserRole(IServiceProvider serviceProvider) => ServiceScope(serviceProvider).ServiceProvider.GetRequiredService<RoleManager<UserRole>>();

		public static void Execute(IServiceProvider serviceProvider, AdminSettings settings)
		{
			using var scope = ServiceScope(serviceProvider);
			ArgumentNullException.ThrowIfNull(scope);

			Task.Run(async () =>
			{
				await InitializeAdmin(User(serviceProvider), UserRole(serviceProvider), settings);
			});

		}

			public static async Task InitializeAdmin(UserManager<User> userManager, RoleManager<UserRole> roleManager, AdminSettings settings)
		{
			
			string adminEmail = settings.Email;
			string password = settings.Password;

			if (await roleManager.FindByNameAsync(Roles.Admin) == null)
			{
				await roleManager.CreateAsync(new UserRole(Roles.Admin));
			}
			if (await roleManager.FindByNameAsync(Roles.Moderator) == null)
			{
				await roleManager.CreateAsync(new UserRole(Roles.Moderator));
			}
			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				User admin = new User { Email = adminEmail, UserName = adminEmail, FullName = settings.FullName };
				IdentityResult result = await userManager.CreateAsync(admin, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, Roles.Admin);
				}
			}
		}
	}
}
