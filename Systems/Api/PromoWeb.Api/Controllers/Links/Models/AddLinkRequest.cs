﻿namespace PromoWeb.Api.Links;

using AutoMapper;
using FluentValidation;
using PromoWeb.Services.Links;

public class AddLinkRequest
{
    public string LinkText { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 

    public int SectionId { get; set; }
}

public class AddLinkRequestValidator : AbstractValidator<AddLinkRequest>
{
    public AddLinkRequestValidator()
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

public class AddLinkRequestProfile : Profile
{
    public AddLinkRequestProfile()
    {
        CreateMap<AddLinkRequest, AddLinkModel>();
    }
}