namespace PromoWeb.Api.Sections;

using AutoMapper;
using FluentValidation;
using PromoWeb.Services.Sections;

public class UpdateSectionRequest
{
    public string SectionName { get; set; } = string.Empty;
}

public class UpdateSectionRequestValidator : AbstractValidator<UpdateSectionRequest>
{
    public UpdateSectionRequestValidator()
    {
        RuleFor(x => x.SectionName)
            .NotEmpty().WithMessage("Section name is required.")
            .MaximumLength(100).WithMessage("Section name is long.");
    }
}

public class UpdateSectionRequestProfile : Profile
{
    public UpdateSectionRequestProfile()
    {
        CreateMap<UpdateSectionRequest, UpdateSectionModel>();
    }
}