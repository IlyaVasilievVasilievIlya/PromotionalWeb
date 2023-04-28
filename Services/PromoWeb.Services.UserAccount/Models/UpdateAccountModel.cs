using FluentValidation;

namespace PromoWeb.Services.UserAccount
{
	public class UpdateAccountModel
	{
		public string Email { get; set; }
		public string FullName { get; set; }
		public bool isAdmin { get; set; }
	}

	public class UpdateAccountModelValidator : AbstractValidator<UpdateAccountModel>
	{
		public UpdateAccountModelValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Email is invalid.");
		}
	}
}
