using FluentValidation;

namespace PromoWeb.Web
{
	public class QuestionModel
	{
		public string Text { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
	}

	public class QuestionModelValidator : AbstractValidator<QuestionModel>
	{
		public QuestionModelValidator()
		{
			RuleFor(x => x.Text)
				.NotEmpty().WithMessage("Question text is required.")
				.MaximumLength(500).WithMessage("Text is too long.");

			RuleFor(x => x.Email)
				.MaximumLength(100).WithMessage("Email is too long.")
				.EmailAddress()
				.When(x => !string.IsNullOrEmpty(x.Email))
				.WithMessage("Invalid email");
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<QuestionModel>.CreateWithOptions((QuestionModel)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
