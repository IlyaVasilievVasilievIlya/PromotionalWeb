namespace PromoWeb.Services.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Context;

public class AddImageModel
{
    public string ImageName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int AppInfoId { get; set; }
	public string UniqueName { get; set; }
}

public class AddImageModelValidator : AbstractValidator<AddImageModel>
{
    public AddImageModelValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description is long.");

        RuleFor(x => x.ImageName)
            .NotEmpty().WithMessage("Image name is required.")
            .MaximumLength(100).WithMessage("Image name is long.");

        RuleFor(x => x.UniqueName)
            .NotEmpty().WithMessage("Image filename is required");

		RuleFor(x => x.AppInfoId)
	        .NotEmpty().WithMessage("AppInfo id is required.");
	}
}

public class AddImageModelProfile : Profile
{
    public AddImageModelProfile()
    {
        CreateMap<AddImageModel, Image>();
    }
}