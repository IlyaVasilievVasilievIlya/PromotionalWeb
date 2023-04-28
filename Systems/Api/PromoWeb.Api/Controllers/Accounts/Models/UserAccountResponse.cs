using AutoMapper;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api.Controllers.Accounts.Models
{
    public class UserAccountResponse
    {
        /// <summary>
        /// User Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User role
        /// </summary>
		public string Role { get; set; }
	}

    public class UserAccountResponseProfile : Profile
    {
        public UserAccountResponseProfile()
        {
            CreateMap<UserAccountModel, UserAccountResponse>();
        }
    }
}
