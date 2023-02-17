﻿namespace PromoWeb.Services.Links;

using AutoMapper;
using PromoWeb.Context.Entities;
using FluentValidation;

public class UpdateLinkModel
{
    public string LinkText { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int SectionId { get; set; }
}

public class UpdateLinkModelValidator : AbstractValidator<UpdateLinkModel>
{
    public UpdateLinkModelValidator()
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

public class UpdateLinkModelProfile : Profile
{
    public UpdateLinkModelProfile()
    {
        CreateMap<UpdateLinkModel, Link>();
    }
}