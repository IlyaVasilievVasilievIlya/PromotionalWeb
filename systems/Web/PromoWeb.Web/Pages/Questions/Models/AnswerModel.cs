using FluentValidation;

namespace PromoWeb.Web
{
    public class AnswerModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class AnswerModelValidator : AbstractValidator<AnswerModel>
    {
        public AnswerModelValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("Question is required.");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Responce text is required.")
                .MaximumLength(1000).WithMessage("Text is too long.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AnswerModel>.CreateWithOptions((AnswerModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
