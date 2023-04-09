using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PromoWeb.Services.UserAccount
{
	public class ResetPasswordModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Code { get; set; }
	}

	public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
	{
		public ResetPasswordModelValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email is invalid.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MaximumLength(50).WithMessage("Password is long.");
		}
	}
}
