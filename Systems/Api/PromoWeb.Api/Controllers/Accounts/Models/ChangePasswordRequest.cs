using AutoMapper;
using FluentValidation;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api.Controllers.Accounts.Models
{
	public class ChangePasswordRequest
	{
		/// <summary>
		/// Email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// New password
		/// </summary>
		public string NewPassword { get; set; }

		/// <summary>
		/// Old password
		/// </summary>
		public string OldPassword { get; set; }
	}

	public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
	{
		public ChangePasswordRequestValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email is invalid.");

			RuleFor(x => x.OldPassword)
				.NotEmpty().WithMessage("Password is required.")
				.MaximumLength(50).WithMessage("Password is long.");

			RuleFor(x => x.NewPassword)
				.NotEmpty().WithMessage("Password is required.")
				.MaximumLength(50).WithMessage("Password is long.");
		}
	}

	public class ChangePasswordRequestProfile : Profile
	{
		public ChangePasswordRequestProfile()
		{
			CreateMap<ChangePasswordRequest, ChangePasswordModel>();
		}
	}
}
