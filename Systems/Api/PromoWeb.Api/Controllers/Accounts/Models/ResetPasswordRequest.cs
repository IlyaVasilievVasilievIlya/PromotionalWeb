using AutoMapper;
using FluentValidation;
using PromoWeb.Services.UserAccount;

namespace PromoWeb.Api.Controllers.Accounts.Models
{
	public class ResetPasswordRequest
	{
		/// <summary>
		/// User email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// New password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Password recovery secret code
		/// </summary>
		public string Code { get; set; }
	}

	public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email is invalid.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MaximumLength(50).WithMessage("Password is long.");
		}
	}

	public class ResetPasswordRequestProfile : Profile
	{
		public ResetPasswordRequestProfile()
		{
			CreateMap<ResetPasswordRequest, ResetPasswordModel>();
		}
	}
}
