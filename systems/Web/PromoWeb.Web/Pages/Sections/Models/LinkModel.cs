using FluentValidation;

namespace PromoWeb.Web
{
	public class LinkModel
    {

		public string LinkText { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public int SectionId { get; set; }
	}

	public class LinkModelValidator : AbstractValidator<LinkModel>
	{
		public LinkModelValidator()
		{
			RuleFor(x => x.SectionId)
				.NotEmpty().WithMessage("Section is required.");

			RuleFor(x => x.LinkText)
				.NotEmpty().WithMessage("Link text is required.")
				.MaximumLength(500).WithMessage("Title is long.");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required.")
				.MaximumLength(1000).WithMessage("Description is long.");
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<LinkModel>.CreateWithOptions((LinkModel)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
