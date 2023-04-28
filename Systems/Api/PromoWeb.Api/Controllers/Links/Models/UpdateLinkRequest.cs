namespace PromoWeb.Api.Links;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using PromoWeb.Services.Links;

public class UpdateLinkRequest
{
	/// <summary>
	/// Link
	/// </summary>
	public string LinkText { get; set; } = string.Empty;

	/// <summary>
	/// Link description
	/// </summary>
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Section that the link will belong to
	/// </summary>
	public int SectionId { get; set; }
}

public class UpdateLinkRequestValidator : AbstractValidator<UpdateLinkRequest>
{
    public UpdateLinkRequestValidator()
    {
        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("Section is required.");

        RuleFor(x => x.LinkText)
            .NotEmpty().WithMessage("Link text is required.")
            .MaximumLength(500).WithMessage("Title is long.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description is long.");
    }
}

public class UpdateLinkRequestProfile : Profile
{
    public UpdateLinkRequestProfile()
    {
        CreateMap<UpdateLinkRequest, UpdateLinkModel>();
    }
}