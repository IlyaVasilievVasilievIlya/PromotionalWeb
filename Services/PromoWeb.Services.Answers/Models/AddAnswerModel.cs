namespace PromoWeb.Services.Answers;

using AutoMapper;
using FluentValidation;
using PromoWeb.Context.Entities;

public class AddAnswerModel
{
    public DateTime Date { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class AddAnswerModelValidator : AbstractValidator<AddAnswerModel>
{
    public AddAnswerModelValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("Question is required.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Responce text is required.")
            .MaximumLength(1000).WithMessage("Text is too long.");
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required");
    }
}

public class AddAnswerModelProfile : Profile
{
    public AddAnswerModelProfile()
    {
        CreateMap<AddAnswerModel, Answer>();
    }
}