namespace PromoWeb.Api.AppInfos;

using AutoMapper;
using FluentValidation;
using PromoWeb.Context.Entities;
using PromoWeb.Services.AppInfos;

public class AddAppInfoRequest
{
    public string TextTitle { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    public int SectionId { get; set; }
}

public class AddAppInfoRequestValidator : AbstractValidator<AddAppInfoRequest>
{
    public AddAppInfoRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text is required.")
            .MaximumLength(5000).WithMessage("Text is too long.");

        RuleFor(x => x.TextTitle)
            .NotEmpty().WithMessage("Text Title is required.")
            .MaximumLength(100).WithMessage("Text Title is too long.");
    }
}

public class AddAppInfoRequestProfile : Profile
{
    public AddAppInfoRequestProfile()
    {
        CreateMap<AddAppInfoRequest, AddAppInfoModel>();
    }
}