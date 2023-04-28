namespace PromoWeb.Api.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using PromoWeb.Services.Images;
using PromoWeb.Common.Extensions;

public class AddImageRequest
{
    /// <summary>
    /// Image name
    /// </summary>
    public string ImageName { get; set; } = string.Empty;

    /// <summary>
    /// Image description
    /// </summary>
    public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Info block that the photo will belong to
	/// </summary>
	public int AppInfoId { get; set; }

	/// <summary>
	/// Upload image file
	/// </summary>
	public IFormFile Image { get; set; }
}

public class AddImageRequestValidator : AbstractValidator<AddImageRequest>
{
    public AddImageRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description is long.");

        RuleFor(x => x.ImageName)
            .NotEmpty().WithMessage("Image name is required.")
            .MaximumLength(100).WithMessage("Image name is long.");

        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Image is required");

        RuleFor(x => x.Image.Length)
            .NotEmpty()
            .When(x => x.Image is not null)
            .WithMessage("Image content required");

        RuleFor(x => x.Image.ContentType)
            .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
            .When(x => x.Image is not null)
            .WithMessage("Only images allowed");

		RuleFor(x => x.AppInfoId)
	        .NotEmpty().WithMessage("AppInfo id is required.");
	}
}

public class AddImageRequestProfile : Profile
{
    public AddImageRequestProfile()
    {
        CreateMap<AddImageRequest, AddImageModel>();
    }
}