using FluentValidation;

namespace PromoWeb.Web
{
	public class SectionModel
	{
		public string SectionName { get; set; } = string.Empty;
	}

	public class SectionModelValidator : AbstractValidator<SectionModel>
	{
		public SectionModelValidator()
		{
			RuleFor(x => x.SectionName)
				.NotEmpty().WithMessage("Section name is required.")
				.MaximumLength(100).WithMessage("Section name is long.")
				;
		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
			{
				var result = await ValidateAsync(ValidationContext<SectionModel>.CreateWithOptions((SectionModel)model, x => x.IncludeProperties(propertyName)));
				if (result.IsValid)
					return Array.Empty<string>();
				return result.Errors.Select(e => e.ErrorMessage);
			};
	}
}
