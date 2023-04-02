using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;

namespace PromoWeb.Web
{
	public class ImageModel
	{
		public string ImageName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public int AppInfoId { get; set; }
		public IBrowserFile Image { get; set; } 
	}

	public class ImageModelValidator : AbstractValidator<ImageModel>
	{
		public ImageModelValidator()
		{
			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required.")
				.MaximumLength(100).WithMessage("Description is long.");

			RuleFor(x => x.ImageName)
				.NotEmpty().WithMessage("Image name is required.")
				.MaximumLength(100).WithMessage("Image name is long.");

			RuleFor(x => x.Image)
				.NotEmpty().WithMessage("Image is required");

			RuleFor(x => x.Image.Size)
				.NotEmpty()
				.When(x => x.Image is not null)
				.WithMessage("Image content required");

			RuleFor(x => x.Image.ContentType)
				.Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
				.When(x => x.Image is not null)
				.WithMessage("Only images allowed");
		}

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<ImageModel>.CreateWithOptions((ImageModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
