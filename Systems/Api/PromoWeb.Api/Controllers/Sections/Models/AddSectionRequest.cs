namespace PromoWeb.Api.Sections;

using AutoMapper;
using FluentValidation;
using PromoWeb.Services.Sections;

public class AddSectionRequest
{
    public string SectionName { get; set; } = string.Empty;
}

public class AddSectionRequestValidator : AbstractValidator<AddSectionRequest>
{
    public AddSectionRequestValidator()
    {
        RuleFor(x => x.SectionName)
            .NotEmpty().WithMessage("Section name is required.")
            .MaximumLength(100).WithMessage("Section name is long.");
    }
}

public class AddSectionRequestProfile : Profile
{
    public AddSectionRequestProfile()
    {
        CreateMap<AddSectionRequest, AddSectionModel>();
    }
}