using FluentValidation;

namespace PromoWeb.Web
{
	public class ContactModel
	{
		public string ContactOwner { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string WebSite { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
	}

	public class ContactModelValidator : AbstractValidator<ContactModel>
	{
		public ContactModelValidator()
		{
			RuleFor(x => x.ContactOwner)
				.NotEmpty().WithMessage("Contact owner is required.")
				.MaximumLength(50).WithMessage("ContactOwner name is long.");

			RuleFor(x => x.Email)
				.MaximumLength(100).WithMessage("Email is long.")
				.EmailAddress()
				.When(x => !string.IsNullOrEmpty(x.Email))
				.WithMessage("Invalid email");

			RuleFor(x => x.WebSite)
				.MaximumLength(200).WithMessage("WebSite is long.");
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<ContactModel>.CreateWithOptions((ContactModel)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
