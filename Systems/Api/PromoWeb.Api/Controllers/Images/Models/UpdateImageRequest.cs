namespace PromoWeb.Api.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using PromoWeb.Services.Images;
using PromoWeb.Common.Extensions;

public class UpdateImageRequest
{
    public string ImageName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    public int AppInfoId { get; set; }
    public IFormFile Image { get; set; }
}

public class UpdateImageRequestValidator : AbstractValidator<UpdateImageRequest>
{
    public UpdateImageRequestValidator()
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
            .WithMessage("Only images allowed");
    }
}

public class UpdateImageRequestProfile : Profile
{
    public UpdateImageRequestProfile()
    {
        CreateMap<UpdateImageRequest, UpdateImageModel>();
    }
}