namespace PromoWeb.Services.Images;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;

public class AddImageModel
{
    public string Description { get; set; } = string.Empty;

    public int AppInfoId { get; set; }
    public byte[] Bytes { get; set; }
    public string FileExtension { get; set; } = string.Empty;
}

public class AddImageModelValidator : AbstractValidator<AddImageModel>
{
    public AddImageModelValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description is long.");

        RuleFor(x => x.FileExtension)
            .NotEmpty().WithMessage("Extension is required.")
            .MaximumLength(6).WithMessage("Extension field is long.");
    }
}

public class AddImageModelProfile : Profile
{
    public AddImageModelProfile()
    {
        CreateMap<AddImageModel, Image>();
    }
}