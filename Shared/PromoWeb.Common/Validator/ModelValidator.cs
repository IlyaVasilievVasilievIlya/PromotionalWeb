
using FluentValidation;

namespace PromoWeb.Common.Validator
{
    public class ModelValidator<T> : IModelValidator<T> where T : class
    {
        private readonly IValidator<T> validator;

        public ModelValidator(IValidator<T> validator)
        {
            this.validator = validator;
        }

        public void Check(T model)
        {
            var result = validator.Validate(model);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public void CheckAsync(T model)
        {
            var result = validator.ValidateAsync(model);
            if (!result.Result.IsValid)
                throw new ValidationException(result.Result.Errors);
        }
    }
}
