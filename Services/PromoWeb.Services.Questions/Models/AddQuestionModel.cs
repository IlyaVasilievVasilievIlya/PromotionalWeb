namespace PromoWeb.Services.Questions;

using AutoMapper;
using FluentValidation;
using PromoWeb.Context.Entities;

public class AddQuestionModel
{
    public DateTime Date { get; set; }
    public string Text { get; set; } = string.Empty;
    public string? Email { get; set; }
}

public class AddQuestionModelValidator : AbstractValidator<AddQuestionModel>
{
    public AddQuestionModelValidator()
    {

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(500).WithMessage("Text is too long.");

        RuleFor(x => x.Email)
            .MaximumLength(100).WithMessage("Email is too long.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required");
    }
}

public class AddQuestionModelProfile : Profile
{
    public AddQuestionModelProfile()
    {
        CreateMap<AddQuestionModel, Question>();
    }
}