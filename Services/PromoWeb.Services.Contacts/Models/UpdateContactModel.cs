namespace PromoWeb.Services.Contacts;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;

public class UpdateContactModel
{
    public string ContactOwner { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? WebSite { get; set; }
    public string? Phone { get; set; }
}

public class UpdateContactModelValidator : AbstractValidator<UpdateContactModel>
{
    public UpdateContactModelValidator()
    {
        RuleFor(x => x.ContactOwner)
            .NotEmpty().WithMessage("Contact owner is required.")
            .MaximumLength(50).WithMessage("ContactOwner name is long.");

        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("Email is long.");

        RuleFor(x => x.WebSite)
            .MaximumLength(200).WithMessage("WebSite is long.");
    }
}

public class UpdateContactModelProfile : Profile
{
    public UpdateContactModelProfile()
    {
        CreateMap<UpdateContactModel, Contact>();
    }
}