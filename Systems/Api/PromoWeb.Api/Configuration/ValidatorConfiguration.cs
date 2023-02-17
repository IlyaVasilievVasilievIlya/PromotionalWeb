using PromoWeb.Common.Validator;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using PromoWeb.Common.Helpers;
using FluentValidation.AspNetCore;
using PromoWeb.Common.Responses;
using PromoWeb.Common.Extensions;

namespace PromoWeb.Api.Configuration  //https://docs.fluentvalidation.net/en/latest/aspnet.html
{
    public static class ValidatorConfiguration
    {
        public static IMvcBuilder AddValidator(this IMvcBuilder builder) //возврат списка ошибок
        {//и это возвращается клиенту вместо того что вернул бы запрос (iactionresult тип)
            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var fieldErrors = new List<ErrorResponseFieldInfo>();
                    foreach (var (field, state) in context.ModelState)
                    {
                        if (state.ValidationState == ModelValidationState.Invalid)
                            fieldErrors.Add(new ErrorResponseFieldInfo()
                            {
                                FieldName = field.ToCamelCase(), //привести в один вид
                                Message = string.Join(", ", state.Errors.Select(x => x.ErrorMessage))
                            });
                    }

                    var result = new BadRequestObjectResult(new ErrorResponse()
                    {
                        ErrorCode = 100,
                        Message = "One or more validation errors occurred.",
                        FieldErrors = fieldErrors
                    });

                    return result;
                };
            });

            //те для валидации нужен mvcbuilder а не app в обычный контейнер не добавить 

            builder.AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.ImplicitlyValidateChildProperties = true;
                fv.AutomaticValidationEnabled = true;
            });

            FluentValidatorHelper.Register(builder.Services);
            builder.Services.AddSingleton(typeof(IModelValidator<>), typeof(ModelValidator<>));

            return builder;
        }
    }
}
