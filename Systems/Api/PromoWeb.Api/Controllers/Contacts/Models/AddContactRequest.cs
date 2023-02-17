namespace PromoWeb.Api.Contacts;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;
using PromoWeb.Services.Contacts;

public class AddContactRequest
{
    public string ContactOwner { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? WebSite { get; set; }
    public string? Phone { get; set; }
}

public class AddContactRequestValidator : AbstractValidator<AddContactRequest>
{
    public AddContactRequestValidator()
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

public class AddContactRequestProfile : Profile
{
    public AddContactRequestProfile()
    {
        CreateMap<AddContactRequest, AddContactModel>();
    }
}