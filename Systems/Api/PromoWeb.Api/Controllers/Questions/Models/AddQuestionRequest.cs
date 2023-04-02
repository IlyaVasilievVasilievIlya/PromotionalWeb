namespace PromoWeb.Api.Controllers;

using AutoMapper;
using FluentValidation;
using PromoWeb.Common.Extensions;
using PromoWeb.Services.Questions;

public class AddQuestionRequest
{
    public string Text { get; set; } = string.Empty;
    public string? Email { get; set; }
}

public class AddQuestionRequestValidator : AbstractValidator<AddQuestionRequest>
{
    public AddQuestionRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(500).WithMessage("Text is too long.");

        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("Email is too long.")
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Invalid email");
    }
}

public class AddQuestionRequestProfile : Profile
{
    public AddQuestionRequestProfile()
    {
        CreateMap<AddQuestionRequest, AddQuestionModel>();
    }
}