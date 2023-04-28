namespace PromoWeb.Api.Contacts;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using PromoWeb.Services.Contacts;

public class UpdateContactRequest
{
	/// <summary>
	/// Whose contact is this
	/// </summary>
	public string ContactOwner { get; set; } = string.Empty;

	/// <summary>
	/// Contact email
	/// </summary>
	public string? Email { get; set; }

	/// <summary>
	/// Contact address
	/// </summary>
	public string? Address { get; set; }

	/// <summary>
	/// Contact website
	/// </summary>
	public string? WebSite { get; set; }

	/// <summary>
	/// Contact phone
	/// </summary>
	public string? Phone { get; set; }
}

public class UpdateContactRequestValidator : AbstractValidator<UpdateContactRequest>
{
    public UpdateContactRequestValidator()
    {
        RuleFor(x => x.ContactOwner)
            .NotEmpty().WithMessage("Contact owner is required.")
            .MaximumLength(50).WithMessage("ContactOwner name is long.");

        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("Email is long.")
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email");

        RuleFor(x => x.WebSite)
            .MaximumLength(200).WithMessage("WebSite is long.");
    }
}

public class UpdateContactRequestProfile : Profile
{
    public UpdateContactRequestProfile()
    {
        CreateMap<UpdateContactRequest, UpdateContactModel>();
    }
}