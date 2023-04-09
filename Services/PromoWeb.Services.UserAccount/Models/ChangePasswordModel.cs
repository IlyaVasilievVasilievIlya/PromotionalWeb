using FluentValidation;

namespace PromoWeb.Services.UserAccount
{
	public class ChangePasswordModel
	{
		//id
		public string Email { get; set; }
		public string NewPassword { get; set; }
		public string OldPassword { get; set; }
	}

	public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
	{
		public ChangePasswordModelValidator()
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
}
