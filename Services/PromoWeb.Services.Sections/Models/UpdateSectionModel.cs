namespace PromoWeb.Services.Sections;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;

public class UpdateSectionModel
{
    public string SectionName { get; set; } = string.Empty;
}

public class UpdateSectionModelValidator : AbstractValidator<UpdateSectionModel>
{
    public UpdateSectionModelValidator()
    {
        RuleFor(x => x.SectionName)
            .NotEmpty().WithMessage("Section name is required.")
            .MaximumLength(100).WithMessage("Section name is long.");
    }
}

public class UpdateSectionModelProfile : Profile
{
    public UpdateSectionModelProfile()
    {
        CreateMap<UpdateSectionModel, Section>();
    }
}