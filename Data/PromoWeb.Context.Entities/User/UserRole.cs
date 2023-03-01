using Microsoft.AspNetCore.Identity;

namespace PromoWeb.Context.Entities
{
    public class UserRole : IdentityRole<Guid>
    {
        public UserRole() { }
        public UserRole(string roleName) : base(roleName) { }
    }
}
