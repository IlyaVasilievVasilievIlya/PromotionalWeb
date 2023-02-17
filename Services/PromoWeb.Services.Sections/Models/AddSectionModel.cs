namespace PromoWeb.Services.Sections;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;

public class AddSectionModel
{
    public string SectionName { get; set; } = string.Empty;
}

public class AddSectionModelValidator : AbstractValidator<AddSectionModel>
{
    public AddSectionModelValidator()
    {
        RuleFor(x => x.SectionName)
            .NotEmpty().WithMessage("Section name is required.")
            .MaximumLength(100).WithMessage("Section name is long.");
    }
}

public class AddSectionModelProfile : Profile
{
    public AddSectionModelProfile()
    {
        CreateMap<AddSectionModel, Section>();
    }
}