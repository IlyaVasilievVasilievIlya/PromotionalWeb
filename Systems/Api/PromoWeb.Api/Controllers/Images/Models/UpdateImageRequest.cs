namespace PromoWeb.Api.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using PromoWeb.Services.Images;

public class UpdateImageRequest
{
    public string Description { get; set; } = string.Empty;

    public int AppInfoId { get; set; }
    public byte[] Bytes { get; set; }
    public string FileExtension { get; set; } = string.Empty;
}

public class UpdateImageRequestValidator : AbstractValidator<UpdateImageRequest>
{
    public UpdateImageRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description is long.");

        RuleFor(x => x.FileExtension)
            .NotEmpty().WithMessage("Extension is required.")
            .MaximumLength(6).WithMessage("Extension field is long.");
    }
}

public class UpdateImageRequestProfile : Profile
{
    public UpdateImageRequestProfile()
    {
        CreateMap<UpdateImageRequest, UpdateImageModel>();
    }
}