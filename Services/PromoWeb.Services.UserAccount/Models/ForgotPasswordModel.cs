using FluentValidation;

namespace PromoWeb.Services.UserAccount
{
	public class ForgotPasswordModel
	{
		public string Email { get; set; }
	}

	public class ForgotPasswordModelValidator : AbstractValidator<ForgotPasswordModel>
	{
		public ForgotPasswordModelValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email is invalid.");
		}
	}
}
