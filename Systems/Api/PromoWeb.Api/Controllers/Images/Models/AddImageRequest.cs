namespace PromoWeb.Api.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using PromoWeb.Services.Images;

public class AddImageRequest
{
    public string Description { get; set; } = string.Empty;

    public int AppInfoId { get; set; }
    public byte[] Bytes { get; set; }
    public string FileExtension { get; set; } = string.Empty;
}

public class AddImageRequestValidator : AbstractValidator<AddImageRequest>
{
    public AddImageRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description is long.");

        RuleFor(x => x.FileExtension)
            .NotEmpty().WithMessage("Extension is required.")
            .MaximumLength(6).WithMessage("Extension field is long.");
    }
}

public class AddImageRequestProfile : Profile
{
    public AddImageRequestProfile()
    {
        CreateMap<AddImageRequest, AddImageModel>();
    }
}