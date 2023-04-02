using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Context;
using PromoWeb.Context.Entities;

namespace PromoWeb.Identity.Configuration
{
	public static class SeedUserData
	{
		private static IServiceScope ServiceScope(IServiceProvider serviceProvider) => serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();	
		private static UserManager<User> User(IServiceProvider serviceProvider) => ServiceScope(serviceProvider).ServiceProvider.GetRequiredService<UserManager<User>>();
		private static RoleManager<UserRole> UserRole(IServiceProvider serviceProvider) => ServiceScope(serviceProvider).ServiceProvider.GetRequiredService<RoleManager<UserRole>>();

		public static void Execute(IServiceProvider serviceProvider)
		{
			using var scope = ServiceScope(serviceProvider);
			ArgumentNullException.ThrowIfNull(scope);

			Task.Run(async () =>
			{
				await InitializeAdmin(User(serviceProvider), UserRole(serviceProvider));
			});

		}

			public static async Task InitializeAdmin(UserManager<User> userManager, RoleManager<UserRole> roleManager)
		{

			string adminEmail = "ilyavasilev56@gmail.com";
			string password = "1234";
			if (await roleManager.FindByNameAsync("admin") == null)
			{
				await roleManager.CreateAsync(new UserRole("admin"));
			}
			if (await roleManager.FindByNameAsync("moderator") == null)
			{
				await roleManager.CreateAsync(new UserRole("moderator"));
			}
			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				User admin = new User { Email = adminEmail, UserName = adminEmail, FullName = "unknown" };
				IdentityResult result = await userManager.CreateAsync(admin, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, "admin");
				}
			}
		}
	}
}
