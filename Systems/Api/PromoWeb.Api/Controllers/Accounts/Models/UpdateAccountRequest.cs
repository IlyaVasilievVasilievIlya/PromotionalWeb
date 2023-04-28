using AutoMapper;
using FluentValidation;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api.Controllers.Accounts.Models
{
	public class UpdateAccountRequest
	{
		/// <summary>
		/// User email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// User full name
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// Has admin role
		/// </summary>
		public bool isAdmin { get; set; }
	}

	public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
	{
		public UpdateAccountRequestValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email is invalid.");
		}
	}

	public class UpdateAccountRequestProfile : Profile
	{
		public UpdateAccountRequestProfile()
		{
			CreateMap<UpdateAccountRequest, UpdateAccountModel>();
		}
	}
}
