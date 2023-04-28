namespace PromoWeb.Services.Answers;

using AutoMapper;
using FluentValidation;
using PromoWeb.Context.Entities;

/// <summary>
/// Answer request model
/// </summary>
public class AddAnswerRequest
{
    /// <summary>
    /// Question id
    /// </summary>
    public int QuestionId { get; set; }

	/// <summary>
	/// Answer text
	/// </summary>
	public string Text { get; set; } = string.Empty;
}

public class AddAnswerRequestValidator : AbstractValidator<AddAnswerRequest>
{
    public AddAnswerRequestValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("Question is required.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Responce text is required.")
            .MaximumLength(1000).WithMessage("Text is too long.");
    }
}

public class AddAnswerRequestProfile : Profile
{
    public AddAnswerRequestProfile()
    {
        CreateMap<AddAnswerRequest, AddAnswerModel>();
    }
}