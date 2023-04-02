using FluentValidation;

namespace PromoWeb.Web
{
	public class AppInfoModel
	{
		public string TextTitle { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;

		public int SectionId { get; set; }
	}

	public class AppInfoModelValidator : AbstractValidator<AppInfoModel>
	{
		public AppInfoModelValidator()
		{
			RuleFor(x => x.Text)
				.NotEmpty().WithMessage("Text is required.")
				.MaximumLength(5000).WithMessage("Text is too long.");

			RuleFor(x => x.TextTitle)
				.NotEmpty().WithMessage("Text Title is required.")
				.MaximumLength(100).WithMessage("Text Title is too long.");
		}

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AppInfoModel>.CreateWithOptions((AppInfoModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
