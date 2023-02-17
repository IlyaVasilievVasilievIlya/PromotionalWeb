﻿namespace PromoWeb.Api.Controllers;

using AutoMapper;
using FluentValidation;
using PromoWeb.Services.Questions;

public class AddQuestionRequest
{
    public DateTime Date { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; //пользователь введет только свой и текст
    public string RecipientEmail { get; set; } = string.Empty;
}

public class AddQuestionRequestValidator : AbstractValidator<AddQuestionRequest>
{
    public AddQuestionRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(500).WithMessage("Text is too long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email")
            .MaximumLength(100).WithMessage("Email is too long.");

        RuleFor(x => x.RecipientEmail)
            .NotEmpty().WithMessage("RecipientEmail is required.")
            .EmailAddress().WithMessage("Invalid email")
            .MaximumLength(100).WithMessage("Email is too long.");
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required");
    }
}

public class AddQuestionRequestProfile : Profile
{
    public AddQuestionRequestProfile()
    {
        CreateMap<AddQuestionRequest, AddQuestionModel>();
    }
}