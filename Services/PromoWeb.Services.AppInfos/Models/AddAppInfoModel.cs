namespace PromoWeb.Services.AppInfos;

using AutoMapper;
using FluentValidation;
using PromoWeb.Context.Entities;

public class AddAppInfoModel
{
    public string TextTitle { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    public int SectionId { get; set; }
}

public class AddAppInfoModelValidator : AbstractValidator<AddAppInfoModel>
{
    public AddAppInfoModelValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text is required.")
            .MaximumLength(5000).WithMessage("Text is too long.");

        RuleFor(x => x.TextTitle)
            .NotEmpty().WithMessage("Text Title is required.")
            .MaximumLength(100).WithMessage("Text Title is too long.");

		RuleFor(x => x.SectionId)
	        .NotEmpty().WithMessage("Section id is required.");
	}
}

public class AddAppInfoModelProfile : Profile
{
    public AddAppInfoModelProfile()
    {
        CreateMap<AddAppInfoModel, AppInfo>();
    }
}